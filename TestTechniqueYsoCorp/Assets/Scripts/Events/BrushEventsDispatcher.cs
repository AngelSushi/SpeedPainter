using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EventPriority {
    HIGH,
    LOW
}


public class EventAction {
    public Delegate action;
    public object[] args;
    public EventPriority priority;
}

public class BrushEventsDispatcher : MonoBehaviour {
    public static Action<BrushBase,Vector3> OnBrushMoveEvent;
    public static Action<int> OnBrushChangeEvent;
    public static Action<Color> OnBrushColorChangeEvent;
    public static Action OnScaleUpBrushEvent;
    public static Action OnScaleDownBrushEvent;

    // Queue is old system and have no value right but no time to change it
    private static Queue<EventAction> _eventQueue = new Queue<EventAction>();
    
    public static void InvokeEvent(Delegate eventAction,EventPriority priority,params object[] args) {
        if (priority == EventPriority.HIGH) {
            Queue<EventAction> filtered = new Queue<EventAction>();

            while (_eventQueue.Count > 0) {
                EventAction action = _eventQueue.Dequeue();

                if (action.priority == EventPriority.HIGH) {
                    filtered.Enqueue(action);
                }
            }

            _eventQueue = filtered;
        }

        _eventQueue.Enqueue(new EventAction {
            action = eventAction,
            priority = priority,
            args = args
        });
    }

    private void LateUpdate() {
        while (_eventQueue.Count > 0) {
            EventAction action = _eventQueue.Dequeue();
            action.action.DynamicInvoke(action.args);
        }
        
        _eventQueue.Clear();
    }
}
