using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : BrushBase {

    private int _sprayRadius;
    private int _sprayDensity;
    
    
    public Spray(int brushSize) : base(brushSize) {
        _sprayRadius = brushSize / 2;
        _sprayDensity = 10000;
    }

    public override void ComputePixels() {
        _pixels = new List<Color>(new Color[BrushSize * BrushSize]); // Init List with Color.clear by default 
        
        for (int i = 0; i < _sprayDensity; i++) { // Generate random particles
            float angle = Random.Range(0.0f, Mathf.PI * 2);
            float radius = Random.Range(0.0f, _sprayRadius);
            
            // Compute coordinates of the particle
            int xCoord = (int)(Mathf.Cos(angle) * radius); 
            int yCoord = (int)(Mathf.Sin(angle) * radius); 

            // Center the particle in the brush
            int pixelX = (BrushSize / 2) + xCoord;  
            int pixelY = (BrushSize / 2) + yCoord;

            if (pixelX >= 0 && pixelX < BrushSize && pixelY >= 0 && pixelY < BrushSize) {
                int index = pixelY * BrushSize + pixelX;
                _pixels[index] = Color.black;
            }
        }
    }

    public override bool CanMixColors() {
        return false;
    }

    public override void MixColors() {
        Debug.Log("mix colors");
    }
}
