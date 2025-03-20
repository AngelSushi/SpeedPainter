using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventsDispatcher : MonoBehaviour  {

    public void OnChoosePencilBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(0);
    }
    public void OnChooseCircleBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(1);
    }
    
    public void OnChooseSprayBrush() {
        BrushEventsDispatcher.OnBrushChangeEvent?.Invoke(2);
    }
}
