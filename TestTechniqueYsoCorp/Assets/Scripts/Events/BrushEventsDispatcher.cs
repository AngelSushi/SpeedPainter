using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushEventsDispatcher : MonoBehaviour {
    public static Action<BrushBase,Vector3> OnBrushMoveEvent;
    public static Action<int> OnBrushChangeEvent;
    public static Action<Color> OnBrushColorChangeEvent;
    public static Action OnScaleUpBrushEvent;
    public static Action OnScaleDownBrushEvent;


}
