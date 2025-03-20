using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {

    private Spray _spray;
    private Pencil _pencil;
    
    void Start() {
        _spray = new Spray(200);
        _pencil = new Pencil(10);
    }
    
    
    void Update() {
        if (Input.touches.Length > 0) {
            Vector3 position = Input.touches[0].position;
            
           /* Vector2 touchPosition = Camera.main.ScreenToWorldPoint(position);
            Vector2 correctPosition = new Vector2(position.x, position.y);

            Debug.Log("position :" + position);
            */
            BrushEventsDispatcher.OnBrushMoveEvent?.Invoke(_spray,position);
            
          /*  _spray.ComputePixels();
            _spray.MixColors();
            _spray.CanMixColors();
            */
        }
    }
}
