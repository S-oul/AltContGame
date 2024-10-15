using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using static Fingers;

[CreateAssetMenu(fileName = "HandsSequence", menuName = "Scriptables/HandsSequence")]
public class HandsSequence : ScriptableObject
{
    public List<HandsSign> handSigns = new List<HandsSign>();

    public HandsSign GetHandSign(int index) => handSigns[index];
    public int SequenceCount => handSigns.Count;
}

[Serializable]
public struct HandsSign
{
    [Expandable]
    public Fingers handSignLeft;

    [Expandable]
    public Fingers handSignRight;

    [HorizontalLine(color: EColor.Black)]
    public Height height;
    public PlayerNumber player;

    public enum Height
    {
        Low = 0,
        Medium,
        High
    }

    public enum PlayerNumber
    {
        Player1 = 0,
        Player2
    }



    public bool IsLeftHandCorrect(List<KeyCode> inputs)
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

        return finger == handSignLeft.FingersTypes ? true : false;
    }

    public bool IsRightHandCorrect(List<KeyCode> inputs)
    {
        FingerType finger = FingerType.None;

        // WARNING: It's only placeholder code. We need to change it to the real implementation.
        if (inputs.Contains(KeyCode.Y))
            finger |= FingerType.Thumb;
        if (inputs.Contains(KeyCode.U))
            finger |= FingerType.Index;
        if (inputs.Contains(KeyCode.I))
            finger |= FingerType.Middle;
        if (inputs.Contains(KeyCode.O))
            finger |= FingerType.Ring;
        if (inputs.Contains(KeyCode.P))
            finger |= FingerType.Pinky;

        return finger == handSignRight.FingersTypes ? true : false;
    }



    #region Operators

    #region Operator equals
    public static bool operator ==(HandsSign a, HandsSign b) => IsEqualsTwoHands(a, b) && IsEqualsGeneric(a, b);

    private static bool IsEqualsGeneric(HandsSign a, HandsSign b)
    {
        return a.height == b.height && a.player == b.player;
    }

    private static bool IsEqualsTwoHands(HandsSign a, HandsSign b)
    {
        return a.handSignLeft == b.handSignLeft && a.handSignRight == b.handSignRight;
    }
    #endregion

    #region Operator different
    public static bool operator !=(HandsSign a, HandsSign b) => IsDifferentTwoHands(a, b) && IsDifferentGeneric(a, b);
    private static bool IsDifferentGeneric(HandsSign a, HandsSign b)
    {
        return a.height != b.height || a.player != b.player;
    }

    private static bool IsDifferentTwoHands(HandsSign a, HandsSign b)
    {
        return a.handSignLeft != b.handSignLeft || a.handSignRight != b.handSignRight;
    }
    #endregion

    public override bool Equals(object obj) => obj is HandsSign handSign && handSign == this;
    public override int GetHashCode() => base.GetHashCode();
    #endregion
}