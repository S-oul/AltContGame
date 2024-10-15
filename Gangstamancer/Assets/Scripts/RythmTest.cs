using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RhythmTest : MonoBehaviour
{

    public static event System.Action OnBeat;

    [SerializeField] TextMeshProUGUI _text;

    [SerializeField] AudioSource musicSource;
    [SerializeField] float bpm = 120f;

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
        new KeyCode[] { KeyCode.U, KeyCode.Y },
        new KeyCode[] { KeyCode.L, KeyCode.M }
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
        //beatInterval = 60f / bpm;
        if (_text.color.a > 0) _text.color = _text.color - new Color(0, 0, 0, _alphaDepletion * Time.fixedDeltaTime);

        if (!isPlaying && Input.GetKey(KeyCode.Space))
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
        float beatDifference = Mathf.Abs(musicSource.time - nextBeatTime);
        if (isPlaying)
        {
            if (CheckInput())
            {
                CheckInputTiming(musicSource.time);
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
        float beatDifference = inputTime - nextBeatTime + beatInterval;
        if(beatDifference > 1-_timingWindow*2) beatDifference -= 1;
        
        if (beatDifference > 0)
        {
            if (beatDifference <= _timingWindow)
            {
                _text.text = "Perfect !";
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
            }
            else if (beatDifference <= _timingWindow * 2)
            {
                _text.text = "Good";
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .5f);
            }
            else
            {
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .1f);
                _text.text = "Miss...";
            }
        }
        else if (beatDifference == 0)
        {
            _text.color = new Color(1, 0, 0, 1f);
            _text.text = "WAHTS";
        }
        else
        {
            if (beatDifference >= - _timingWindow)
            {
                _text.text = "Perfect !";
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
            }
            else if (beatDifference >= - _timingWindow * 2)
            {
                _text.text = "Good";
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .5f);
            }
            else
            {
                    _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, .1f);
                    _text.text = "Miss...";
            }
        }


        Debug.Log(beatDifference + " " + _text.text);

    }
}
