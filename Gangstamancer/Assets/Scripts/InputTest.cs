    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputTest : MonoBehaviour
{
    [SerializeField] int BPM = 120;
    [SerializeField] KeyCode[] key = new KeyCode[3];

    [SerializeField] float TimeSinceStart = 0;

    private void Start()
    {
        StartCoroutine(DoBeat());
    }
    private void Update()
    {
        if (CheckInput());
    }

    bool CheckInput()
    {
        int reussite = 0;
        foreach(KeyCode key in key)
        {
            if(Input.GetKey(key))
            {
                reussite++;
            }
        }
        return reussite == key.Length - 1;
    }

    void FixedUpdate()
    {
    }

    IEnumerator DoBeat()
    {
        print("yaaaa");
        yield return new WaitForSeconds(60f/(float)BPM);
        StartCoroutine(DoBeat());
    }
}
