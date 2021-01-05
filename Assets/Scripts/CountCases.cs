using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountCases : MonoBehaviour
{
    // Start is called before the first frame update

    Text _caseTxt;
    public int compteur, comptPerso;
    string _connect1;
    Color _checkedColor = new Color(1,0.6f,1,0.5f);
    Color _lastColor;

    void Start()
    {
        //compter le nombre max de cases / récupérer dans le GameManager?

         _caseTxt = GameObject.Find("CaseTxt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.saveCounter = compteur;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cases")
        {
            compteur++;
            _caseTxt.text = "Cases : " + compteur;
            _lastColor = other.gameObject.GetComponent<SpriteRenderer>().color;

            other.gameObject.GetComponent<SpriteRenderer>().color = _checkedColor;
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
                GameManager.Instance._connected(_connect1, other.name);      
            }
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.GetComponent<SpriteRenderer>().color = _lastColor;
    }
}
