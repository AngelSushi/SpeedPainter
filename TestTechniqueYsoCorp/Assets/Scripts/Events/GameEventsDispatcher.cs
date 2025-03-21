using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsDispatcher : MonoBehaviour {

    public static Action<int,Level> OnLevelStartEvent;
    public static Action<int> OnLevelEndEvent;
    public static Action<float> OnLevelTimeUpdateEvent;

}
