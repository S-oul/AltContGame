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
    public Image image;

    [Button]
    public void GetKeyCodesFromHandSign()
    {
        handSign.CreateKeyCodesFromFingers();
        image.sprite = handSign.handSignLeft.SpriteLeft;
        text.text = "";
        foreach (var item in handSign.KeyCodesFingers)
        {
            text.text += item + " ";
        }
    }
}
