using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "HandsSequence", menuName = "Scriptables/HandsSequence")]
public class HandsSequence : ScriptableObject
{
    public List<HandSign> handSigns = new List<HandSign>();
}

[Serializable]
public struct HandSign
{
    public bool isTwoHands;
    [Expandable]
    public Fingers handSign;

    [HideIf("isTwoHands"), AllowNesting]
    public HandType handType;

    [Expandable, ShowIf("isTwoHands")]
    public Fingers handSignTwo;

    [HorizontalLine(color: EColor.Black)]
    public Height height;
    public Player player;

    public enum Height
    {
        Low = 0,
        Medium,
        High
    }

    public enum HandType
    {
        Left = 0,
        Right
    }

    public enum Player
    {
        Player1 = 0,
        Player2
    }


    #region Operators

    #region Operator equals
    public static bool operator ==(HandSign a, HandSign b) => a.isTwoHands ? IsEqualsTwoHands(a, b) && IsEqualsGeneric(a, b) : IsEqualsSingleHand(a, b) && IsEqualsGeneric(a, b);

    private static bool IsEqualsGeneric(HandSign a, HandSign b)
    {
        return a.height == b.height && a.player == b.player;
    }

    private static bool IsEqualsSingleHand(HandSign a, HandSign b)
    {
        return a.handSign == b.handSign && a.handType == b.handType;
    }

    private static bool IsEqualsTwoHands(HandSign a, HandSign b)
    {
        return a.handSign == b.handSign && a.handSignTwo == b.handSignTwo;
    }
    #endregion

    #region Operator different
    public static bool operator !=(HandSign a, HandSign b) => a.isTwoHands ? IsDifferentTwoHands(a, b) && IsDifferentGeneric(a, b) : IsDifferentSingleHand(a, b) && IsDifferentGeneric(a, b);
    private static bool IsDifferentGeneric(HandSign a, HandSign b)
    {
        return a.height != b.height || a.player != b.player;
    }

    private static bool IsDifferentSingleHand(HandSign a, HandSign b)
    {
        return a.handSign != b.handSign || a.handType != b.handType;
    }

    private static bool IsDifferentTwoHands(HandSign a, HandSign b)
    {
        return a.handSign != b.handSign || a.handSignTwo != b.handSignTwo;
    }
    #endregion

    public override bool Equals(object obj) => obj is HandSign sequence && sequence == this;
    public override int GetHashCode() => base.GetHashCode();
    #endregion
}