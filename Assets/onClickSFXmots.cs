using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClickSFXmots : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickSFX()
    {
        GameManager.Instance._SFX.clip = GameManager.Instance.trackSFX[2];
        GameManager.Instance._SFX.Play();
    }
}
