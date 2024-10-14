using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTest : MonoBehaviour
{

    public static event System.Action OnBeat;

    public AudioSource musicSource;  
    public float bpm = 120f;         
    public KeyCode inputKey = KeyCode.Space;

    private float beatInterval;      
    private float nextBeatTime;      
    private bool isPlaying = false;
    private float timingWindow = 0.15f;

    List<KeyCode[]> _keyCodes = new List<KeyCode[]>
    {
        new KeyCode[] { KeyCode.Z, KeyCode.E },
      /*  new KeyCode[] { KeyCode.U, KeyCode.Y },
        new KeyCode[] { KeyCode.L, KeyCode.M },*/

    };

    void Start()
    {
        beatInterval = 60f / bpm;
        nextBeatTime = 0f;

        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying && Input.GetKeyDown(inputKey))
        {
            //StartMusic();
        }

        if (isPlaying)
        {
            float songPosition = Time.time;

            if (songPosition >= nextBeatTime)
            {
                Debug.Log("Beat!");
                OnBeat?.Invoke();
                nextBeatTime += beatInterval; 
            }

            if (CheckInput())
            {
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
        musicSource.Play();
        nextBeatTime = musicSource.time + beatInterval;
        isPlaying = true;
    }

    void CheckInputTiming(float inputTime)
    {
        float beatDifference = Mathf.Abs(inputTime - nextBeatTime + beatInterval);

        if (beatDifference <= timingWindow)
        {
            Debug.Log("Perfect hit!");
        }
        else if (beatDifference <= timingWindow * 2)
        {
            Debug.Log("Good hit!");
        }
        else
        {
            Debug.Log("Miss!");
        }
    }
}
