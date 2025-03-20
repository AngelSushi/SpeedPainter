using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : BrushBase {
    public Pencil(int brushSize) : base(brushSize) {}

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

}
