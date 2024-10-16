using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using static Fingers;
using System.Linq;
using static HandsSign;

[CreateAssetMenu(fileName = "HandsSequence", menuName = "Scriptables/HandsSequence")]
public class HandsSequence : ScriptableObject
{
    public List<HandsSign> handSigns = new List<HandsSign>();

    public HandsSign GetHandSign(int index) => handSigns[index];
    public int SequenceCount => handSigns.Count;

    public void CreateRandomHandSign(PlayerNumber playerNumber)
    {
        GameManager gameManager = GameManager.Instance;
        HandsSign handSign = new HandsSign();
        handSign.handSignLeft = gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.handSignRight = gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.height = (HandsSign.Height)UnityEngine.Random.Range(0, 3);
        // modulo to get player 1 then 2 every time
        handSign.player = playerNumber;
        handSign.inputsPlayer = handSign.player == HandsSign.PlayerNumber.Player1 ? gameManager.Player1Inputs : gameManager.Player2Inputs;
        handSign.CreateKeyCodesFromFingers();
        handSigns.Add(handSign);
    }

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
    public List<KeyCode> KeyCodesFingers;

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

    public List<KeyCode> CreateKeyCodesFromFingers()
    {
        List<KeyCode> keyCodes = new List<KeyCode>();
        KeyCodesFingers = new List<KeyCode>();
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Index))
            keyCodes.Add(inputsPlayer.LeftHandInputs[0]);
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Middle))
            keyCodes.Add(inputsPlayer.LeftHandInputs[1]);
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Ring))
            keyCodes.Add(inputsPlayer.LeftHandInputs[2]);
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Pinky))
            keyCodes.Add(inputsPlayer.LeftHandInputs[3]);

        if (handSignRight.FingersTypes.HasFlag(FingerType.Index))
            keyCodes.Add(inputsPlayer.RightHandInputs[0]);
        if (handSignRight.FingersTypes.HasFlag(FingerType.Middle))
            keyCodes.Add(inputsPlayer.RightHandInputs[1]);
        if (handSignRight.FingersTypes.HasFlag(FingerType.Ring))
            keyCodes.Add(inputsPlayer.RightHandInputs[2]);
        if (handSignRight.FingersTypes.HasFlag(FingerType.Pinky))
            keyCodes.Add(inputsPlayer.RightHandInputs[3]);


        return KeyCodesFingers = keyCodes;
        
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