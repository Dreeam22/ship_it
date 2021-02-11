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
    public Color _checkedColor = new Color(1,0.6f,1,0.5f);
    Color _lastColor;
    Animator casesAnimator;
    

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
            #region compte case
            GameManager.Instance.caseTrigger = true;
            
            GameManager.Instance.casePos = other.gameObject.transform.position;

            compteur++;


            if ((GameManager.Instance.relationLVL +1) == 0)
                _caseTxt.text = "Before next LVL : " + (10 - compteur);

            if ((GameManager.Instance.relationLVL+1) == 1)
                _caseTxt.text = "Before next LVL : " + (50 - compteur);

            if ((GameManager.Instance.relationLVL+1) == 2)
                _caseTxt.text = "Before next LVL : " + (70 - compteur);

            if (GameManager.Instance.relationLVL == 2)
                _caseTxt.text = "Max Level !";

            #endregion

            _lastColor = other.gameObject.GetComponent<SpriteRenderer>().color;
            other.gameObject.GetComponent<SpriteRenderer>().color = _checkedColor;

            casesAnimator = other.gameObject.GetComponent<Animator>();
            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FDBK";

            casesAnimator.SetTrigger("TrigEnter");

        }

        if (other.tag == "Player")
        {
            #region noms persos connectés
            comptPerso++;
            if (comptPerso == 1)
            { 
              _connect1 = other.name; 
            }

            
            if (comptPerso == 2)
            {
                GameManager.Instance._connected(_connect1, other.name);      
            }
            #endregion

        }

        if (other.name == "Wall")
        {
            Debug.Log(other.name);
            GameObject.Find("DrawLine").GetComponent<DrawLine>().DeleteLine();
        }




    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Cases")
        { 
            other.gameObject.GetComponent<SpriteRenderer>().color = _lastColor; 
        }
        GameManager.Instance.caseTrigger = false;
    }

}
