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
        foreach (HandAnim t in handAnims)
        {
            Animator animator = t.transform.GetComponent<Animator>();
            if(t.pos == 0)
            {
                animator.SetTrigger("Arrive");
            }
            if (t.pos == 1)
            {
                animator.SetTrigger("Middle");
            }
            if (t.pos == 2)
            {
                animator.SetTrigger("Leave");
            }
        }
    }

}
[System.Serializable]
public struct HandAnim
{
    public Transform transform;
    public int pos;
}