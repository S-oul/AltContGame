using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public UnityEvent Player1Turn;
    public UnityEvent Player2Turn;

    #region GameStates
    public enum GameStates
    {
        MainMenu,
        GameStart,
        Player1Turn,
        Player2Turn,
        GameOver
    }
    private GameStates _currentState;
    public GameStates CurrentState => _currentState;

    public PlayerHandsInput Player1Inputs { get => _player1Inputs; set => _player1Inputs = value; }
    public PlayerHandsInput Player2Inputs { get => _player2Inputs; set => _player2Inputs = value; }
    public List<Fingers> AllFingers { get => _allFingers; set => _allFingers = value; }

    #endregion


    [Expandable]
    [SerializeField] private HandsSequence _handsSequence;
    private int _currentSequenceIndex = 0;
    private float _timeBetweenSequences = 3f;
    private float _timeForPlayerToInput = 2f;
    private HandsSign _currentHandSign;
    private List<KeyCode> _player1InputKeys;

    [Expandable]
    [SerializeField] private PlayerHandsInput _player1Inputs;
    [Expandable]
    [SerializeField] private PlayerHandsInput _player2Inputs;
    [SerializeField] private List<KeyCode> _allInputKeysDown;

    //Random
    [SerializeField] private List<Fingers> _allFingers;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void GenerateHandsSequence()
    {
        _handsSequence.handSigns.Clear();
        for (int i = 0; i < 10; i++)
        {
            HandsSign handSign = new HandsSign();
            handSign.handSignLeft = AllFingers[UnityEngine.Random.Range(0, AllFingers.Count)];
            handSign.handSignRight = AllFingers[UnityEngine.Random.Range(0, AllFingers.Count)];
            handSign.height = (HandsSign.Height)UnityEngine.Random.Range(0, 3);
            // modulo to get player 1 then 2 every time
            handSign.player = (HandsSign.PlayerNumber)(i % 2);
            handSign.inputsPlayer = handSign.player == HandsSign.PlayerNumber.Player1 ? Player1Inputs : Player2Inputs;

            _handsSequence.handSigns.Add(handSign);
        }
    }

    public void ChangeState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainMenu:
                break;
            case GameStates.GameStart:
                break;
            case GameStates.Player1Turn:
                Player1Turn.Invoke();
                break;
            case GameStates.Player2Turn:
                Player2Turn.Invoke();   
                break;
            case GameStates.GameOver:
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        ChangeState(GameStates.GameStart);
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        while (_currentSequenceIndex < _handsSequence.SequenceCount)
        {
            yield return new WaitForSeconds(_timeBetweenSequences);
            yield return StartCoroutine(PlayHandSign());
            _currentSequenceIndex++;
        }
    }

    private IEnumerator PlayHandSign()
    {
        float timer = 0;
        while (timer < _timeForPlayerToInput)
        {
            timer += Time.deltaTime;
            _currentHandSign = _handsSequence.GetHandSign(_currentSequenceIndex);
            if (false)
            {
                // wait for player input
                //yield return new WaitUntil(() => handSign == _handsSequence.GetHandSign(_currentSequenceIndex));
            }
            yield break;
        }
    }

    private void Update()
    {
        
    }

    private void CheckInputFromList(KeyCode key)
    {
        if(_allInputKeysDown.Contains(key))
            _allInputKeysDown.Remove(key);
        else
            _allInputKeysDown.Add(key);

        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (_handsSequence.GetHandSign(_currentSequenceIndex).IsHandCorrect(_allInputKeysDown, HandsSign.HandType.Left))
        {
            // correct input
            Debug.Log("Correct input");
        }
        else
        {
            // wrong input
        }
    }

    private void CheckPlayer1()
    {

    }

    private void GetPlayerInputKeys()
    {
        // get player input keys
    }

}
