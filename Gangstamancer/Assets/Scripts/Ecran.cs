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
        RythmTimeLine.OnBeat += DoOnBeat;
        RythmTimeLine.CreateNewHandSign += CreateHandSignOnScreen;
        RythmTimeLine.CreateNewMultipleHandsSigns += CreateMultipleHandSignsOnScreen;
    }

    private void OnDisable()
    {
        RythmTimeLine.OnBeat -= DoOnBeat;
        RythmTimeLine.CreateNewHandSign -= CreateHandSignOnScreen;
        RythmTimeLine.CreateNewMultipleHandsSigns -= CreateMultipleHandSignsOnScreen;
    }

    private void DoOnBeat()
    {
        UpdateHandAnimationsPlayer1();
        UpdateHandAnimationsPlayer2();
    }

    private void UpdateHandAnimationsPlayer1()
    {

        for (int i = 0; i < handAnimsPlayer1.Count; i++)
        {
            HandAnim currentHandAnim = handAnimsPlayer1[i];

            if (currentHandAnim.transform == null)
            {
                if (currentHandAnim.pos < 4)
                {
                    currentHandAnim.pos++;
                }
                else if (currentHandAnim.pos == 4)
                {
                    handAnimsPlayer1.RemoveAt(i);
                    i--;
                }
                continue;
            }

            Animator animator = currentHandAnim.transform.GetComponent<Animator>();

            if (currentHandAnim.pos == 0)
            {
                animator.SetTrigger("Arrive");
            }
            else if (currentHandAnim.pos == 1)
            {
                animator.SetTrigger("Middle");
            }
            else if (currentHandAnim.pos == 2)
            {
                animator.SetTrigger("ToMiddle");
            }
            else if (currentHandAnim.pos == 3)
            {
                animator.SetTrigger("Quit");
            }
            else if (currentHandAnim.pos == 4)
            {
                Destroy(currentHandAnim.transform.gameObject);
                handAnimsPlayer1.RemoveAt(i);
                i--;
                continue;
            }

            currentHandAnim.pos++;

        }
    }

    private void UpdateHandAnimationsPlayer2()
    {

        for (int i = 0; i < handAnimsPlayer2.Count; i++)
        {
            HandAnim currentHandAnim = handAnimsPlayer2[i];

            if (currentHandAnim.transform == null)
            {
                if (currentHandAnim.pos < 4)
                {
                    currentHandAnim.pos++;
                }
                else if (currentHandAnim.pos == 4)
                {
                    handAnimsPlayer2.RemoveAt(i);
                    i--;
                }
                continue;
            }
            

            Animator animator = currentHandAnim.transform.GetComponent<Animator>();
            
            if (currentHandAnim.pos == 0)
            {
                animator.SetTrigger("Arrive");
            }
            else if (currentHandAnim.pos == 1)
            {
                animator.SetTrigger("Middle");
            }
            else if (currentHandAnim.pos == 2)
            {
                animator.SetTrigger("ToMiddle");
            }
            else if (currentHandAnim.pos == 3)
            {
                animator.SetTrigger("Quit");
            }
            else if (currentHandAnim.pos == 4)
            {
                Destroy(currentHandAnim.transform.gameObject);
                handAnimsPlayer2.RemoveAt(i);
                i--;
                continue;
            }

            currentHandAnim.pos++;

        }
    }

    private void CreateHandSignOnScreen(HandsSign handsSign)
    {
        CreateHandSignToPlayer(handsSign);
        AddEmptyHandSignToPlayerList(handsSign.player == PlayerNumber.Player1 ? PlayerNumber.Player2 : PlayerNumber.Player1);
    }

    private void CreateMultipleHandSignsOnScreen(List<HandsSign> handsSigns)
    {
        for (int i = 0; i < handsSigns.Count; i++)
        {
            CreateHandSignToPlayer(handsSigns[i]);
            AddEmptyHandSignToPlayerList(handsSigns[0].player == PlayerNumber.Player1 ? PlayerNumber.Player2 : PlayerNumber.Player1);
        }
    }

    private void CreateHandSignToPlayer(HandsSign handsSign)
    {
        GameObject handsScreen = Instantiate(GetPlayerHandScreenPrefab(handsSign.player), new Vector3(50, 50, 0), Quaternion.identity, GetParentHandScreen(handsSign.player));
        handsScreen.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handsSign.handSignLeft.SpriteToDoLeft;
        handsScreen.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handsSign.handSignRight.SpriteToDoRight;
        AddHandSignToList(handsScreen, handsSign);
    }

    private GameObject GetPlayerHandScreenPrefab(PlayerNumber playerNumber) => playerNumber switch
    {
        PlayerNumber.Player1 => _prefabHandsScreenPlayer1,
        PlayerNumber.Player2 => _prefabHandsScreenPlayer2,
        _ => null,
    };

    private Transform GetParentHandScreen(PlayerNumber playerNumber) => playerNumber switch
    {
        PlayerNumber.Player1 => _handsScreenPlayer1Parent.transform,
        PlayerNumber.Player2 => _handsScreenPlayer2Parent.transform,
        _ => null,
    };

    private void AddHandSignToList(GameObject handsScreen, HandsSign handsSign)
    {
        List<HandAnim> handAnim = GetPlayerHandAnimList(handsSign.player);
        int lastPos = IsHandAnimListNotEmpty(handAnim) && handAnim.Last().pos < 1 ? handAnim.Last().pos - 1 : 0;
        handAnim.Add(new HandAnim(handsScreen.transform, handsSign, lastPos));
    }

    private bool IsHandAnimListNotEmpty(List<HandAnim> handAnim) => handAnim.Count > 0;

    private void AddEmptyHandSignToPlayerList(PlayerNumber playerNumber)
    {
        List<HandAnim> handAnim = GetPlayerHandAnimList(playerNumber);

        int lastPos = IsHandAnimListNotEmpty(handAnim) && handAnim.Last().pos < 1 ? handAnim.Last().pos - 1 : 0;
        handAnim.Add(new HandAnim(lastPos));
    }

    private List<HandAnim> GetPlayerHandAnimList(PlayerNumber playerNumber) => playerNumber switch
    {
        PlayerNumber.Player1 => handAnimsPlayer1,
        PlayerNumber.Player2 => handAnimsPlayer2,
        _ => null,
    };


    #region OldMethods

    private bool IsPlayerTurn(PlayerNumber player)
    {
        if (player == PlayerNumber.Player1)
            return GameManager.Instance.CurrentState == GameManager.GameStates.Player1Defense || GameManager.Instance.CurrentState == GameManager.GameStates.Player1Attack;
        else
            return GameManager.Instance.CurrentState == GameManager.GameStates.Player2Defense || GameManager.Instance.CurrentState == GameManager.GameStates.Player2Attack;
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

    #endregion


}
[System.Serializable]
public class HandAnim
{
    public HandsSign? handsSign;
    public Transform transform;
    public int pos;

    public HandAnim(Transform transform, HandsSign handsSign, int pos = 0)
    {
        this.transform = transform;
        this.pos = pos;
        this.handsSign = handsSign;
    }

    public HandAnim(int pos = 0)
    {
        this.pos = pos;
        this.transform = null;
        this.handsSign = null;
    }
}
