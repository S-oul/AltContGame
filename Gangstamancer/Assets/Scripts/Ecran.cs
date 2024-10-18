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
        RythmTimeLine.RemoveOldHandSign += RemoveHandSignOnScreen;
    }

    private void OnDisable()
    {
        RythmTimeLine.CreateNewHandSign -= CreateHandSignOnScreen;
        RythmTimeLine.RemoveOldHandSign -= RemoveHandSignOnScreen;
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
            Animator animator = handAnimsPlayer1[i].transform.GetComponent<Animator>();
            if (handAnimsPlayer1[i].pos < 0)
            {
                handAnimsPlayer1[i].pos++;
                continue;
            }
            else if (handAnimsPlayer1[i].pos == 0 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer1))
            {
                animator.SetTrigger("Arrive");
                handAnimsPlayer1[i].pos++;
            }
            else if (handAnimsPlayer1[i].pos == 1 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer1))
            {
                animator.SetTrigger("Middle");
                handAnimsPlayer1[i].pos++;
            }
            else if (handAnimsPlayer1[i].pos == 2 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer1) && IsPlayerTurn(HandsSign.PlayerNumber.Player1))
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
            Animator animator = handAnimsPlayer2[i].transform.GetComponent<Animator>();
            if (handAnimsPlayer2[i].pos < 0)
            {
                handAnimsPlayer2[i].pos++;
                continue;
            }
            else if (handAnimsPlayer2[i].pos == 0 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer2))
            {
                animator.SetTrigger("Arrive");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 1 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer2))
            {
                animator.SetTrigger("Middle");
                handAnimsPlayer2[i].pos++;
            }
            else if (handAnimsPlayer2[i].pos == 2 && HandAnimCanMoveToNextPoint(i, handAnimsPlayer2) && IsPlayerTurn(HandsSign.PlayerNumber.Player2))
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

    private void CreateHandSignOnScreen(HandsSign handsSign)
    {
        GameObject handsScreenPrefab = handsSign.player == HandsSign.PlayerNumber.Player1 ? _prefabHandsScreenPlayer1 : _prefabHandsScreenPlayer2;
        GameObject handsScreenParent = handsSign.player == HandsSign.PlayerNumber.Player1 ? _handsScreenPlayer1Parent : _handsScreenPlayer2Parent;
        GameObject handsScreen = Instantiate(handsScreenPrefab, new Vector3(50, 50, 0), Quaternion.identity, handsScreenParent.transform);
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

    private void RemoveHandSignOnScreen(HandsSign handsSign)
    {

        return;
        GameObject handsScreenParent = handsSign.player == HandsSign.PlayerNumber.Player1 ? _handsScreenPlayer1Parent : _handsScreenPlayer2Parent;
        RemoveHandSignFromList(handsScreenParent.transform.GetChild(0).gameObject, handsSign);
        Destroy(handsScreenParent.transform.GetChild(0).gameObject);

    }

    private void RemoveHandSignFromList(GameObject handsScreen, HandsSign handsSign)
    {
        if (handsSign.player == HandsSign.PlayerNumber.Player1)
        {
            HandAnim hand = handAnimsPlayer1.Find(x => x.handsSign.Equals(handsSign));
            handAnimsPlayer1.Remove(hand);
            Destroy(hand.transform.gameObject);
        }
        else
        {
            HandAnim hand = handAnimsPlayer2.Find(x => x.handsSign.Equals(handsSign));
            handAnimsPlayer2.Remove(hand);
            Destroy(hand.transform.gameObject);
        }
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