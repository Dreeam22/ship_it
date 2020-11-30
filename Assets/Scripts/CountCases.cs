using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountCases : MonoBehaviour
{
    // Start is called before the first frame update

    Text _caseTxt, _winTxt;
    public int compteur, comptPerso;
    string _connect1;
    void Start()
    {
        //compter le nombre max de cases / récupérer dans le GameManager?

         _caseTxt = GameObject.Find("CaseTxt").GetComponent<Text>();
        _winTxt = GameObject.Find("WinTxt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cases")
        {
            compteur++;
            _caseTxt.text = "Cases : " + compteur;
        }

        if (other.tag == "Player")
        {
            
            comptPerso++;
            if (comptPerso == 1)
            { 
              _connect1 = other.name; 
            }

            
            if (comptPerso == 2)
            {
                _winTxt.text = "Connected " + _connect1 + " with " + other.name;
                Time.timeScale = 0;
            }
        }


    }
}
