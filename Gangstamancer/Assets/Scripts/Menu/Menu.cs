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

    bool _hasStarted = false;
    

    // Update is called once per frame
    void Update()
    {

        soustitre.color = new Color(0, 0, 0, (Mathf.Sin(Time.time * 2) + 1) / 2);


        if ((Input.GetKey(KeyCode.A)
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
            && Input.GetKey(KeyCode.M)
            )

            || Input.GetKey(KeyCode.Space))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        if (!_hasStarted)
        {
            StartCoroutine(IgmFondue());
            _hasStarted = true;
        }
    }

    IEnumerator IgmFondue()
    {
        while (fondue.color.a <= .99f)
        {
            fondue.color += new Color(0, 0, 0, 1f * Time.deltaTime);
            yield return null;
        }
        fondue.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(1);
    }

    private void OnGUI()
    {
        GUILayout.Label("Version : " + Application.version);

    }
}
