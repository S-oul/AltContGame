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

    [SerializeField] HypeMeter hypeMeter;

    bool _isPlayer1Turn = false;


    [SerializeField] List<KeyCode> Player1Inputs = new List<KeyCode>();
    [SerializeField] List<KeyCode> Player2Inputs = new List<KeyCode>();
    [SerializeField] PlayableDirector _timeLine;
    [SerializeField] TextMeshProUGUI _sucessText;
    [SerializeField] TextMeshProUGUI _inputText;



    bool _isPlaying = false;
    bool _isPaused = false;

    /// ALL LEFT HAND POSES
    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.R }, // MAJEUR 
        new KeyCode[] { KeyCode.Q, KeyCode.W }, // Finger Gun
        new KeyCode[] { KeyCode.W, KeyCode.E }, //Corne Du Diable
        new KeyCode[] { KeyCode.R}, //Index
        new KeyCode[] { KeyCode.Q}, //CallMe
        new KeyCode[] { KeyCode.W, KeyCode.Q, KeyCode.E, KeyCode.R  }, //Poing
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
        bool fullSucses = intSuccess == _keyCodes[_randomKeyCode].Length;
        if (fullSucses)
        {
            _sucessText.color = new Color(1, 1, 1, 1);
            _sucessText.text = "Good";
            SelectNewInputs();
            if (_isPlayer1Turn)
            {
                hypeMeter._winOMeter += .1f/2;
            }
            else hypeMeter._winOMeter -= .1f/2;
        }
        else
        {
            if (!_isPlayer1Turn)
            {
                hypeMeter._winOMeter += .1f / 2;
            }
            else hypeMeter._winOMeter -= .1f / 2;
        }

        _isPlayer1Turn = !_isPlayer1Turn;
    }
    private void SelectNewInputs()
    {
        _inputText.text = "";
        _randomKeyCode = Random.Range(0, _keyCodes.Count);
        foreach (KeyCode key in _keyCodes[_randomKeyCode])
            _inputText.text += key + " ";
    }

    private void Start()
    {
        SelectNewInputs();
    }
    void Update()
    {
        if (_sucessText.color.a > 0) _sucessText.color = new Color(1, 1, 1, _sucessText.color.a - 0.01f);
        if (!_isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            _isPlaying = true;
            _timeLine.Play();
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("CONVERSION QWERTY DE SES MORTS -- JOUEZ EN FULL HD 1920/1080");
    }
}
