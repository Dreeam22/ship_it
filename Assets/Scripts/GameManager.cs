using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{

    public int[,] plato;

    public GameObject _parent;
    public GameObject casePrefab;
    public GameObject currentCase;

    public float casewidth = 4.5f;
    public int  nbCX = 10 , nbCY = 10;

    // Start is called before the first frame update
    void Start()
    {
        plato = new int[nbCX, nbCY]; //création plateau 10 par 10

        for (int i =0; i <  nbCX ; i++)
        {            
            for (int j = 0; j < nbCY; j++)
            {
                currentCase = Instantiate(casePrefab, new Vector3(i-casewidth, j-casewidth, 0), Quaternion.identity, _parent.transform);             
                currentCase.name = i+j.ToString();

                if (currentCase.name == "45" || currentCase.name == "49" || currentCase.name == "44" || currentCase.name == "54" || currentCase.name == "55" || currentCase.name == "59"
                    || currentCase.name == "40" || currentCase.name == "50" || currentCase.name == "21" || currentCase.name == "22" || currentCase.name == "71" || currentCase.name == "72"
                    || currentCase.name == "28" || currentCase.name == "27" || currentCase.name == "78" || currentCase.name == "77" || currentCase.name == "15" || currentCase.name == "14"
                    || currentCase.name == "85" || currentCase.name == "84") 
                {
                    currentCase.GetComponent<SpriteRenderer>().color = Color.blue;
                    currentCase.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
