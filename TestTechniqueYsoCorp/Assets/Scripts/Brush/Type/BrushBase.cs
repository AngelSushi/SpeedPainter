using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrushBase {
    protected List<Color> _pixels;
    private int _brushSize;

    public int BrushSize {
        get => _brushSize;
        set {
            _brushSize = value;
           // _pixels = new List<Color>(value * value);
            ComputePixels();
        }
    }

    public List<Color> Pixels {
        get => _pixels;
    }

    public BrushBase(int brushSize) {
      //  _pixels = new List<Color>(brushSize * brushSize);
        _pixels = new List<Color>();
        BrushSize = brushSize;
    }
    
    public abstract void ComputePixels();
    public abstract bool CanMixColors();
    public abstract void MixColors();
    
}
