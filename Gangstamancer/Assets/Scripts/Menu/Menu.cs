using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI soustitre;
    [SerializeField] Image fondue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        soustitre.color = new Color(1,1,1,(Mathf.Sin(Time.time*2) + 1)/2); 


        if( (   Input.GetKey(KeyCode.A)
            && Input.GetKey(KeyCode.Z)
            && Input.GetKey(KeyCode.E)
            && Input.GetKey(KeyCode.R)
            
            && Input.GetKey(KeyCode.U)
            && Input.GetKey(KeyCode.I)
            && Input.GetKey(KeyCode.O)
            && Input.GetKey(KeyCode.P)

            && Input.GetKey(KeyCode.Q)
            && Input.GetKey(KeyCode.S)
            && Input.GetKey(KeyCode.D)
            && Input.GetKey(KeyCode.F)
            
            && Input.GetKey(KeyCode.J)
            && Input.GetKey(KeyCode.K)
            && Input.GetKey(KeyCode.L)
            && Input.GetKey(KeyCode.M))

            || Input.GetKey(KeyCode.Space))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        StartCoroutine(IgmFondue());
    }

    IEnumerator IgmFondue()
    {
        while(fondue.color.a <= .99f)
        {
            print(fondue.color.a);
            fondue.color += new Color(0, 0, 0, .1f * Time.deltaTime);
            yield return null;
        }
        fondue.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(1);
    }
}
