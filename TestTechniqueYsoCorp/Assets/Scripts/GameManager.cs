using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {

    public int time;
    public List<Texture2D> _paintings;
}
public class GameManager : MonoBehaviour  {
   
    [SerializeField]
    private List<Level> _levels;
    
    private int _currentLevelIndex = 0;
    private Level _currentLevel;
    private float _currentLevelTime = 0;
    
    void Start() {
        _currentLevel = _levels[_currentLevelIndex];
        GameEventsDispatcher.OnLevelStartEvent?.Invoke(_currentLevelIndex + 1,_currentLevel);
    }

    void Update() {
        _currentLevelTime += Time.deltaTime;
        GameEventsDispatcher.OnLevelTimeUpdateEvent?.Invoke(_currentLevel.time - _currentLevelTime);
        
        if (_currentLevelTime >= _levels[_currentLevelIndex].time) {
            _currentLevelIndex++;
            _currentLevelTime = 0;
        }
    }
}
