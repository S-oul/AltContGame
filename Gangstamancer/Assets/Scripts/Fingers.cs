using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fingers", menuName = "Scriptables/Fingers")]
public class Fingers : ScriptableObject
{
    public FingerType FingersTypes;

    [Serializable, Flags]
    public enum FingerType
    {
        None = 0,
        Thumb = 1 << 0,
        Index = 1 << 1,
        Middle = 1 << 2,
        Ring = 1 << 3,
        Pinky = 1 << 4
    }

    public bool GetFingerFromInput(List<KeyCode> inputs, Fingers fingerP)
    {
        FingerType finger = FingerType.None;

        // WARNING: It's only placeholder code. We need to change it to the real implementation.
        if (inputs.Contains(KeyCode.A))
            finger |= FingerType.Thumb;
        if (inputs.Contains(KeyCode.Z))
            finger |= FingerType.Index;
        if (inputs.Contains(KeyCode.E))
            finger |= FingerType.Middle;
        if (inputs.Contains(KeyCode.R))
            finger |= FingerType.Ring;
        if (inputs.Contains(KeyCode.T))
            finger |= FingerType.Pinky;

        return finger == fingerP.FingersTypes ? true: false;
    }
}
