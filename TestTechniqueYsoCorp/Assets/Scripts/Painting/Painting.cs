using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PixelData {
    Vector2 position;
    Color color;
}

public class Painting : MonoBehaviour {
    
    private Texture2D _paintingTexture;

    private List<PixelData> _pixelsData;
    
    public List<PixelData> PixelsData {
        get { return _pixelsData; }
        set { _pixelsData = value; }
    }
    
    private bool _isPainting = false;

    private int _brushSize = 200;

    private Color[] _brushPixels;

    private Color _brushColor;
    
    
    void Start() {
        Image image = GetComponent<Image>();
        Sprite imageSprite = image.sprite;

        int texWidth = Screen.width;
        int texHeight = Screen.height;

        _paintingTexture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
    
        Color[] pixels = new Color[texWidth * texHeight];
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = Color.white;
        }

        _paintingTexture.SetPixels(pixels);
        _paintingTexture.Apply(false);
        
        
        imageSprite = Sprite.Create(_paintingTexture,new Rect(0,0,_paintingTexture.width,_paintingTexture.height),new Vector2(0.5f,0.5f));
        image.sprite = imageSprite;
        
        _brushPixels = new Color[_brushSize * _brushSize];

        _brushPixels = new Color[_brushSize * _brushSize];
        int radius = _brushSize / 2;
        int radiusSquared = radius * radius;

        for (int y = 0; y < _brushSize; y++) {
            for (int x = 0; x < _brushSize; x++) {
                int dx = x - radius;
                int dy = y - radius;

                // Si le pixel est dans le cercle, on met la couleur du pinceau, sinon transparent
                if (dx * dx + dy * dy <= radiusSquared) {
                    _brushPixels[y * _brushSize + x] = Color.red;
                } else {
                    _brushPixels[y * _brushSize + x] = Color.clear;
                }
            }
        }

        _brushColor = Color.red;

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
            }
        }
        
        _paintingTexture.SetPixels(centerX, centerY, currentBrush.BrushSize, currentBrush.BrushSize, texturePixels);
        _paintingTexture.Apply(false);
    }

    private void OnBrushChangeColor(Color color) {
        _brushColor = color;
    }

}
