using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using static GameManager;

public class RythmTimeLine : MonoBehaviour
{
    public static event System.Action OnBeat;

    [SerializeField] HypeMeter hypeMeter;

    bool _isPlayer1Turn = false;

    [SerializeField] PlayableDirector _timeLine;

    [Header("Players Inputs")]
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] PlayerHandsInput _player2Inputs;

    [Header("UI Players")]
    [SerializeField] TextMeshProUGUI _sucessTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer1;
    [SerializeField] TextMeshProUGUI _sucessTextPlayer2;
    [SerializeField] TextMeshProUGUI _inputTextPlayer2;

    [Header("Players Hands Sequences")]
    [SerializeField] private HandsSequence _player1HandSequence;
    [SerializeField] private HandsSequence _player2HandSequence;
    private HandsSequence _currentHandsSequence;
    private List<KeyCode> _currentKeyCodes;


    bool _isPlaying = false;
    bool _isPaused = false;

    bool _player1AttackSuccess = false;
    bool _player1DefenseSuccess = false;

    bool _player2AttackSuccess = false;
    bool _player2DefenseSuccess = false;


    int CheckInput()
    {
        int isSuccess = 0;

        List<KeyCode> toUse = _isPlayer1Turn ? _player1Inputs.playerInputs : _player2Inputs.playerInputs;

        foreach (KeyCode key in toUse)
        {
            if (Input.GetKey(key))
            {
                if (_currentKeyCodes.Contains(key)) 
                    isSuccess++;
                else isSuccess--;
            }
        }
        return isSuccess;
    }

    public void DoOnBeat()
    {
        _sucessTextPlayer1.text = "";
        OnBeat?.Invoke();
        int intSuccess = CheckInput();
        bool fullSucses = intSuccess == _currentKeyCodes.Count;
        if (fullSucses || Input.GetKey(KeyCode.Space))
        {

            switch (GameManager.Instance.CurrentState)
            {
                case GameStates.Player1Defense:
                    _player1DefenseSuccess = true;
                    print("P1 Success Defense");
                    break;

                case GameStates.Player1Attack:
                    _player1AttackSuccess = true;
                    print("P1 Sucess A");


                    FouleUnitaire.Instance.AddLeftFan();
                    break;

                case GameStates.Player2Defense:
                    _player2DefenseSuccess = true;
                    print("P2 Success D");


                    break;

                case GameStates.Player2Attack:
                    _player2AttackSuccess = true;
                    print("P2 Success A");


                    FouleUnitaire.Instance.AddRightFan();

                    break;
            }


            //DO PLAYER ATTACK
            //DO DEFENSE ?????
            _sucessTextPlayer1.color = Color.black;
            _sucessTextPlayer1.text = "Good";

        }
        else
        {
            switch (GameManager.Instance.CurrentState)
            {
                case GameStates.Player1Defense:
                    _player1DefenseSuccess = false;
                    print("P1 Failed Defnse");

                    if (_player2AttackSuccess)
                    {
                        print("P1 Failed Defend P2");
                        if (FouleUnitaire.Instance.FouleLeft == 0) Debug.LogError("GAMEOVER");

                        print(FouleUnitaire.Instance.FouleRight + "   " + FouleUnitaire.Instance.FouleRight / 2);
                        for (int i = 0; i < FouleUnitaire.Instance.FouleRight / 2; i++)
                        {
                            FouleUnitaire.Instance.RemoveLeftFan();
                        }
                    }
                    break;

                case GameStates.Player1Attack:
                    _player1AttackSuccess = false;
                    print("P1 Failed Attack");


                    break;

                case GameStates.Player2Defense:
                    _player2DefenseSuccess = false;
                    print("P2 Failed Defnse");


                    if (_player1AttackSuccess)
                    {
                        print("P2 Failed Defnse against P1");

                        if (FouleUnitaire.Instance.FouleRight == 0) Debug.LogError("GAMEOVER");

                        print(FouleUnitaire.Instance.FouleLeft + "   " +FouleUnitaire.Instance.FouleLeft / 2);
                        for(int i = 0; i < FouleUnitaire.Instance.FouleLeft/2; i++)
                        {
                            FouleUnitaire.Instance.RemoveRightFan();
                        }
                    }

                    //CODE (4LAN) FOR SUPERS ATTACKS
                    break;

                case GameStates.Player2Attack:
                    _player2AttackSuccess = false;
                    print("P2 failed attack");


                    break;
            }

            _sucessTextPlayer1.color = Color.black;
            _sucessTextPlayer1.text = "FAIL";
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
        _currentKeyCodes = _currentHandsSequence.handSigns[0].KeyCodesFingers;


        TextMeshProUGUI inputText = _isPlayer1Turn ? _inputTextPlayer1 : _inputTextPlayer2;
        inputText.text = "";

        foreach (KeyCode key in _currentKeyCodes)
            inputText.text += key + " ";

    }

    private void Start()
    {
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
        if (_sucessTextPlayer1.color.a > 0) _sucessTextPlayer1.color = new Color(0, 0, 0, _sucessTextPlayer1.color.a - 0.01f);
        if (!_isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(GameManager.GameStates.Player1Attack);
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
