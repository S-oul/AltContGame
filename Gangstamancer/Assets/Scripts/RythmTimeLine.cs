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
    [SerializeField] bool _generateMirrorHands;
    public bool GenerateMirrorHands { get => _generateMirrorHands; set => _generateMirrorHands = value; }   

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

    [Header("Ecran Principal Validation")]
    [SerializeField] SpriteRenderer EcranPrincipalValidation;
    [SerializeField] Sprite EcranSuccess;
    [SerializeField] Sprite EcranFail;

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
        EcranPrincipalValidation.sprite = fullSucses ? EcranSuccess : EcranFail;

        if (fullSucses)
        {
            _handsSpriteOnPlayers.PutGoodHandsOnPlayer(_isPlayer1Turn ? _player1HandSequence.handSigns[0] : _player2HandSequence.handSigns[0]);
        }

        switch (GameManager.Instance.CurrentState)
        {
            case GameStates.Player1Defense:

                EcranPrincipalValidation.GetComponent<Animator>().SetTrigger("Validation");
                BGEcranPrincipal.sprite = BGYellowAttack;
                _player1DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P1 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _feebbackFouleYellow.SetTrigger("boooo");
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
                    _feebbackFouleYellow.SetTrigger("Yay");
                    if (_player1SuperAttack) _player1Sayajin.SetBool("Sayajin", true);
                    else _player1Attack.SetTrigger("Defense");
                }

                break;

            case GameStates.Player1Attack:

                EcranPrincipalValidation.GetComponent<Animator>().SetTrigger("Validation");
                BGEcranPrincipal.sprite = BGPurpleDefense;

                _player1AttackSuccess = fullSucses;
                _player1SuperAttack = false;
                _player1Sayajin.SetBool("Sayajin", false);

                _sucessTextPlayer1.text = "P1 Attack is  " + fullSucses;

                if (!fullSucses)
                {
                    _feebbackFouleYellow.SetTrigger("boooo");
                    _player2SuperAttack = true;
                }else
                {
                    _feebbackFouleYellow.SetTrigger("Yay");

                    _player1Attack.SetTrigger(ChooseAnim());
                    if (FouleUnitaire.Instance.FouleLeft != 0) FouleUnitaire.Instance.AddLeftFan();
                }

                break;

            case GameStates.Player2Defense:

                EcranPrincipalValidation.GetComponent<Animator>().SetTrigger("Validation");
                BGEcranPrincipal.sprite = BGPurpleAttack;


                _player2DefenseSuccess = fullSucses;
                _sucessTextPlayer1.text = "P2 Defense is  " + fullSucses;

                if (!fullSucses) // FAILED
                {
                    _feebbackFouleViolette.SetTrigger("BooViolette");
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
                    _feebbackFouleViolette.SetTrigger("YayViolette");

                    if (_player2SuperAttack) _player2Sayajin.SetBool("Sayajin", true);
                    else _player2Attack.SetTrigger("Defense");
                }

                break;

            case GameStates.Player2Attack:

                EcranPrincipalValidation.GetComponent<Animator>().SetTrigger("Validation");
                BGEcranPrincipal.sprite = BGYellowDefense;

                _player2AttackSuccess = fullSucses;
                _sucessTextPlayer1.text = "P2 Attack is  " + fullSucses;
                _player2Sayajin.SetBool("Sayajin", false);
                _player2SuperAttack = false;
                if (!fullSucses)
                {
                    _feebbackFouleViolette.SetTrigger("BooViolette");
                    _player1SuperAttack = true;
                }else
                {
                    _feebbackFouleViolette.SetTrigger("YayViolette");

                    _player2Attack.SetTrigger(ChooseAnim());

                    if (FouleUnitaire.Instance.FouleRight != 0) FouleUnitaire.Instance.AddRightFan();
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

        var handSign = _currentHandsSequence.CreateRandomHandSign(_isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, _generateMirrorHands);
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

        
        var handSequenceStarting = _isPlayer1Turn ? _player1HandSequence : _player2HandSequence;
        var handSequenceSecond = _isPlayer1Turn ? _player2HandSequence : _player1HandSequence;

        handSequenceStarting.handSigns.Clear();
        handSequenceSecond.handSigns.Clear();

        Debug.Log($"_isPlayer1Turn: {_isPlayer1Turn} \nCurrentState: {GameManager.Instance.CurrentState}");
        
        BGEcranPrincipal.color = Color.white;
        BGEcranPrincipal.sprite = _isPlayer1Turn ? BGYellowAttack : BGPurpleAttack;

        CreateAndInvokeHandSigns(handSequenceStarting, _isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 1);
        CreateAndInvokeHandSigns(handSequenceSecond, _isPlayer1Turn ? PlayerNumber.Player2 : PlayerNumber.Player1, 2);
        CreateAndInvokeHandSigns(handSequenceStarting, _isPlayer1Turn ? PlayerNumber.Player1 : PlayerNumber.Player2, 2);

        void CreateAndInvokeHandSigns(HandsSequence sequence, PlayerNumber playerNumber, int numberToCreate)
        {
            var tempHandSign = sequence.CreateRandomHandSign(playerNumber, numberToCreate, _generateMirrorHands);
            CreateNewMultipleHandsSigns.Invoke(tempHandSign);
        }
    }



    void Update()
    {
        if (_sucessTextPlayer1.color.a > 0) _sucessTextPlayer1.color = new Color(0, 0, 0, _sucessTextPlayer1.color.a - 0.01f);
        if (!_isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            OnStart();
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
