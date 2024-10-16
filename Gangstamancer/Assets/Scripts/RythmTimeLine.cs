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
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] List<KeyCode> Player2Inputs = new List<KeyCode>();
    [SerializeField] PlayerHandsInput _player2Inputs;
    [SerializeField] PlayableDirector _timeLine;
    [SerializeField] TextMeshProUGUI _sucessText;
    [SerializeField] TextMeshProUGUI _inputText;

    [SerializeField] private HandsSequence handSequence1;
    [SerializeField] private HandsSign _currentHandSign;


    [SerializeField] private SpriteRenderer _EcranPrincipalLeftSprite;
    [SerializeField] private SpriteRenderer _EcranPrincipalRightSprite;

    [SerializeField] private SpriteRenderer _EcranGaucheLeftSprite;
    [SerializeField] private SpriteRenderer _EcranGaucheRightSprite;

    [SerializeField] private SpriteRenderer _EcranDroitLeftSprite;
    [SerializeField] private SpriteRenderer _EcranDroitRightSprite;


    bool _isPlaying = false;
    bool _isPaused = false;

    /// ALL LEFT HAND POSES
    [SerializeField] private List<Fingers> _allFingers;
    List<KeyCode> _keyCodes;
    int CheckInput()
    {
        int isSuccess = 0;
        List<KeyCode> toUse = _isPlayer1Turn ? _player1Inputs.playerInputs : _player2Inputs.playerInputs;

        foreach (KeyCode key in _player1Inputs.playerInputs)
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
        _sucessText.text = "";
        OnBeat?.Invoke();
        int intSuccess = CheckInput();
        bool fullSucses = intSuccess == _keyCodes.Count;
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
        handSequence1.handSigns.RemoveAt(0);
        handSequence1.CreateRandomHandSign(HandsSign.PlayerNumber.Player1);
        _currentHandSign = handSequence1.handSigns[0];
        _keyCodes = handSequence1.handSigns[0].CreateKeyCodesFromFingers();
        _inputText.text = "";

        _EcranPrincipalLeftSprite.sprite = handSequence1.handSigns[0].handSignLeft.SpriteLeft;
        _EcranPrincipalRightSprite.sprite = handSequence1.handSigns[0].handSignRight.SpriteRight;

        _EcranGaucheLeftSprite.sprite = handSequence1.handSigns[1].handSignLeft.SpriteLeft;
        _EcranGaucheRightSprite.sprite = handSequence1.handSigns[1].handSignRight.SpriteRight;


        foreach (KeyCode key in _keyCodes)
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
