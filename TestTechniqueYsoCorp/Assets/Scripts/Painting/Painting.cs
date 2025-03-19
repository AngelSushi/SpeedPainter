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

    private int _brushSize = 10;

    private Color[] _brushPixels;
    
    
    void Start() {
        Image image = GetComponent<Image>();
        Sprite imageSprite = image.sprite;
        
        
        _paintingTexture = new Texture2D(imageSprite.texture.width, imageSprite.texture.height, TextureFormat.RGBA32, false);

        Color[] pixels = imageSprite.texture.GetPixels();
        _paintingTexture.SetPixels(pixels);
        _paintingTexture.Apply(false); // A quoi serrt mimmaps ? 
        
        imageSprite = Sprite.Create(_paintingTexture,new Rect(0,0,_paintingTexture.width,_paintingTexture.height),new Vector2(0.5f,0.5f));
        image.sprite = imageSprite;
        
        _brushPixels = new Color[_brushSize * _brushSize];
        for (int i = 0; i < _brushPixels.Length; i++) {
            _brushPixels[i] = Color.red;
        }
        
        BrushEventsDispatcher.OnBrushMoveEvent += OnBrushMove;
    }

    private void OnBrushMove(Vector3 position) {

        RectTransform rectTransform = GetComponent<Image>().rectTransform;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,position, null, out localPoint)) {
            
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            
            int x = Mathf.Clamp((int)((localPoint.x + width * 0.5f) / width * _paintingTexture.width), 0, _paintingTexture.width - _brushSize);
            int y = Mathf.Clamp((int)((localPoint.y + height * 0.5f) / height * _paintingTexture.height), 0, _paintingTexture.height - _brushSize);
            
            _paintingTexture.SetPixels(x,y,_brushSize,_brushSize,_brushPixels);
            _paintingTexture.Apply(false);
            
        }
    }
    
    
}
