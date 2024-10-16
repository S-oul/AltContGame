using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMove : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnEnable()
    {
        RythmTimeLine.OnBeat += DoOnBeat;
    }

    private void OnDisable()
    {
        RhythmTest.OnBeat -= DoOnBeat;
    }

    private void DoOnBeat()
    {
        animator.SetTrigger("OnBeat");
    }
}
