using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static event System.Action OnBeat;
    [SerializeField] private float _bpm;
    private float _beatInterval;
    private float _beatTimer;
    private float _nextBeat;

    void Reset()
    {
        _bpm = 120f;
    }

    private void Start()
    {
        _beatTimer = 0;
    }

    private void FixedUpdate()
    {
        _beatTimer += Time.fixedDeltaTime;
        float _beatInterval = (60f / _bpm);
        
        if (_beatTimer >= _beatInterval)
        {   
            _beatTimer = 0;
            _nextBeat = Time.fixedTime + _beatInterval;
            OnBeat?.Invoke();
        }
    }

    private void Update()
    {
        Press();
    }

    private void Press()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var difference = Time.time - _nextBeat;
            Debug.Log($"Difference: {difference}");
        }
    }
}
