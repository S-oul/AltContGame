using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ecran : MonoBehaviour
{
    [Header("Players Hands On Screen")]
    [SerializeField] GameObject _prefabHandsScreenPlayer1;
    [SerializeField] GameObject _prefabHandsScreenPlayer2;
    [SerializeField] GameObject _handsScreenPlayer1Parent;
    [SerializeField] GameObject _handsScreenPlayer2Parent;


    [Header("Anims")]
    [SerializeField] List<HandAnim> handAnimsPlayer1 = new List<HandAnim>();
    [SerializeField] List<HandAnim> handAnimsPlayer2 = new List<HandAnim>();

    private void OnEnable()
    {
        RythmTimeLine.CreateNewHandSign += CreateHandSignOnScreen;
        RythmTimeLine.CreateNewMultipleHandsSigns += CreateMultipleHandSignsOnScreen;
    }

    private void OnDisable()
    {
        RythmTimeLine.CreateNewHandSign -= CreateHandSignOnScreen;
        RythmTimeLine.CreateNewMultipleHandsSigns -= CreateMultipleHandSignsOnScreen;
    }

    void Start()
    {
        RythmTimeLine.OnBeat += DoOnBeat;
    }

    void DoOnBeat()
    {
        if (handAnimsPlayer1.Count <= 0)
            return;
        UpdateHandAnimationsPlayer1();
        UpdateHandAnimationsPlayer2();
    }

    private bool IsPlayerTurn(HandsSign.PlayerNumber player)
    {
        if (player == HandsSign.PlayerNumber.Player1)
            return GameManager.Instance.CurrentState == GameManager.GameStates.Player1Defense || GameManager.Instance.CurrentState == GameManager.GameStates.Player1Attack;
        else
            return GameManager.Instance.CurrentState == GameManager.GameStates.Player2Defense || GameManager.Instance.CurrentState == GameManager.GameStates.Player2Attack;
    }

    private void UpdateHandAnimationsPlayer1()
    {

        for (int i = 0; i < handAnimsPlayer1.Count; i++)
        {
            if (handAnimsPlayer1[i].transform == null && handAnimsPlayer1[i].pos < 4)
            {
                handAnimsPlayer1[i].pos++;
                continue;
            }else if (handAnimsPlayer1[i].transform == null && handAnimsPlayer1[i].pos == 4)
            {
                HandAnim hand = handAnimsPlayer1[i];
                handAnimsPlayer1.RemoveAt(i);
                i--;
                continue;
            }

            Animator animator = handAnimsPlayer1[i].transform.GetComponent<Animator>();
            if (handAnimsPlayer1[i].pos < 0)
            {
                handAnimsPlayer1[i].pos++;
                continue;
            }
            else if (handAnimsPlayer1[i].pos == 0)
            {
                animator.SetTrigger("Arrive");
                handAnimsPlayer1[i].pos++;
            }
            else if (handAnimsPlayer1[i].pos == 1)
            {
                animator.SetTrigger("Middle");
                handAnimsPlayer1[i].pos++;
            }
            else if (handAnimsPlayer1[i].pos == 2)
            {
                animator.SetTrigger("ToMiddle");
                handAnimsPlayer1[i].pos++;
            }

            else if (handAnimsPlayer1[i].pos == 3)
            {
                animator.SetTrigger("Quit");
                handAnimsPlayer1[i].pos++;
            }
            else if (handAnimsPlayer1[i].pos == 4)
            {
                HandAnim hand = handAnimsPlayer1[i];
                Destroy(hand.transform.gameObject);
                handAnimsPlayer1.RemoveAt(i);
                i--;
                continue;
            }

        }
    }

    private void UpdateHandAnimationsPlayer2()
    {

        for (int i = 0; i < handAnimsPlayer2.Count; i++)
        {
            if (handAnimsPlayer2[i].transform == null && handAnimsPlayer2[i].pos < 4)
            {
                handAnimsPlayer2[i].pos++;
                continue;
            }
            else if (handAnimsPlayer2[i].transform == null && handAnimsPlayer2[i].pos == 4)
            {
                HandAnim hand = handAnimsPlayer2[i];
                handAnimsPlayer2.RemoveAt(i);
                i--;
                continue;
            }

            Animator animator = handAnimsPlayer2[i].transform.GetComponent<Animator>();
            if (handAnimsPlayer2[i].pos < 0)
            {
                handAnimsPlayer2[i].pos++;
                continue;
            }
            else if (handAnimsPlayer2[i].pos == 0)
            {
                animator.SetTrigger("Arrive");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 1)
            {
                animator.SetTrigger("Middle");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 2)
            {
                animator.SetTrigger("ToMiddle");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 3)
            {
                animator.SetTrigger("Quit");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 4)
            {
                HandAnim hand = handAnimsPlayer2[i];
                Destroy(hand.transform.gameObject);
                handAnimsPlayer2.RemoveAt(i);
                i--;
                continue;
            }

        }
    }

    private int GetPreviousHandAnimPos(int currentIndexFromList, List<HandAnim> listhandAnim)
    {
        if (currentIndexFromList == 0)
        {
            return -1;
        }
        else
        {
            return listhandAnim.ElementAt(currentIndexFromList - 1).pos;
        }
    }

    private bool HandAnimCanMoveToNextPoint(int currentPos, List<HandAnim> listhandAnim)
    {
        if (GetPreviousHandAnimPos(currentPos, listhandAnim) == -1)
            return true;
        else if (GetPreviousHandAnimPos(currentPos, listhandAnim) > listhandAnim[currentPos].pos + 1)
        {
            return true;
        }
        return false;
    }

    private void CreateMultipleHandSignsOnScreen(List<HandsSign> handsSigns)
    {
        bool isPlayer1 = handsSigns[0].player == HandsSign.PlayerNumber.Player1;
        for (int i = 0; i < handsSigns.Count; i++)
        {
            if (isPlayer1)
            {
                Debug.Log("Create Player 1 full");
                CreateHandSignplayer1(handsSigns[i], false);
            }
            else
            {
                Debug.Log("Create Player 2 full");
                CreateHandSignplayer2(handsSigns[i], false);
            }
        }
        
        for (int i = 0; i < handsSigns.Count; i++)
        {
            if (isPlayer1)
            {
                Debug.Log("Create Player 2 empty");
                CreateHandSignplayer2(handsSigns[i], true);
            }
            else
            {
                Debug.Log("Create Player 1 empty");
                CreateHandSignplayer1(handsSigns[i], true);
            }
        }
    }

    private void CreateHandSignOnScreen(HandsSign handsSign)
    {

        if (handsSign.player == HandsSign.PlayerNumber.Player1)
        {
            CreateHandSignplayer1(handsSign, false);
            CreateHandSignplayer2(handsSign, true);
        }
        else
        {
            CreateHandSignplayer1(handsSign, true);
            CreateHandSignplayer2(handsSign, false);
        }
    }

    private void CreateHandSignplayer1(HandsSign handsSign, bool isEmpty)
    {
        if (isEmpty)
        {
            AddEmptyHandSignFromListPlayer1(new HandsSign());
            return;
        }

        GameObject handsScreen = Instantiate(_prefabHandsScreenPlayer1, new Vector3(50, 50, 0), Quaternion.identity, _handsScreenPlayer1Parent.transform);
        handsScreen.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handsSign.handSignLeft.SpriteLeft;
        handsScreen.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handsSign.handSignRight.SpriteRight;
        AddHandSignFromList(handsScreen, handsSign);
    }

    private void CreateHandSignplayer2(HandsSign handsSign, bool isEmpty)
    {
        if (isEmpty)
        {
            AddEmptyHandSignFromListPlayer2(handsSign);
            return;
        }
        GameObject handsScreen = Instantiate(_prefabHandsScreenPlayer2, new Vector3(50, 50, 0), Quaternion.identity, _handsScreenPlayer2Parent.transform);
        handsScreen.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handsSign.handSignLeft.SpriteLeft;
        handsScreen.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handsSign.handSignRight.SpriteRight;
        AddHandSignFromList(handsScreen, handsSign);
    }

    private void AddHandSignFromList(GameObject handsScreen, HandsSign handsSign)
    {
        var handAnim = handsSign.player == HandsSign.PlayerNumber.Player1 ? handAnimsPlayer1 : handAnimsPlayer2;
        int lastPos = handAnim.Count > 0 && handAnim.Last().pos < 1 ? handAnim.Last().pos - 1 : 0;
        handAnim.Add(new HandAnim(handsScreen.transform, handsSign, lastPos));
    }
    private void AddEmptyHandSignFromListPlayer1(HandsSign handsSign)
    {
        List<HandAnim> handAnim = handAnimsPlayer1;

        int lastPos = handAnim.Count > 0 && handAnim.Last().pos < 1 ? handAnim.Last().pos - 1 : 0;
        handAnim.Add(new HandAnim(null, handsSign, lastPos));
    }
    private void AddEmptyHandSignFromListPlayer2(HandsSign handsSign)
    {
        List<HandAnim> handAnim = handAnimsPlayer2;

        int lastPos = handAnim.Count > 0 && handAnim.Last().pos < 1 ? handAnim.Last().pos - 1 : 0;
        handAnim.Add(new HandAnim(null, handsSign, lastPos));
    }

}
[System.Serializable]
public class HandAnim
{
    public HandsSign handsSign;
    public Transform transform;
    public int pos;

    public HandAnim(Transform transform, HandsSign handsSign, int pos = 0)
    {
        this.transform = transform;
        this.pos = pos;
        this.handsSign = handsSign;
    }
}