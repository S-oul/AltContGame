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
    public Image bg;

    [SerializeField] Image _eclair;
    [SerializeField] Image _versus;

    public Image JauneCD;
    public Image PurpleCD;


    public List<Sprite> CdYeloow = new List<Sprite>();
    public List<Sprite> CdPurple = new List<Sprite>();


    HandsSign handSign1;
    HandsSign handSign2;

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
        if (!hasAWinner) if (CheckInputP1() == handSign1.KeyCodesFingers.Count || Input.GetKeyDown(KeyCode.X))
            {
                phasep1--;
                if (phasep1 == 0)
                {
                    hasAWinner = true;
                    timeLine.firstState = GameManager.GameStates.Player1Attack;
                    //YELLOW START
                    StartCoroutine(ONWin());
                    JauneCD.color = new Color(0,0,0,0);
                }
                //PLAY AUDIO CUE
                if (!hasAWinner)
                    ChooseP1RandomHands();

                JauneCD.sprite = CdYeloow[phasep1];
            }
        if (!hasAWinner) if (CheckInputP2() == handSign2.KeyCodesFingers.Count || Input.GetKeyDown(KeyCode.C))
            {
                //APFJAUIZGEBUKDFGZIUDKFGBI
                phasep2--;
                if (phasep2 == 0)
                {
                    hasAWinner = true;
                    timeLine.firstState = GameManager.GameStates.Player2Attack;
                    StartCoroutine(ONWin());

                    //VIOLET START START
                    PurpleCD.color = new Color(0, 0, 0, 0);

                }
                if (!hasAWinner)
                    ChooseP2RandomHands();

                PurpleCD.sprite = CdPurple[phasep2];
            }

    }

    IEnumerator ONWin()
    {
        while (bg.color.a > 0)
        {
            bg.color -= new Color(0, 0, 0, Time.deltaTime * 3);
            p1Left.color = new Color(1, 1, 1, bg.color.a);
            p1Right.color = new Color(1, 1, 1, bg.color.a);
            p2Left.color = new Color(1, 1, 1, bg.color.a);
            p2Right.color = new Color(1, 1, 1, bg.color.a);
            _eclair.color = new Color(1, 1, 1, bg.color.a);
            _versus.color = new Color(1, 1, 1, bg.color.a);

            yield return null;
        }
        print(timeLine.firstState + " start");
        bg.rectTransform.parent.GetComponent<Canvas>().enabled = false;
        fouleeee.OnStart();

        int test = 25;
        while (test > 0)
        {
            test--;
        }

        timeLine.OnStart();
    }
    void ChooseP1RandomHands()
    {
        handSign1 = HandsSequence.CreateStaticRandomHandSign(PlayerNumber.Player1, timeLine.GenerateMirrorHands);
        p1Left.sprite = handSign1.handSignLeft.SpriteToDoLeft;
        p1Right.sprite = handSign1.handSignRight.SpriteToDoRight;


        p1Right.rectTransform.localScale = new Vector3(-3, 3, 3);


    }
    void ChooseP2RandomHands()
    {
        handSign2 = HandsSequence.CreateStaticRandomHandSign(PlayerNumber.Player2, timeLine.GenerateMirrorHands);
        p2Left.sprite = handSign2.handSignLeft.SpriteToDoLeft;
        p2Right.sprite = handSign2.handSignRight.SpriteToDoRight;

        p2Right.rectTransform.localScale = new Vector3(-3, 3, 3);

    }

    int CheckInputP1()
    {
        if (PreviewHandsInputs.input1.ToString() == handSign1.inputString.ToString()) return handSign1.KeyCodesFingers.Count;
        else return 0;
    }
    int CheckInputP2()
    {
        if (PreviewHandsInputs.input2.ToString() == handSign2.inputString.ToString()) return handSign2.KeyCodesFingers.Count;
        else return 0;
    }
}
