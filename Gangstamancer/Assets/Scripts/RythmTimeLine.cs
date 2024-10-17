using NaughtyAttributes;
using System;
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

    [SerializeField] AudioClip targetClip;

    [SerializeField] List<KeyCode> Player1Inputs = new List<KeyCode>();
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] List<KeyCode> Player2Inputs = new List<KeyCode>();
    [SerializeField] PlayerHandsInput _player2Inputs;
    [SerializeField] PlayableDirector _timeLine;

    [Header("UI Players")]
    [SerializeField] TextMeshProUGUI _sucessTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer1;
    [SerializeField] TextMeshProUGUI _sucessTextPlayer2;
    [SerializeField] TextMeshProUGUI _inputTextPlayer2;

    [SerializeField] private HandsSequence _player1HandSequence;
    [SerializeField] private HandsSequence _player2HandSequence;

    [SerializeField] private HandsSequence _currentHandsSequence;


    bool _isPlaying = false;
    bool _isPaused = false;

    /// ALL LEFT HAND POSES
    [SerializeField] private List<Fingers> _allFingers;
    [SerializeField] List<KeyCode> _keyCodes;
    int CheckInput()
    {
        int isSuccess = 0;

        List<KeyCode> toUse = _isPlayer1Turn ? _player1Inputs.playerInputs : _player2Inputs.playerInputs;

        foreach (KeyCode key in toUse)
        {
            if (Input.GetKey(key))
            {
                if (_keyCodes.Contains(key)) 
                    isSuccess++;
                else isSuccess--;
            }
        }
        return isSuccess;
    }

    public void DoOnBeat()
    {
        TextMeshProUGUI oldSuccessText = _isPlayer1Turn ? _sucessTextPlayer2 : _sucessTextPlayer1;
        oldSuccessText.text = "";
        TextMeshProUGUI _successTextCurrentPlayer = _isPlayer1Turn ? _sucessTextPlayer1 : _sucessTextPlayer2;

        _successTextCurrentPlayer.text = "";
        OnBeat?.Invoke();
        int intSuccess = CheckInput();
        bool fullSucses = intSuccess == _keyCodes.Count;
        if (fullSucses)
        {
            _successTextCurrentPlayer.color = new Color(1, 1, 1, 1);
            _successTextCurrentPlayer.text = "Good";
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


        GameManager.Instance.ChangeNextStatePlayer();
        _isPlayer1Turn = GameManager.Instance.PlayerTurn() == 1;
        SelectNewInputs();
    }
    private void SelectNewInputs()
    {
        ClearTextInput();
        _currentHandsSequence.handSigns.RemoveAt(0); // remove the handsign that was just played

        _currentHandsSequence = _isPlayer1Turn ? _player1HandSequence : _player2HandSequence;
        _currentHandsSequence.CreateRandomHandSign(_isPlayer1Turn ? HandsSign.PlayerNumber.Player1: HandsSign.PlayerNumber.Player2);
        _keyCodes = _currentHandsSequence.handSigns[0].KeyCodesFingers;


        TextMeshProUGUI inputText = _isPlayer1Turn ? _inputTextPlayer1 : _inputTextPlayer2;
        inputText.text = "";

        foreach (KeyCode key in _keyCodes)
            inputText.text += key + " ";

    }

    private void Start()
    {
        int bpm = UniBpmAnalyzer.AnalyzeBpm(targetClip);
        Debug.Log("BPM is " + bpm);

        CreateNewSequenceAtStart();
        SelectNewInputs();
    }

    private void CreateNewSequenceAtStart()
    {
        _isPlayer1Turn = GameManager.Instance.PlayerTurn() == 1;
        _currentHandsSequence = _isPlayer1Turn ? _player1HandSequence : _player2HandSequence;
        _player1HandSequence.handSigns.Clear();
        _player1HandSequence.CreateRandomHandSign(HandsSign.PlayerNumber.Player1);
        _player1HandSequence.CreateRandomHandSign(HandsSign.PlayerNumber.Player1);
        _player2HandSequence.handSigns.Clear();
        _player2HandSequence.CreateRandomHandSign(HandsSign.PlayerNumber.Player2);
        _player2HandSequence.CreateRandomHandSign(HandsSign.PlayerNumber.Player2);
    }

    void Update()
    {
        if (_sucessTextPlayer1.color.a > 0) _sucessTextPlayer1.color = new Color(1, 1, 1, _sucessTextPlayer1.color.a - 0.01f);
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

    private void ClearTextInput()
    {
        _inputTextPlayer1.text = "";
        _inputTextPlayer2.text = "";
    }
}
