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
            pixels[i] = Color.white;
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
        _modelTexture = new Texture2D(_texWidth,_texHeight,TextureFormat.RGBA32,false);
        
        Color[] clearPixels = new Color[_texWidth * _texHeight];
       // for (int i = 0; i < clearPixels.Length; i++)
         //   clearPixels[i] = Color.white; 

        _modelTexture.SetPixels(clearPixels);
        
        
        int offsetX = (_texWidth - sourceTexture.width) / 2;
        int offsetY = (_texHeight - sourceTexture.height) / 2;

        Color[] sourcePixels = sourceTexture.GetPixels();
        for (int y = 0; y < sourceTexture.height; y++) {
            for (int x = 0; x < sourceTexture.width; x++) {
                Color pixel = sourcePixels[y * sourceTexture.width + x];
                _modelTexture.SetPixel(x + offsetX, y + offsetY, pixel);
            }
        }
        
        _modelTexture.Apply(false);
        
        Sprite modelSprite = Sprite.Create(_modelTexture,new Rect(0,0,_modelTexture.width,_modelTexture.height),new Vector2(0.5f,0.5f));
        refSource.GetComponent<Image>().sprite = modelSprite;
    }

    public void CompareTexture() {
        if(_modelTexture.width != _paintingTexture.width || _modelTexture.height != _paintingTexture.height) {
            Debug.LogError("Textures have different sizes");
            return;
        }

        Color[] sourcePixels = _modelTexture.GetPixels();
        Color[] paintingPixels = _paintingTexture.GetPixels();
        
        int matchingPixels = 0;
        
        Debug.Log("sourcePixels: " + sourcePixels.Length + " pixelDrawPosition: " + _pixelDrawPosition.Count);
        
       /* for(int y = 0; y < _modelTexture.height; y++) {
            for(int x = 0; x < _modelTexture.width; x++) {
                bool foundPixel = false;
                
                Debug.Log("y: " + y + " x: " + x);
                
                int checkIndex = y * _modelTexture.width + x;

                /*for (int i = 0; i < _pixelDrawPosition.Count; i++) {
                    int drawIndex = (int)_pixelDrawPosition[i].y * _modelTexture.width + (int)_pixelDrawPosition[i].x;

                    float distance = Mathf.Abs(drawIndex - checkIndex);
                    Debug.Log("distance: " + distance);
                }
            }
        }*/
        
        int percentageSucess = (int)((float)matchingPixels / (_modelTexture.width * _modelTexture.height) * 100);
        Debug.Log("percentageSuccess: " + percentageSucess);
    }
    
    private void OnBrushChangeColor(Color color) {
        _brushColor = color;
    }

}
