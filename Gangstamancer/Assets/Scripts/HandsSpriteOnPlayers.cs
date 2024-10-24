using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HandsSpriteOnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _handsOnPlayer1;
    [SerializeField] private GameObject _handsOnPlayer2;
    private Animator handPlayer1;
    private Animator handPlayer2;

    [SerializeField] private List<Fingers> _allFingers = new List<Fingers>();

    private void Start()
    {
        SelectRandomHandsAtStart(_handsOnPlayer1);
        SelectRandomHandsAtStart(_handsOnPlayer2);
    }

    private void SelectRandomHandsAtStart(GameObject parent)
    {
        Animator handPlayer = parent.GetComponent<Animator>();
        int random = Random.Range(0, _allFingers.Count);
        SendTriggerToAnimator(handPlayer, _allFingers[random]);
    }
    public void DiplayPlayer1Hands(bool display)
    {
        //if (!display) return;
        DisplayChildrenSprites(display, _handsOnPlayer1);
    }

    public void DiplayPlayer2Hands(bool display)
    {
        //if (!display) return;
        DisplayChildrenSprites(display, _handsOnPlayer2);
    }

    private void DisplayChildrenSprites(bool display, GameObject parent)
    {
        foreach (var item in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            item.enabled = display;
        }
    }

    public void PutGoodHandsOnPlayer(HandsSign handSign)
    {
        var handsOnPlayer = handSign.player == PlayerNumber.Player1 ? _handsOnPlayer1 : _handsOnPlayer2;
        Animator animator = handsOnPlayer.GetComponent<Animator>();
        SendTriggerToAnimator(animator, handSign.handSignLeft);
        handsOnPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handSign.handSignLeft.SpriteLeft;
        handsOnPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handSign.handSignRight.SpriteRight;
    }

    private void SendTriggerToAnimator(Animator animator, Fingers finger)
    {
        if (_allFingers[0] == finger)
            animator.SetTrigger("Call");
        else if (_allFingers[1] == finger)
            animator.SetTrigger("Cornes");
        else if (_allFingers[2] == finger)
            animator.SetTrigger("Fuck");
        else if (_allFingers[3] == finger)
            animator.SetTrigger("Gun");
        else if (_allFingers[4] == finger)
            animator.SetTrigger("Index");
        else if (_allFingers[5] == finger)
            animator.SetTrigger("Poing");
        
    }
}
