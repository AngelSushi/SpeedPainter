using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour {
    
    private TextMeshProUGUI _text;
    
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
        GameEventsDispatcher.OnLevelTimeUpdateEvent += OnLevelTimeUpdate;
    }

    private void OnLevelTimeUpdate(float time) {
        _text.text = ((int)time).ToString();
    }
    
}
