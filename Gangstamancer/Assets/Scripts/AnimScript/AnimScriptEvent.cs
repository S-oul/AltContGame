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
        Debug.Log("Anim Started");
        OnAnimStartedEvent.Invoke();
    }
    public void OnAnimFinished()
    {
        Debug.Log("Anim Finished");
        OnAnimFinishedEvent.Invoke();
    }
}
