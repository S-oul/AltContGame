using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ecran : MonoBehaviour
{
    [SerializeField] List<HandAnim> handAnims = new List<HandAnim>();

    void Start()
    {
        RythmTimeLine.OnBeat += DoOnBeat;
    }

    void DoOnBeat()
    {
        for (int i = 0; i < handAnims.Count; i++)
        {
            Animator animator = handAnims[i].transform.GetComponent<Animator>();
            if (handAnims[i].pos == 0)
            {
                animator.SetTrigger("Arrive");
            }
            if (handAnims[i].pos == 1)
            {
                animator.SetTrigger("Middle");
            }
            if (handAnims[i].pos == 2)
            {
                animator.SetTrigger("Leave");
            }
            handAnims[i].pos++;
        }
    }

}
[System.Serializable]
public class HandAnim
{
    public Transform transform;
    public int pos;
}