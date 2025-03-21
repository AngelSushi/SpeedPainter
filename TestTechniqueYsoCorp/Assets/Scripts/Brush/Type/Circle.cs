using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : BrushBase {
    public Circle(int brushSize) : base(brushSize) {
        
        BrushEventsDispatcher.OnScaleUpBrushEvent += OnScaleUpBrush;
        BrushEventsDispatcher.OnScaleDownBrushEvent += OnScaleDownBrush;
    }

    public override void ComputePixels() {
        int radius = BrushSize / 2;
        
        for(int y = 0; y < BrushSize; y++) {
            for(int x = 0; x < BrushSize; x++) {
                int dx = x - radius;
                int dy = y - radius;
                
                if (dx * dx + dy * dy <= radius * radius) {
                    _pixels.Add(Color.black);
                } else {
                    _pixels.Add(Color.clear);
                }
            }
        }
    }

    public override bool CanMixColors() {
        return true;
    }

    public override void MixColors() {
    }
    
    private void OnScaleUpBrush() {
        BrushSize += 10;
        BrushSize = Mathf.Clamp(BrushSize,0,200);
    }
    
    private void OnScaleDownBrush() {
        BrushSize -= 10;
        BrushSize = Mathf.Clamp(BrushSize, 0, 200);
    }
}
