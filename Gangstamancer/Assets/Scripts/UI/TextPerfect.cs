using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPerfect : MonoBehaviour
{
    [SerializeField] Animator animator;
    void OnEnable()
    {
        RhythmTest.OnBeat += OnBeat;
    }

    // Update is called once per frame
    void OnBeat ()
    {
        animator.SetTrigger("OnBeat");
    }
}
