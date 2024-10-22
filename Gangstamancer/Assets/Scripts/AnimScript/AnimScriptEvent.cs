using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimScriptEvent : MonoBehaviour
{
    public UnityEvent OnAnimStartedEvent;
    public UnityEvent OnAnimFinishedEvent;
    public void OnAnimStarted()
    {
        OnAnimStartedEvent.Invoke();
    }
    public void OnAnimFinished()
    {
        OnAnimFinishedEvent.Invoke();
    }
}
