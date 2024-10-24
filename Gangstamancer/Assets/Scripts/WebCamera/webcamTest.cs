using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class webcamTest : MonoBehaviour
{

    WebCamDevice webCam;
    public Renderer webCamRenderer;

    // Start is called before the first frame update
    void Start()
    {
        WebCamTexture tt = new WebCamTexture();
        print(tt.deviceName);
        webCamRenderer = GetComponent<Renderer>();
        webCamRenderer.material.mainTexture = tt;
        tt.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
