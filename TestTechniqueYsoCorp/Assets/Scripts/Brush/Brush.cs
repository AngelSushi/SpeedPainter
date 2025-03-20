using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {

    private Spray _spray;
    private Pencil _pencil;
    private Circle _circle;

    private BrushBase _currentBrush;
    
    void Start() {
        _spray = new Spray(200);
        _pencil = new Pencil(10);
        _circle = new Circle(10);

        _currentBrush = _pencil;
        BrushEventsDispatcher.OnBrushChangeEvent += OnBrushChange;
    }
    
    
    void Update() {
        if (Input.touches.Length > 0) {
            Vector3 position = Input.touches[0].position;
            BrushEventsDispatcher.OnBrushMoveEvent?.Invoke(_currentBrush,position);
        }
    }
    private void OnBrushChange(int brushIndex) {
        switch (brushIndex) {
            default:
            case 0:
                _currentBrush = _pencil;
                break;
            
            case 1:
                _currentBrush = _circle;
                break;
            
            case 2:
                _currentBrush = _spray;
                break;
        }
    }
}
