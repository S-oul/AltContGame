using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static event System.Action OnBeat;

    [SerializeField] TextMeshProUGUI _text;

    [SerializeField] AudioSource musicSource;
    [SerializeField] float bpm = 120f;
    [SerializeField] SquareMove currentSquare;

    float beatInterval;
    float nextBeatTime;
    bool isPlaying = false;
    float lastBeatTime;

    [SerializeField] float _timingWindow = 0.15f;
    [SerializeField] float _alphaDepletion = .5f;
    [SerializeField] float _DelayToPlaySong = .005f;


    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Z, KeyCode.E },
      /*  new KeyCode[] { KeyCode.U, KeyCode.Y },
        new KeyCode[] { KeyCode.L, KeyCode.M },*/

    };
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_DelayToPlaySong);
        musicSource.Play();
    }
    void Start()
    {
        beatInterval = 60f / bpm;
        nextBeatTime = 0f;
    }

    void FixedUpdate()
    {
        beatInterval = 60f / bpm;
        if (_text.color.a > 0) _text.color = _text.color - new Color(0, 0, 0, _alphaDepletion * Time.deltaTime);

        if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            StartMusic();
            isPlaying = true;
        }

        if (isPlaying)
        {
            float songPosition = musicSource.time;

            if (songPosition >= nextBeatTime)
            {
                Debug.Log("Beat!");
                OnBeat?.Invoke();
                nextBeatTime += beatInterval;
            }

            
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (CheckInput())
            {
                float songPosition = musicSource.time;
                float inputTime = songPosition;
                CheckInputTiming(inputTime);
            }
        }
    }


    bool CheckInput()
    {
        int reussite = 0;
        foreach (KeyCode key in _keyCodes[0])
        {
            if (Input.GetKeyDown(key))
            {
                reussite++;
            }
        }
        return reussite == _keyCodes[0].Length - 1;
    }
    void StartMusic()
    {
        StartCoroutine(Delay());
        nextBeatTime = musicSource.time + beatInterval;
        isPlaying = true;
    }

    void CheckInputTiming(float inputTime)
    {
        float beatDifference = Mathf.Abs(inputTime - nextBeatTime + beatInterval);
        if (beatDifference <= _timingWindow)
        {
            _text.text = "Perfect !";
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
            Debug.Log("Perfect hit!");
        }
        else if (beatDifference <= _timingWindow * 2)
        {
            _text.text = "Good";
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .5f);
            Debug.Log("Good hit");
        }
        else
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .1f);
            _text.text = "Miss...";
            Debug.Log("Miss");
        }
    }
}
