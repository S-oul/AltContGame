using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fondue : MonoBehaviour
{
    [SerializeField] Image fondue;
    void Start()
    {
        StartCoroutine(IgmFondue());
    }

    // Update is called once per frame
    IEnumerator IgmFondue()
    {
        while (fondue.color.a >= 0)
        {
            fondue.color -= new Color(0, 0, 0, 1f * Time.deltaTime);
            yield return null;
        }
        fondue.color = new Color(0, 0, 0, 0);
        fondue.enabled = false;
    }
}
