using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public struct PixelData {
    Vector2 position;
    Color color;
}

public class Painting : MonoBehaviour {
    
    private Texture2D _paintingTexture;
    private Texture2D _modelTexture;

    private Color _brushColor;

    private int _texWidth, _texHeight;

    public GameObject refSource;

    public Texture2D _sourceTexture;

    [SerializeField] private int _tolerance;

    private List<Vector2> _pixelDrawPosition = new List<Vector2>();
    
    void Start() {
        Image image = GetComponent<Image>();
        Sprite imageSprite = image.sprite;

        _texWidth = Screen.width;
        _texHeight = Screen.height;
        
        Debug.Log("width: " + _texWidth + " height: " + _texHeight);

        _paintingTexture = new Texture2D(_texWidth, _texHeight, TextureFormat.RGBA32, false);
    
        Color[] pixels = new Color[_texWidth * _texHeight];
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = Color.clear;
        }

        _paintingTexture.SetPixels(pixels);
        _paintingTexture.Apply(false);
        
        imageSprite = Sprite.Create(_paintingTexture,new Rect(0,0,_paintingTexture.width,_paintingTexture.height),new Vector2(0.5f,0.5f));
        image.sprite = imageSprite;
        
        _brushColor = Color.black;
        
        GenerateModelTexture(_sourceTexture);

        BrushEventsDispatcher.OnBrushMoveEvent += OnBrushMove;
        BrushEventsDispatcher.OnBrushColorChangeEvent += OnBrushChangeColor;
    }
    
    private void OnBrushMove(BrushBase currentBrush, Vector3 position) {
        int centerX = Mathf.Clamp((int)position.x - currentBrush.BrushSize / 2, 0, _paintingTexture.width);
        int centerY = Mathf.Clamp((int)position.y - currentBrush.BrushSize / 2, 0, _paintingTexture.height);
        
        currentBrush.ComputePixels();
        
        Color[] texturePixels = _paintingTexture.GetPixels(centerX, centerY, currentBrush.BrushSize, currentBrush.BrushSize);

        // Get If the current pixel is not transparent, we take the color from the spray
        for (int i = 0; i < currentBrush.BrushSize * currentBrush.BrushSize; i++) {
            if (currentBrush.Pixels[i] != Color.clear) { 
                texturePixels[i] = _brushColor;
                
                _pixelDrawPosition.Add(new Vector2(centerX + i % currentBrush.BrushSize, centerY + i / currentBrush.BrushSize));
            }
        }
        
        _paintingTexture.SetPixels(centerX, centerY, currentBrush.BrushSize, currentBrush.BrushSize, texturePixels);
        _paintingTexture.Apply(false);
    }

    private void GenerateModelTexture(Texture2D sourceTexture) {
        _modelTexture = new Texture2D(_texWidth, _texHeight, TextureFormat.RGBA32, false);

        // Generate a copy of _modelTexture just for the visual representation
        Texture2D visualModel = new Texture2D(_texWidth, _texHeight, TextureFormat.RGBA32, false);
        Color[] visualPixels = new Color[_texWidth * _texHeight];

        for (int i = 0; i < visualPixels.Length; i++) {
            visualPixels[i] = Color.white;
            _modelTexture.SetPixel(i % _texWidth, i / _texWidth, Color.white);
        }
        
        // Compute center of texture 
        int offsetX = (_texWidth - sourceTexture.width) / 2;
        int offsetY = (_texHeight - sourceTexture.height) / 2;

        Color[] sourcePixels = sourceTexture.GetPixels();
        // Background of textures 
        Color bg = new Color(25f / 255f, 63f / 255f, 74f / 255f);

        for (int y = 0; y < sourceTexture.height; y++) {
            for (int x = 0; x < sourceTexture.width; x++) {
                
                int index = y * sourceTexture.width + x;
                Color pixel = sourcePixels[index];
                
                // Compute coordinates in center of the texture
                int destX = x + offsetX;
                int destY = y + offsetY;
                int destIndex = destY * _texWidth + destX;

                // If the pixel is  background, we don't draw it
                if (Vector4.Distance(pixel, bg) < 0.1f) {
                    pixel = Color.white; 
                }

                Color fadedPixel = Color.Lerp(Color.white, pixel, 0.2f);
                fadedPixel.a = 1.0f;
                visualPixels[destIndex] = fadedPixel;
                
                _modelTexture.SetPixel(destX, destY, pixel);
            }
        }

        visualModel.SetPixels(visualPixels);
        visualModel.Apply(false);
        _modelTexture.Apply(false);
        
        Sprite modelSprite = Sprite.Create(visualModel, new Rect(0, 0, _texWidth, _texHeight), new Vector2(0.5f, 0.5f));
        refSource.GetComponent<Image>().sprite = modelSprite;
    }


    public void CompareTexture() {
    if (_modelTexture.width != _paintingTexture.width || _modelTexture.height != _paintingTexture.height) {
        Debug.LogError("The two textures have different sizes");
        return;
    }

    int width = _modelTexture.width;
    int height = _modelTexture.height;

    Color[] sourcePixels = _modelTexture.GetPixels();
    Color[] paintingPixels = _paintingTexture.GetPixels();

    int totalToCompare = 0;
    int matchedPixels = 0;

    for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
            int index = y * width + x;

            Color sourcePixel = sourcePixels[index];
            Color paintingPixel = paintingPixels[index];

            // Get if current pixel is white or background or transparent to reduce the number of pixels to compare
            bool sourceIsColored = !IsTransparentOrBackground(sourcePixel);
            bool paintingIsColored = !IsTransparentOrBackground(paintingPixel);

            if (sourceIsColored || paintingIsColored) {
                totalToCompare++;

                if (_tolerance == 0) {
                    if (ColorsAreSimilar(sourcePixel, paintingPixel, 0.1f)) {
                        matchedPixels++;
                    }
                } else {
                    bool matchFound = false;

                    // Check in nearest pixel if the color is similar
                    for (int offsetY = -_tolerance; offsetY <= _tolerance && !matchFound; offsetY++) {
                        for (int offsetX = -_tolerance; offsetX <= _tolerance && !matchFound; offsetX++) {
                            int neighborX = x + offsetX;
                            int neighborY = y + offsetY;

                            if (neighborX >= 0 && neighborY >= 0 && neighborX < width && neighborY < height) {
                                int nIndex = neighborY * width + neighborX;
                                Color neighborPixel = paintingPixels[nIndex];
                                
                                // if the neighbor pixel is correct, we accept it
                                if (ColorsAreSimilar(sourcePixel, neighborPixel, 0.1f)) {
                                    matchFound = true;
                                    matchedPixels++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    float accuracy = totalToCompare > 0 ? (float)matchedPixels / totalToCompare * 100f : 100f;
    Debug.Log($"🎯 Précision du dessin : {accuracy:F2}% ({matchedPixels}/{totalToCompare} pixels comparés)");
}


    
    private void OnBrushChangeColor(Color color) {
        _brushColor = color;
    }
    
    bool IsTransparentOrBackground(Color c) {
        Color bg = new Color(25 / 255f, 63 / 255f, 74 / 255f, 1f);
        return c.a < 0.01f || Vector4.Distance(c, bg) < 0.1f || c == Color.white;
    }
    
    bool ColorsAreSimilar(Color a, Color b, float tolerance) {
        return Vector4.Distance(a, b) < tolerance;
    }
    
    

}
