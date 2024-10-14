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

    public FingerType GetFingerFromInput(List<KeyCode> inputs)
    {
        FingerType finger = FingerType.None;

        if(inputs.Contains(KeyCode.A))
            finger |= FingerType.Thumb;
        if (inputs.Contains(KeyCode.Z))
            finger |= FingerType.Index;
        if (inputs.Contains(KeyCode.E))
            finger |= FingerType.Middle;
        if (inputs.Contains(KeyCode.R))
            finger |= FingerType.Ring;
        if (inputs.Contains(KeyCode.T))
            finger |= FingerType.Pinky;


        return finger;
    }
}
