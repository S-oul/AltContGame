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
    [SerializeField] List<KeyCode> Player2Inputs = new List<KeyCode>();
    [SerializeField] PlayableDirector _timeLine;
    [SerializeField] TextMeshProUGUI _sucessText;
    [SerializeField] TextMeshProUGUI _inputText;



    bool _isPlaying = false;
    bool _isPaused = false;

    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Q, KeyCode.W },
        new KeyCode[] { KeyCode.W, KeyCode.E },
        new KeyCode[] { KeyCode.E, KeyCode.R }
    };
    int _randomKeyCode = 0;
    int CheckInput()
    {
        int isSuccess = 0;
        foreach (KeyCode key in Player1Inputs)
        {
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
        _sucessText.text = "";
        OnBeat?.Invoke();
        int intSuccess = CheckInput();
        bool test = intSuccess == _keyCodes[_randomKeyCode].Length;
        if (test)
        {
            _sucessText.text = "Good";
            SelectNewInputs();
        }
    }
    private void SelectNewInputs()
    {
        _inputText.text = "";
        _randomKeyCode = Random.Range(0, _keyCodes.Count);
        foreach (KeyCode key in _keyCodes[_randomKeyCode])
        _inputText.text += key + " ";
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
