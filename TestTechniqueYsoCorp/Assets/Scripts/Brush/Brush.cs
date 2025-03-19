using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {
    
    void Update() {
        if (Input.touches.Length > 0) {
            Vector3 position = Input.touches[0].position;
            
           /* Vector2 touchPosition = Camera.main.ScreenToWorldPoint(position);
            Vector2 correctPosition = new Vector2(position.x, position.y);

            Debug.Log("position :" + position);
            */
            BrushEventsDispatcher.OnBrushMoveEvent?.Invoke(position);
        }
    }
}
