using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mockups_tests : MonoBehaviour
{
    int img = 0;
    public GameObject mockuptest;
    SpriteRenderer test;
    // Start is called before the first frame update
    void Start()
    {
        test = mockuptest.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(img);
        Debug.Log(test.sprite);

        switch (img)
        {
            case 0:               
                test.sprite = Resources.Load<Sprite>("Sprites/mockup_menu_princ");
                break;                                                                                        
            case 1:                                                                                           
                test.sprite = Resources.Load<Sprite>("Sprites/mockup_puzzle");
                break;                                                                                         
            case 2:                                                                                            
                test.sprite = Resources.Load<Sprite>("Sprites/mockup_histoire");
                break;                                                                                        
            case 3:                                                                                           
                test.sprite = Resources.Load<Sprite>("Sprites/mockup_menu");
                break;

        }
    }

    public void onClickChange()
    {
        
        switch (img)
        {
            case 0:
                img++;
                break;
            case 1:
                img++;
                break;
            case 2:
                img++;
                break;
            case 3:
                img = 0;
                break;

        }
    }
}
