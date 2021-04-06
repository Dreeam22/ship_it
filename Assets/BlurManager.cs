using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurManager : MonoBehaviour
{

    public Camera BlurCam;
    public Material BlurMat;

    // Start is called before the first frame update
    void Start()
    {
        if(BlurCam.targetTexture !=null)
        {
            BlurCam.targetTexture.Release();
        }

        BlurCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        BlurMat.SetTexture("_RenText", BlurCam.targetTexture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
