using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class RythmTimeLine : MonoBehaviour
{
    public static event System.Action OnBeat;

    [SerializeField] List<KeyCode> Player1Inputs = new List<KeyCode>();
    [SerializeField] PlayableDirector _timeLine;
    [SerializeField] TextMeshProUGUI _text;


    bool _isPlaying = false;
    bool _isPaused = false;

    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Z, KeyCode.E },
        new KeyCode[] { KeyCode.U, KeyCode.Y },
        new KeyCode[] { KeyCode.L, KeyCode.M }
    };
    int _randomKeyCode = 0;
    int CheckInput()
    {
        int isSuccess = 0;
        foreach (KeyCode key in Player1Inputs)
        {
            Debug.Log(key);
            if (Input.GetKey(key))
            {
                if (_keyCodes[_randomKeyCode].Contains(key)) isSuccess++;
                else isSuccess--;
            }
        }
        return isSuccess;
    }

    public void DoOnBeat()
    {
        _text.text = "";
        OnBeat?.Invoke();
        Debug.Log(CheckInput());
        bool test = CheckInput() == _keyCodes[_randomKeyCode].Length - 1;
        if (test)
        {
            _text.text = "Good";
            SelectNewInputs();
        }
    }
    private void SelectNewInputs()
    {
        _randomKeyCode = Random.Range(0, _keyCodes.Count);
        Debug.Log("Changed  " + _randomKeyCode);
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
