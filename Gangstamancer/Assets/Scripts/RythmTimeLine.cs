using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using static GameManager;

public class RythmTimeLine : MonoBehaviour
{
    public static event System.Action OnBeat;
    public static event System.Action<int> OnGameOver; // 1 = player 1, 2 = player 2
    public static event System.Action<HandsSign> CreateNewHandSign;
    public static event System.Action<List<HandsSign>> CreateNewMultipleHandsSigns;

    bool _isPlayer1Turn = false;

    [SerializeField] int _nbOfBeatToSkipAtStart;
    [SerializeField] bool _generateMirrorhands;
    public bool GenerateMirrorHands { get => _generateMirrorhands; set => _generateMirrorhands = value; }   

    [SerializeField] PlayableDirector _timeLine;

    [SerializeField] float winOnLastChance = 2;

    [Header("Players Inputs")]
    [SerializeField, Expandable] PlayerHandsInput _player1Inputs;
    [SerializeField, Expandable] PlayerHandsInput _player2Inputs;

    [Header("UI Players")]
    [SerializeField] TextMeshProUGUI _sucessTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer1;
    [SerializeField] TextMeshProUGUI _inputTextPlayer2;
    [SerializeField] HandsSpriteOnPlayers _handsSpriteOnPlayers;

    [Header("Players Hands Sequences")]
    [SerializeField, Expandable] private HandsSequence _player1HandSequence;
    [SerializeField, Expandable] private HandsSequence _player2HandSequence;


    [Header("Ecren Principal PNG")]
    [SerializeField] Sprite BGPurpleAttack;
    [SerializeField] Sprite BGPurpleDefense;
    [SerializeField] Sprite BGYellowAttack;
    [SerializeField] Sprite BGYellowDefense;
    [SerializeField] SpriteRenderer BGEcranPrincipal;

    [SerializeField] Animator _player1Sayajin;
    [SerializeField] Animator _player1Attack;
    [SerializeField] Animator _player1;


    [SerializeField] Animator _player2Sayajin;
    [SerializeField] Animator _player2Attack;
    [SerializeField] Animator _player2;

    [SerializeField] Animator _feebbackFouleYellow;
    [SerializeField] Animator _feebbackFouleViolette;




    private HandsSequence _currentHandsSequence;
    [SerializeField] private List<KeyCode> _currentKeyCodes;

    public GameManager.GameStates firstState;


    bool _isPlaying = false;
    bool _isPaused = false;

    bool _player1AttackSuccess = false;
    bool _player1DefenseSuccess = false;

    bool _player2AttackSuccess = false;
    bool _player2DefenseSuccess = false;

    bool _player1SuperAttack = false;
    bool _player2SuperAttack = false;


    bool CheckInput()
    {
        if (_isPlayer1Turn)
        {
            print(_currentHandsSequence.handSigns[0].inputString.ToString() + " " + PreviewHandsInputs.input1.ToString());
            return _currentHandsSequence.handSigns[0].inputString.ToString() == PreviewHandsInputs.input1.ToString();
        }
        else
        {
            print(_currentHandsSequence.handSigns[0].inputString.ToString() + " " + PreviewHandsInputs.input2.ToString());
            return _currentHandsSequence.handSigns[0].inputString.ToString() == PreviewHandsInputs.input2.ToString();
        }
    }

    public void DoOnBeat()
    {
        OnBeat?.Invoke();

        if (_nbOfBeatToSkipAtStart > 0)
        {
            _nbOfBeatToSkipAtStart--;
            return;
        }

        BGEcranPrincipal.color = Color.white;
        _sucessTextPlayer1.text = GameManager.Instance.CurrentState.ToString() + "  ";
        
        bool fullSucses = CheckInput() || Input.GetKey(KeyCode.Space);

        switch (GameManager.Instance.CurrentState)
        {
            case GameStates.Player1Defense:


                BGEcranPrincipal.sprite = BGYellowAttack;
                _player1DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P1 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _feebbackFouleYellow.SetTrigger(0);
                    _player1Sayajin.SetBool("Sayajin", false);
                    _player1SuperAttack = false;

                    if (_player2AttackSuccess)
                    {
                        _player1.SetTrigger("OnHit");

                        if (FouleUnitaire.Instance.FouleLeft == 0) OnGameOver?.Invoke(2);
                        
                        if (FouleUnitaire.Instance.FouleRight == 0)
                        {
                            for (int i = 0; i < winOnLastChance; i++)
                            {
                                FouleUnitaire.Instance.AddRightFan();
                            }
                        }


                        if (_player2SuperAttack)
                        {
                            for (int i = 0; i < FouleUnitaire.Instance.FouleRight; i++)
                            {
                                FouleUnitaire.Instance.RemoveLeftFan();
                                _player2Sayajin.SetBool("Sayajin", false);
                                _player2SuperAttack = false;
                            }
                        }
                        print(FouleUnitaire.Instance.FouleRight + "   " + FouleUnitaire.Instance.FouleRight / 2);
                        for (int i = 0; i < FouleUnitaire.Instance.FouleRight / 2; i++)
                        {
                            FouleUnitaire.Instance.RemoveLeftFan();
                        }
                    }
                }
                else
                {
                    _feebbackFouleYellow.SetTrigger(1);
                    if (_player1SuperAttack) _player1Sayajin.SetBool("Sayajin", true);
                    else _player1Attack.SetTrigger("Defense");

                    _handsSpriteOnPlayers.PutGoodHandsOnPlayer1(_player1HandSequence.handSigns[0]);
                }

                break;

            case GameStates.Player1Attack:
                BGEcranPrincipal.sprite = BGPurpleDefense;

                _player1AttackSuccess = fullSucses;
                _player1SuperAttack = false;
                _player1Sayajin.SetBool("Sayajin", false);

                _sucessTextPlayer1.text = "P1 Attack is  " + fullSucses;

                if (!fullSucses)
                {
                    _feebbackFouleYellow.SetTrigger(1);
                    _player2SuperAttack = true;
                }else
                {
                    _feebbackFouleYellow.SetTrigger(0);

                    _player1Attack.SetTrigger(ChooseAnim());
                    if (FouleUnitaire.Instance.FouleLeft != 0) FouleUnitaire.Instance.AddLeftFan();
                    _handsSpriteOnPlayers.PutGoodHandsOnPlayer1(_player1HandSequence.handSigns[0]);
                }

                break;

            case GameStates.Player2Defense:

                BGEcranPrincipal.sprite = BGPurpleAttack;


                _player2DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P2 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _player2Sayajin.SetBool("Sayajin", false);
                    _player2SuperAttack = false;

                    if (_player1AttackSuccess)
                    {
                        _player2.SetTrigger("OnHit");
                        if (FouleUnitaire.Instance.FouleRight == 0) OnGameOver?.Invoke(1);

                        if (FouleUnitaire.Instance.FouleLeft == 0)
                        {
                            for (int i = 0; i < winOnLastChance; i++)
                            {
                                FouleUnitaire.Instance.AddLeftFan();
                            }
                        }

                        if (_player1SuperAttack)
                        {
                            for (int i = 0; i < FouleUnitaire.Instance.FouleRight; i++)
                            {
                                FouleUnitaire.Instance.RemoveLeftFan();
                                _player1Sayajin.SetBool("Sayajin", false);
                                _player1SuperAttack = false;
                            }
                        }
                        for (int i = 0; i < FouleUnitaire.Instance.FouleLeft / 2; i++)
                        {
                            FouleUnitaire.Instance.RemoveRightFan();
                        }
                    }
                }
                else
                {
                    if (_player2SuperAttack) _player2Sayajin.SetBool("Sayajin", true);
                    else _player2Attack.SetTrigger("Defense");
                    _handsSpriteOnPlayers.PutGoodHandsOnPlayer2(_player2HandSequence.handSigns[0]);
                }

                break;

            case GameStates.Player2Attack:
                BGEcranPrincipal.sprite = BGYellowDefense;

                _player2AttackSuccess = fullSucses;
                _player2Sayajin.SetBool("Sayajin", false);

                if (!fullSucses)
                {
                    _player1SuperAttack = true;
                }

                _sucessTextPlayer1.text = "P2 Attack is  " + fullSucses;

                if (fullSucses)
                {
                    _player2Attack.SetTrigger(ChooseAnim());

                    if (FouleUnitaire.Instance.FouleRight != 0) FouleUnitaire.Instance.AddRightFan();
                    _handsSpriteOnPlayers.PutGoodHandsOnPlayer2(_player2HandSequence.handSigns[0]);
                }

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
        _currentKeyCodes = _currentHandsSequence.handSigns[0].KeyCodesFingers;
        DiplayCurrentKeyCodes();

        var handSign = _currentHandsSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, _generateMirrorhands);
        CreateNewHandSign.Invoke(handSign);

    }

    private string ChooseAnim()
    {
        int ran = Random.Range(0, 3);
        if (ran == 0) return "FireBall";
        if (ran == 1) return "Portal";
        if (ran == 2) return "Thunder";

        return "FireBall";

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
        tempHandSign = _player1HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 1, _generateMirrorhands);
        CreateNewMultipleHandsSigns.Invoke(tempHandSign);

        tempHandSign.Clear();
        tempHandSign = _player2HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player2 : PlayerNumber.Player1, 2, _generateMirrorhands);
        CreateNewMultipleHandsSigns.Invoke(tempHandSign);

        tempHandSign.Clear();
        tempHandSign = _player1HandSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 2, _generateMirrorhands);
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
