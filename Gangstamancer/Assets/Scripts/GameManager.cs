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
}
