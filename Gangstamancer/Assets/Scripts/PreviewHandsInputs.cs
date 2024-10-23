using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Text;

public class PreviewHandsInputs : MonoBehaviour
{
    [Header("Players Inputs")]
    [SerializeField] private PlayerHandsInput _player1Inputs;
    [SerializeField] private PlayerHandsInput _player2Inputs;

    [Foldout("Fingers Preview")]
    [SerializeField] Sprite _hand;
    [Foldout("Fingers Preview")]
    [SerializeField] List<Sprite> fingersOpened = new List<Sprite>();
    [Foldout("Fingers Preview")]
    [SerializeField] List<Sprite> fingersClosed = new List<Sprite>();

    [Header("Player 1 Hands")]
    [SerializeField] private Transform _player1Lefthand;
    [SerializeField] private Transform _player1Righthand;

    [Header("Player 2 Hands")]
    [SerializeField] private Transform _player2Lefthand;
    [SerializeField] private Transform _player2Righthand;

    [SerializeField] public static StringBuilder input1 = new StringBuilder("00000000");

    private void Update()
    {
        CheckPlayer1Inputs();
        CheckPlayer2Inputs();   
    }

    private void CheckPlayer1Inputs()
    {
        for (int i = 0; i < _player1Inputs.LeftHandInputs.Count; i++)
        {
            if (Input.GetKeyDown(_player1Inputs.LeftHandInputs[i]))
            {
                ReplaceFinger(_player1Lefthand, fingersClosed, i);
                input1[i] = '1';
            }
            else if (Input.GetKeyUp(_player1Inputs.LeftHandInputs[i]))
            {
                ReplaceFinger(_player1Lefthand, fingersOpened, i);
                input1[i] = '0';

            }
        }

        for (int i = 0; i < _player1Inputs.RightHandInputs.Count; i++)
        {
            if (Input.GetKeyDown(_player1Inputs.RightHandInputs[i]))
            {
                ReplaceFinger(_player1Righthand, fingersClosed, i);
                input1[i + 4] = '1';

            }
            else if (Input.GetKeyUp(_player1Inputs.RightHandInputs[i]))
            {
                ReplaceFinger(_player1Righthand, fingersOpened, i);
                input1[i+4] = '0';

            }
        }
        //print(input1.ToString());
    }

    private void CheckPlayer2Inputs()
    {
        for (int i = 0; i < _player2Inputs.LeftHandInputs.Count; i++)
        {
            if (Input.GetKeyDown(_player2Inputs.LeftHandInputs[i]))
            {
                ReplaceFinger(_player2Lefthand, fingersClosed, i);
            }
            else if (Input.GetKeyUp(_player2Inputs.LeftHandInputs[i]))
            {
                ReplaceFinger(_player2Lefthand, fingersOpened, i);
            }
        }

        for (int i = 0; i < _player2Inputs.RightHandInputs.Count; i++)
        {
            if (Input.GetKeyDown(_player2Inputs.RightHandInputs[i]))
            {
                ReplaceFinger(_player2Righthand, fingersClosed, i);
            }
            else if (Input.GetKeyUp(_player2Inputs.RightHandInputs[i]))
            {
                ReplaceFinger(_player2Righthand, fingersOpened, i);
            }
        }
    }   

    private void ReplaceFinger(Transform hand, List<Sprite> sprites, int index)
    {
        hand.GetChild(index).GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
