using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bounce : MonoBehaviour
{
    [SerializeField] Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        RythmTimeLine.OnBeat += DoOnBeat;
    }

    // Update is called once per frame
    void DoOnBeat ()
    {
        if(animator != null)
        animator.SetTrigger("OnBeat");
    }
}
