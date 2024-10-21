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
    public static event System.Action<int> OnGameOver; // 1 = player 1, 2 = player 2
    public static event System.Action<HandsSign> CreateNewHandSign;
    public static event System.Action<List<HandsSign>> CreateNewMultipleHandsSigns;

    bool _isPlayer1Turn = false;

    [SerializeField] PlayableDirector _timeLine;

    [SerializeField] float winOnLastChance = 2;

    [Header("Players Inputs")]
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] PlayerHandsInput _player2Inputs;

    [Header("UI Players")]
    [SerializeField] TextMeshProUGUI _sucessTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer2;

    [Header("Players Hands Sequences")]
    [SerializeField] private HandsSequence _player1HandSequence;
    [SerializeField] private HandsSequence _player2HandSequence;


    [Header("Ecren Principal PNG")]
    [SerializeField] Sprite BGPurpleAttack;
    [SerializeField] Sprite BGPurpleDefense;
    [SerializeField] Sprite BGYellowAttack;
    [SerializeField] Sprite BGYellowDefense;
    [SerializeField] SpriteRenderer BGEcranPrincipal;

    [SerializeField] Animator _player1;
    [SerializeField] Animator _player1Attack;

    [SerializeField] Animator _player2;
    [SerializeField] Animator _player2Attack;


    private HandsSequence _currentHandsSequence;
    private List<KeyCode> _currentKeyCodes;

    public GameManager.GameStates firstState;


    bool _isPlaying = false;
    bool _isPaused = false;

    bool _player1AttackSuccess = false;
    bool _player1DefenseSuccess = false;

    bool _player2AttackSuccess = false;
    bool _player2DefenseSuccess = false;

    bool _player1SuperAttack = false;
    bool _player2SuperAttack = false;




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
        BGEcranPrincipal.color = Color.white;
        _sucessTextPlayer1.text = GameManager.Instance.CurrentState.ToString() + "  ";
        OnBeat?.Invoke();
        int intSuccess = CheckInput();
        bool fullSucses = intSuccess == _currentKeyCodes.Count || Input.GetKey(KeyCode.Space);

        switch (GameManager.Instance.CurrentState)
        {
            case GameStates.Player1Defense:

                BGEcranPrincipal.sprite = BGYellowAttack;
                _player1DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P1 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _player1.SetBool("Sayajin", false);
                    _player1SuperAttack = false;

                    if (_player2AttackSuccess)
                    {
                        if (FouleUnitaire.Instance.FouleRight == 0) 
                        {
                            for (int i = 0; i < winOnLastChance; i++)
                            {
                                FouleUnitaire.Instance.AddRightFan();
                            }
                        }

                        if (FouleUnitaire.Instance.FouleLeft == 0) OnGameOver?.Invoke(2);

                        if (_player2SuperAttack)
                        {
                            for (int i = 0; i < FouleUnitaire.Instance.FouleRight; i++)
                            {
                                FouleUnitaire.Instance.RemoveLeftFan();
                                _player2.SetBool("Sayajin", false);
                                _player2SuperAttack = false;
                            }
                        }
                            print(FouleUnitaire.Instance.FouleRight + "   " + FouleUnitaire.Instance.FouleRight / 2);
                        for (int i = 0; i < FouleUnitaire.Instance.FouleRight / 2; i++)
                        {
                            FouleUnitaire.Instance.RemoveLeftFan();
                        }
                    }
                }else
                {
                }

                break;

            case GameStates.Player1Attack:
                BGEcranPrincipal.sprite = BGPurpleDefense;

                _player1AttackSuccess = fullSucses;
                _sucessTextPlayer1.text = "P1 Attack is  " + fullSucses;

                if (!fullSucses)
                {
                    _player2SuperAttack = true;
                    _player2.SetBool("Sayajin", true);

                    _player1SuperAttack = false;
                    _player1.SetBool("Sayajin", false);
                }

                if (fullSucses && FouleUnitaire.Instance.FouleLeft != 0) FouleUnitaire.Instance.AddLeftFan();


                break;

            case GameStates.Player2Defense:

                BGEcranPrincipal.sprite = BGPurpleAttack;
                

                _player2DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P2 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _player2.SetBool("Sayajin", false);
                    _player2SuperAttack = false;

                    if (_player1AttackSuccess)
                    {
                        if (FouleUnitaire.Instance.FouleLeft == 0)
                        {
                            for (int i = 0; i < winOnLastChance; i++)
                            {
                                FouleUnitaire.Instance.AddLeftFan();
                            }
                        }
                        if (FouleUnitaire.Instance.FouleRight == 0) OnGameOver?.Invoke(1);

                        print(FouleUnitaire.Instance.FouleLeft + "   " + FouleUnitaire.Instance.FouleLeft / 2);
                        
                        if (_player1SuperAttack)
                        {
                            for (int i = 0; i < FouleUnitaire.Instance.FouleRight; i++)
                            {
                                FouleUnitaire.Instance.RemoveLeftFan();
                                _player1.SetBool("Sayajin", false);
                                _player1SuperAttack = false;
                            }
                        }
                        for (int i = 0; i < FouleUnitaire.Instance.FouleLeft / 2; i++)
                        {
                            FouleUnitaire.Instance.RemoveRightFan();
                        }
                    }
                }

                break;

            case GameStates.Player2Attack:
                BGEcranPrincipal.sprite = BGYellowDefense;

                _player2AttackSuccess = fullSucses;

                if (!fullSucses)
                {
                    _player1SuperAttack = true;
                    _player1.SetBool("Sayajin", true);

                    _player2SuperAttack = false;
                    _player2.SetBool("Sayajin", false);


                }

                _sucessTextPlayer1.text = "P2 Attack is  " + fullSucses;

                if (fullSucses && FouleUnitaire.Instance.FouleRight != 0) FouleUnitaire.Instance.AddRightFan();

                break;

        }
        _sucessTextPlayer1.color = Color.black;


        GameManager.Instance.ChangeNextStatePlayer();
        _isPlayer1Turn = GameManager.Instance.PlayerTurn() == 1;
        SelectNewInputs();
    }
    private void SelectNewInputs()
    {
        ClearTextInput();
        _currentHandsSequence.handSigns.RemoveAt(0); // remove the handsign that was just played

        _currentHandsSequence = _isPlayer1Turn ? _player1HandSequence : _player2HandSequence;
        var handSign = _currentHandsSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1: PlayerNumber.Player2);
        CreateNewHandSign.Invoke(handSign);
        _currentKeyCodes = _currentHandsSequence.handSigns[0].KeyCodesFingers;

        DiplayCurrentKeyCodes();

    }

    private void DiplayCurrentKeyCodes()
    {
        _currentKeyCodes = _currentHandsSequence.handSigns[0].KeyCodesFingers;
        TextMeshProUGUI inputText = _isPlayer1Turn ? _inputTextPlayer1 : _inputTextPlayer2;
        inputText.text = "";

        foreach (KeyCode key in _currentKeyCodes)
            inputText.text += key + " ";
    }

    private void CreateNewSequenceAtStart()
    {
        _isPlayer1Turn = GameManager.Instance.PlayerTurn() == 1;
        _currentHandsSequence = _isPlayer1Turn ? _player1HandSequence : _player2HandSequence;

        _player1HandSequence.handSigns.Clear();
        _player2HandSequence.handSigns.Clear();

        List<HandsSign> tempHandSign = new List<HandsSign>();
        tempHandSign = _player1HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 2);
        CreateNewMultipleHandsSigns.Invoke(tempHandSign);

        tempHandSign.Clear();
        tempHandSign = _player2HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player2 : PlayerNumber.Player1, 2);
        CreateNewMultipleHandsSigns.Invoke(tempHandSign);

        tempHandSign.Clear();
        tempHandSign = _player1HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 2);
        CreateNewMultipleHandsSigns.Invoke(tempHandSign);
    }

    

    void Update()
    {
        if (_sucessTextPlayer1.color.a > 0) _sucessTextPlayer1.color = new Color(0, 0, 0, _sucessTextPlayer1.color.a - 0.01f);
        if (!_isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeState(firstState);
            CreateNewSequenceAtStart();
            DiplayCurrentKeyCodes();
            _isPlaying = true;
            _timeLine.Play();
        }
    }
    public void OnStart()
    {
        GameManager.Instance.ChangeState(firstState);
        CreateNewSequenceAtStart();
        DiplayCurrentKeyCodes();
        _isPlaying = true;
        _timeLine.Play();
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

    public void StopBeat()
    {
        _timeLine.Stop();
        _isPlaying = false;
    }
}
