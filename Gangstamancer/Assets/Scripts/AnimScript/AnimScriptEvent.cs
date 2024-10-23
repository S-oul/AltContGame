using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimScriptEvent : MonoBehaviour
{
    public UnityEvent OnAnimStartedEvent;
    public UnityEvent OnAnimFinishedEvent;

    public Animator ProjectilAnimator;
    public Animator ImpactAnimator;


    public void OnAnimStarted()
    {
        OnAnimStartedEvent.Invoke();
    }
    public void OnAnimFinished()
    {
        OnAnimFinishedEvent.Invoke();
    }

    public void LaunchFireBallProj()
    {
        ProjectilAnimator.SetTrigger("Fireball");
    }
    public void Explosion()
    {
        ImpactAnimator.SetTrigger("Explosion");
    }
}
