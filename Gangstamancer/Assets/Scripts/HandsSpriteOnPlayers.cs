using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsSpriteOnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _handsOnPlayer1;
    [SerializeField] private GameObject _handsOnPlayer2;
    [SerializeField] private Animator handPlayer1;
    [SerializeField] private Animator handPlayer2;

    [SerializeField] private List<Fingers> _handsSigns;


    public void DiplayPlayer1Hands(bool display)
    {
        _handsOnPlayer1.SetActive(display);
    }

    public void DiplayPlayer2Hands(bool display)
    {
        _handsOnPlayer2.SetActive(display);
    }

    public void PutGoodHandsOnPlayer1(HandsSign handSign)
    {
        SendTriggerToAnimator(handPlayer1, handSign.handSignLeft.name);
        _handsOnPlayer1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handSign.handSignLeft.SpriteLeft;
        _handsOnPlayer1.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handSign.handSignRight.SpriteRight;
    }

    public void PutGoodHandsOnPlayer2(HandsSign handSign)
    {
        SendTriggerToAnimator(handPlayer2, handSign.handSignLeft.name);
        _handsOnPlayer2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handSign.handSignLeft.SpriteLeft;
        _handsOnPlayer2.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handSign.handSignRight.SpriteRight;
    }

    private void SendTriggerToAnimator(Animator animator, string name)
    {
        switch (name)
        {
            case "Call me":
                animator.SetTrigger("Call");
                break;
            case "cornes du diable":
                animator.SetTrigger("Cornes");
                break;
            case "doigt d'honneur":
                animator.SetTrigger("Fuck");
                break;
            case "finger gun":
                animator.SetTrigger("Gun");
                break;
            case "index":
                animator.SetTrigger("Index");
                break;
            case "Poing":
                animator.SetTrigger("Poing");
                break;
        }
    }
}
