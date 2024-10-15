using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class RythmTimeLine : MonoBehaviour
{
    public static event System.Action OnBeat;

    public List<KeyCode> KeyCodesAvailable = new List<KeyCode>();
    public PlayableDirector _timeLine;
    
    bool _isPlaying = false;
    bool _isPaused = false;

    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Z, KeyCode.E },
        new KeyCode[] { KeyCode.U, KeyCode.Y },
        new KeyCode[] { KeyCode.L, KeyCode.M }
    };
    int _randomKeyCode = 0;
    bool CheckInput()
    {
        string aaaa = "";
        foreach (KeyCode keyCode in KeyCodesAvailable)
        {
            if (Input.GetKey(keyCode))
            {
                aaaa+= keyCode.ToString();
            }
        }
        Debug.Log(aaaa);
        int reussite = 0;
        foreach (KeyCode key in _keyCodes[_randomKeyCode])
        {
            if (Input.GetKey(key))
            {
                reussite++;
            }
        }
        return reussite == _keyCodes[_randomKeyCode].Length - 1;
    }

    public void DoOnBeat()
    {
        OnBeat?.Invoke();
        if (CheckInput())
        {
            SelectNewInputs();
        }
    }
    private void SelectNewInputs()
    {
        _randomKeyCode = Random.Range(0, _keyCodes.Count);
        Debug.Log("Changed  " +  _randomKeyCode);
    }

    // Update is called once per frame
    void Update()
    { 
        if (!_isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            _isPlaying = true;
            _timeLine.Play();
        }
    }
    private void OnGUI()
    {
        GUILayout.Label(_keyCodes[_randomKeyCode][0].ToString());
        GUILayout.Label(_keyCodes[_randomKeyCode][1].ToString());
    }
}
