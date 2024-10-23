using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using static Fingers;
using System.Linq;
using static HandsSign;
using System.Text;

[CreateAssetMenu(fileName = "HandsSequence", menuName = "Scriptables/HandsSequence")]
public class HandsSequence : ScriptableObject
{
    public List<HandsSign> handSigns = new List<HandsSign>();

    public HandsSign GetHandSign(int index) => handSigns[index];
    public int SequenceCount => handSigns.Count;

    public HandsSign CreateRandomHandSign(PlayerNumber playerNumber, bool mirrorHands = false)
    {
        GameManager gameManager = GameManager.Instance;
        HandsSign handSign = new HandsSign();
        handSign.handSignLeft = gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.handSignRight = mirrorHands ? handSign.handSignLeft : gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.height = (Height)UnityEngine.Random.Range(1, 3);
        handSign.player = playerNumber;
        handSign.inputsPlayer = handSign.player == PlayerNumber.Player1 ? gameManager.Player1Inputs : gameManager.Player2Inputs;
        handSign.KeyCodesFingers = handSign.CreateKeyCodesFromFingers();
        handSigns.Add(handSign);
        return handSign;
    }
    
    public static HandsSign CreateStaticRandomHandSign(PlayerNumber playerNumber, bool mirrorHands = false)
    {
        GameManager gameManager = GameManager.Instance;
        HandsSign handSign = new HandsSign();
        handSign.handSignLeft = gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.handSignRight = gameManager.AllFingers[UnityEngine.Random.Range(0, gameManager.AllFingers.Count)];
        handSign.height = (Height)UnityEngine.Random.Range(1, 3);
        handSign.player = playerNumber;
        handSign.inputsPlayer = handSign.player == PlayerNumber.Player1 ? gameManager.Player1Inputs : gameManager.Player2Inputs;
        handSign.KeyCodesFingers = handSign.CreateKeyCodesFromFingers();
        return handSign;
    }

    public List<HandsSign> CreateRandomHandSign(PlayerNumber playerNumber, int numberToCreate, bool mirrorHands = false)
    {
        List<HandsSign> handSignsTemp = new List<HandsSign>();
        for (int i = 0; i < numberToCreate; i++)
        {
            var currentHandSign = CreateRandomHandSign(playerNumber, mirrorHands);
            handSignsTemp.Add(currentHandSign);
        }

        return handSignsTemp;

    }
}
public enum PlayerNumber
{
    Player1 = 0,
    Player2
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
    public  StringBuilder inputString;

    public enum Height
    {
        Low = 0,
        Medium,
        High
    }


    public List<KeyCode> CreateKeyCodesFromFingers()
    {
        inputString = new StringBuilder("00000000");
        KeyCodesFingers = new List<KeyCode>();
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Index))
        {
            KeyCodesFingers.Add(inputsPlayer.LeftHandInputs[3]);
            inputString[0] = '1';
        }
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Middle))
        {
            inputString[1] = '1';
            KeyCodesFingers.Add(inputsPlayer.LeftHandInputs[2]);
        }
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Ring))
        {
            inputString[2] = '1';
            KeyCodesFingers.Add(inputsPlayer.LeftHandInputs[1]);
        }
        if (handSignLeft.FingersTypes.HasFlag(FingerType.Pinky))
        {
            inputString[3] = '1';
            KeyCodesFingers.Add(inputsPlayer.LeftHandInputs[0]);
        }

        if (handSignRight.FingersTypes.HasFlag(FingerType.Index))
        {
            inputString[4] = '1';
            KeyCodesFingers.Add(inputsPlayer.RightHandInputs[0]);
        }
        if (handSignRight.FingersTypes.HasFlag(FingerType.Middle))
        {
            inputString[5] = '1';
            KeyCodesFingers.Add(inputsPlayer.RightHandInputs[1]);
        }
        if (handSignRight.FingersTypes.HasFlag(FingerType.Ring)) 
        { 
            KeyCodesFingers.Add(inputsPlayer.RightHandInputs[2]);
            inputString[6] = '1';
        }
        if (handSignRight.FingersTypes.HasFlag(FingerType.Pinky))
        {
            inputString[7] = '1';
            KeyCodesFingers.Add(inputsPlayer.RightHandInputs[3]);
        }


        return KeyCodesFingers;
        
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