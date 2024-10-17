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

    public static event System.Action Player1Turn;
    public static event System.Action Player2Turn;

    #region GameStates
    public enum GameStates
    {
        MainMenu,
        GameStart,
        Player1Defense,
        Player1Attack,
        Player2Defense,
        Player2Attack,
        GameOver
    }
    [SerializeField] private GameStates _currentState;
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

    public int PlayerTurn()
    {
        switch (_currentState)
        {
            case GameStates.Player1Defense:
            case GameStates.Player1Attack:
                return 1;
            case GameStates.Player2Defense:
            case GameStates.Player2Attack:
                return 2;
            default:
                return 0;
        }
    }

    public void ChangeNextStatePlayer()
    {
        switch (_currentState)
        {
            case GameStates.Player1Defense:
                ChangeState(GameStates.Player1Attack);
                break;
            case GameStates.Player1Attack:
                ChangeState(GameStates.Player2Defense);
                break;
            case GameStates.Player2Defense:
                ChangeState(GameStates.Player2Attack);
                break;
            case GameStates.Player2Attack:
                ChangeState(GameStates.Player1Defense);
                break;
            default:
                break;
        }
    }

    public void CurrentPlayerFailedAttack()
    {
        switch (_currentState)
        {
            case GameStates.Player1Attack:
                ChangeState(GameStates.Player2Defense);
                break;
            case GameStates.Player2Attack:
                ChangeState(GameStates.Player1Defense);
                break;
            default:
                break;
        }
    }

    public void ChangeState(GameStates newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case GameStates.MainMenu:
                break;
            case GameStates.GameStart:
                break;
            case GameStates.Player1Defense:
                break;
            case GameStates.Player1Attack:
                break;
            case GameStates.Player2Defense:
                break;
            case GameStates.Player2Attack:
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
