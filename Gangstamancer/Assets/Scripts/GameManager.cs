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
    #endregion


    [SerializeField] private HandsSequence _handsSequence;
    private int _currentSequenceIndex = 0;
    private float _timeBetweenSequences = 3f;
    private float _timeForPlayerToInput = 2f;
    private HandsSign _currentHandSign;
    private List<KeyCode> _player1InputKeys;

    [SerializeField] private List<KeyCode> _allInputKeysDown;

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

    private void Start()
    {
        _currentHandSign = _handsSequence.GetHandSign(_currentSequenceIndex);
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
        // waiting for prefect player input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A))
            CheckInputFromList(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.Z))
            CheckInputFromList(KeyCode.Z);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.E))
            CheckInputFromList(KeyCode.E);
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyUp(KeyCode.R))
            CheckInputFromList(KeyCode.R);
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyUp(KeyCode.T))
            CheckInputFromList(KeyCode.T);


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
        if (_currentHandSign.IsLeftHandCorrect(_allInputKeysDown))
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
