using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventsDispatcher : MonoBehaviour {

    private Color[] _availableColors = {Color.red, Color.green, Color.blue, Color.yellow, Color.black, Color.white};
    
    public void OnChoosePencilBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(0);
    }
    public void OnChooseCircleBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(1);
    }
    
    public void OnChooseSprayBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(2);
    }

    public void OnChooseBrushColor(int colorIndex) {
        BrushEventsDispatcher.OnBrushColorChangeEvent?.Invoke(_availableColors[colorIndex]);
    }
}
