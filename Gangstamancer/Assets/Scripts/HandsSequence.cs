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
    [Expandable]
    public Fingers handSign;
    public Height height;
    public HandType hand;

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

    public static bool operator ==(HandSign a, HandSign b) => (a.handSign == b.handSign && a.height == b.height && a.hand == b.hand);
    public static bool operator !=(HandSign a, HandSign b) => (a.handSign != b.handSign || a.height != b.height || a.hand != b.hand);
    public override bool Equals(object obj) => obj is HandSign sequence && sequence == this;
    public override int GetHashCode() => base.GetHashCode();
}