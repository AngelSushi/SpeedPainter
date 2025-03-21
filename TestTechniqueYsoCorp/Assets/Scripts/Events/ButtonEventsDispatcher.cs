using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventsDispatcher : MonoBehaviour {

    private Color[] _availableColors = {Color.red, Color.green, Color.blue, Color.yellow, Color.black, Color.white};
    
    public void OnChoosePencilBrush() {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnBrushChangeEvent,EventPriority.HIGH,0);
    }
    public void OnChooseCircleBrush() {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnBrushChangeEvent,EventPriority.HIGH,1);
    }
    
    public void OnChooseSprayBrush() {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnBrushChangeEvent,EventPriority.HIGH,2);
    }

    public void OnChooseBrushColor(int colorIndex) {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnBrushColorChangeEvent,EventPriority.HIGH,_availableColors[colorIndex]);
    }

    public void OnScaleUpBrush() {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnScaleUpBrushEvent,EventPriority.HIGH);
    }

    public void OnScaleDownBrush() {
        BrushEventsDispatcher.InvokeEvent(BrushEventsDispatcher.OnScaleDownBrushEvent,EventPriority.HIGH);
    }

    public void PendingEraseButton() {
        Debug.Log("test1");
        FindObjectOfType<Painting>().CompareTexture();
    }
}
