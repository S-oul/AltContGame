using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static event System.Action OnBeat;
    [SerializeField] private float _bpm = 120f;
    private float _beatInterval;
    private float _beatTimer;

    private void FixedUpdate()
    {
        _beatTimer += Time.fixedDeltaTime;
        float _beatInterval = (60f / _bpm);
        if (_beatTimer >= _beatInterval)
        {   
            _beatTimer -= _beatInterval;
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
            var difference = Mathf.Abs(_beatTimer - _beatInterval);
            Debug.Log($"Difference: {difference}");
        }
    }
}
