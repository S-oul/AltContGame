using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class DebugHand : MonoBehaviour
{
    public HandsSign handSign;
    public TextMeshProUGUI text;
    public Image handLeft;
    public Image handRight;

    [Button]
    public void GetKeyCodesFromHandSign()
    {
        handSign.CreateKeyCodesFromFingers();
        handLeft.sprite = handSign.handSignLeft.SpriteLeft;
        handRight.sprite = handSign.handSignRight.SpriteRight;
        text.text = "";
        foreach (var item in handSign.KeyCodesFingers)
        {
            text.text += item + " ";
        }
    }
}
