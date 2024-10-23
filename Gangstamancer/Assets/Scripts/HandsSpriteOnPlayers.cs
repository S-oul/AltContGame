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
        handPlayer1 = _handsOnPlayer1.GetComponent<Animator>();
        handPlayer2 = _handsOnPlayer2.GetComponent<Animator>(); 
    }
    public void DiplayPlayer1Hands(bool display)
    {
        if (!display) return;
        _handsOnPlayer1.SetActive(display);
    }

    public void DiplayPlayer2Hands(bool display)
    {
        if (!display) return;
        _handsOnPlayer2.SetActive(display);
    }

    public void PutGoodHandsOnPlayer1(HandsSign handSign)
    {
        Debug.Log("PutGoodHandsOnPlayer1");
        SendTriggerToAnimator(handPlayer1, handSign.handSignLeft);
        _handsOnPlayer1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handSign.handSignLeft.SpriteLeft;
        _handsOnPlayer1.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handSign.handSignRight.SpriteRight;
    }

    public void PutGoodHandsOnPlayer2(HandsSign handSign)
    {
        Debug.Log("PutGoodHandsOnPlayer2");
        SendTriggerToAnimator(handPlayer2, handSign.handSignLeft);
        _handsOnPlayer2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handSign.handSignLeft.SpriteLeft;
        _handsOnPlayer2.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = handSign.handSignRight.SpriteRight;
    }

    private void SendTriggerToAnimator(Animator animator, Fingers finger)
    {
            Debug.Log(finger);
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
