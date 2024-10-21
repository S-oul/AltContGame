using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWhoStart : MonoBehaviour
{
    [SerializeField] RythmTimeLine timeLine;
    [SerializeField] FouleUnitaire fouleeee;



    [Header("Players Inputs")]
    [SerializeField] PlayerHandsInput _player1Inputs;
    [SerializeField] PlayerHandsInput _player2Inputs;

    [Header("Image")]
    public Image p1Left;
    public Image p1Right;

    public Image p2Left;
    public Image p2Right;

    public TextMeshProUGUI P1TEXT;
    public TextMeshProUGUI P2TEXT;

    public Image bg;
    HandsSign handSign1;
    HandsSign handSign2;

    List<KeyCode> fingerType1;
    List<KeyCode> fingerType2;

    int phasep1 = 3;
    int phasep2 = 3;
    bool hasAWinner = false;
    void Start()
    {
        ChooseP1RandomHands();
        ChooseP2RandomHands();

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAWinner) if (CheckInputP1() == fingerType1.Count - 1 || Input.GetKeyDown(KeyCode.X))
            {
                phasep1--;
                if (phasep1 == 0)
                {
                    hasAWinner = true;
                    timeLine.firstState = GameManager.GameStates.Player1Attack;
                    //YELLOW START
                    StartCoroutine(ONWin());
                }
                P1TEXT.text = phasep1.ToString();
                //PLAY AUDIO CUE
                if(!hasAWinner)
                ChooseP1RandomHands();
            }
        if (!hasAWinner) if (CheckInputP2() == fingerType2.Count - 1 || Input.GetKeyDown(KeyCode.C))
            {
                //APFJAUIZGEBUKDFGZIUDKFGBI
                phasep2--;
                if (phasep2 == 0)
                {
                    hasAWinner = true;
                    timeLine.firstState = GameManager.GameStates.Player1Attack;
                    StartCoroutine(ONWin());

                    //VIOLET START START
                }
                P2TEXT.text = phasep2.ToString();
                if(!hasAWinner)
                    ChooseP2RandomHands();
            }

    }

    IEnumerator ONWin()
    {
        while (bg.color.a > 0)
        {
            bg.color -= new Color(0, 0, 0, 1 * Time.deltaTime / 3);
            p1Left.color = new Color(1, 1, 1, bg.color.a);
            p1Right.color = new Color(1, 1, 1, bg.color.a);
            p2Left.color = new Color(1, 1, 1, bg.color.a);
            p2Right.color = new Color(1, 1, 1, bg.color.a);
            yield return null;
        }
        print("test");
        bg.rectTransform.parent.GetComponent<Canvas>().enabled = false;
        fouleeee.OnStart();

        int test = 50;
        while (test > 0)
        {
            test--;
        }

        timeLine.OnStart();
    }
    void ChooseP1RandomHands()
    {
        handSign1 = HandsSequence.CreateRandomHandSign(PlayerNumber.Player1);
        p1Left.sprite = handSign1.handSignLeft.SpriteToDoLeft;
        p1Right.sprite = handSign1.handSignLeft.SpriteToDoRight;


        p1Right.rectTransform.localScale = new Vector3(-3, 3, 3);

        fingerType1 = handSign1.CreateKeyCodesFromFingers();

    }
    void ChooseP2RandomHands()
    {
        handSign2 = HandsSequence.CreateRandomHandSign(PlayerNumber.Player2);
        p2Left.sprite = handSign2.handSignLeft.SpriteToDoLeft;
        p2Right.sprite = handSign2.handSignLeft.SpriteToDoRight;

        p2Right.rectTransform.localScale = new Vector3(-3, 3, 3);

        fingerType2 = handSign2.CreateKeyCodesFromFingers();
    }

    int CheckInputP1()
    {
        int isSuccess = 0;

        foreach (KeyCode key in _player1Inputs.playerInputs)
        {
            if (Input.GetKey(key))
            {
                if (fingerType1.Contains(key))
                {
                    print("YEEEEEEEEEEEEEEES");
                    isSuccess++;
                }
                else isSuccess--;
            }
        }
        print(isSuccess + "  " + _player1Inputs.playerInputs.Count);
        return isSuccess;
    }
    int CheckInputP2()
    {
        int isSuccess = 0;

        List<KeyCode> toUse;

        foreach (KeyCode key in _player2Inputs.playerInputs)
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
}
