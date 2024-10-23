using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("GameOver")]
    [SerializeField] private Canvas _canvasGameOver;
    [SerializeField] private Image _WinText;
    [SerializeField] private Image _gameOverPlayer;

    [Header("PlayersSprites")]
    [SerializeField] private Sprite _playerYellowWin;
    [SerializeField] private Sprite _playerPurpleWin;

    [SerializeField] private Sprite _player1Sprite;
    [SerializeField] private Sprite _player2Sprite;

    private void Start()
    {
        _canvasGameOver.enabled = false;
    }

    private void OnEnable()
    {
        RythmTimeLine.OnGameOver += GameOverScreen;
    }

    private void OnDisable()
    {
        RythmTimeLine.OnGameOver -= GameOverScreen;
    }

    private void GameOverScreen(int player)
    {
        _canvasGameOver.enabled = true;
        GameManager.Instance.ChangeState(GameManager.GameStates.GameOver);
        if (player == 1)
        {
            _WinText.sprite = _playerYellowWin;
            _gameOverPlayer.sprite = _player1Sprite;
        }
        else
        {
            _WinText.sprite = _playerPurpleWin;
            _gameOverPlayer.sprite = _player2Sprite;
        }
        StartCoroutine(TOEND());
    }

    IEnumerator TOEND()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(0);
    }

}
