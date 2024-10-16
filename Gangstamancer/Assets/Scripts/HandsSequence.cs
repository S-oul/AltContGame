using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using static Fingers;
using System.Linq;

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

    public PlayerHandsInput inputsPlayer;

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

    public enum HandType
    {
        Left = 0,
        Right
    }

    public bool IsHandCorrect(List<KeyCode> inputs, HandType handType)
    {
        return handType == HandType.Left ? IsLeftHandCorrect(inputs) : IsRightHandCorrect(inputs);
    }

    private bool IsLeftHandCorrect(List<KeyCode> inputs)
    {
        return handSignLeft.FingersTypes == GetFingersFromInputs(inputs, inputsPlayer.LeftHandInputs) ? true : false;
    }   

    private bool IsRightHandCorrect(List<KeyCode> inputs)
    {
        return handSignRight.FingersTypes == GetFingersFromInputs(inputs, inputsPlayer.RightHandInputs) ? true : false;
    }

    private FingerType GetFingersFromInputs(List<KeyCode> inputs, List<KeyCode> hand)
    {
        if (hand.Count != 5)
            throw new Exception("The hand must have 5 fingers");

        FingerType finger = FingerType.None;
        if (inputs.Contains(hand[0]))
            finger |= FingerType.Thumb;
        if (inputs.Contains(hand[1]))
            finger |= FingerType.Index;
        if (inputs.Contains(hand[2]))
            finger |= FingerType.Middle;
        if (inputs.Contains(hand[3]))
            finger |= FingerType.Ring;
        if (inputs.Contains(hand[4]))
            finger |= FingerType.Pinky;

        return finger;
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