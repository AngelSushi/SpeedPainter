using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour {
    private TextMeshProUGUI _text;
    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
        GameEventsDispatcher.OnLevelStartEvent += OnLevelStart;
    }

    private void OnLevelStart(int levelIndex,Level level) {
        _text.text = levelIndex.ToString();
    }

}
