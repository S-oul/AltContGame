using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) print("W");
        if (Input.GetKeyDown(KeyCode.Q)) print("A");
        if (Input.GetKeyDown(KeyCode.E)) print("E");
        if (Input.GetKeyDown(KeyCode.R)) print("R");
    }
}
