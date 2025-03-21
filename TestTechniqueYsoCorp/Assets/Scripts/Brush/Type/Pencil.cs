using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : BrushBase {
    public Pencil(int brushSize) : base(brushSize) {
        BrushEventsDispatcher.OnScaleUpBrushEvent += OnScaleUpBrush;
        BrushEventsDispatcher.OnScaleDownBrushEvent += OnScaleDownBrush;
    }

    public override void ComputePixels() {
        for (int y = 0; y < BrushSize; y++) {
            for (int x = 0; x < BrushSize; x++) {
                _pixels.Add(Color.black);
            }
        }
    }

    public override bool CanMixColors() {
        return true;
    }

    public override void MixColors() {
        Debug.Log("mix colors");
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
