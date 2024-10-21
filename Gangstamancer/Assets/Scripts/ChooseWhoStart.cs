using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWhoStart : MonoBehaviour
{

    [Header("Players Inputs")]
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] PlayerHandsInput _player2Inputs;

    [Header("Image")]
    public Image p1Left;
    public Image p1Right;
    
    public Image p2Left;
    public Image p2Right;

    HandsSign handSign;

    List<KeyCode> fingerType1;
    List<KeyCode> fingerType2;
    void Start()
    {
        ChooseP1RandomHands();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputP1();
        //CheckInputP2();
    }

    void ChooseP1RandomHands()
    {
        int ran = Random.Range(0, GameManager.Instance.AllFingers.Count); 
        //handSign = HandsSequence.CreateRandomHandSign(PlayerNumber.Player1);
        p1Left.sprite = handSign.handSignLeft.SpriteToDoLeft;
        p1Right.sprite = handSign.handSignLeft.SpriteToDoRight;
        
        p2Left.sprite = handSign.handSignRight.SpriteToDoLeft;
        p2Right.sprite = handSign.handSignRight.SpriteToDoRight;

        p1Right.rectTransform.localScale = new Vector3(-3, 3, 3);
        p2Right.rectTransform.localScale = new Vector3(-3, 3, 3);

    }

    int CheckInputP1()
    {
        int isSuccess = 0;

        foreach (KeyCode key in _player1Inputs.playerInputs)
        {
            if (Input.GetKey(key))
            {
                if (fingerType1.Contains(key))
                    isSuccess++;
                else isSuccess--;
            }
        }
        return isSuccess;
    }
/*    int CheckInputP2()
    {
        int isSuccess = 0;

        List<KeyCode> toUse;

        foreach (KeyCode key in toUse)
        {
            if (Input.GetKey(key))
            {
                if (_currentKeyCodes.Contains(key))
                    isSuccess++;
                else isSuccess--;
            }
        }
        return isSuccess;
    }*/
}
