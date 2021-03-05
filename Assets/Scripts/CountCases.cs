using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountCases : MonoBehaviour
{
    // Start is called before the first frame update
    public static CountCases Instance;
    public Text _caseTxt;
    public int compteur=0, comptPerso;
    public string _connect1;
    public Color _checkedColor = new Color(1,0.6f,1,0.5f);
    public Color _lastColor;
    public Animator casesAnimator;
    public Collider2D otherColl;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

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
        otherColl = other;
        if (other.tag == "Cases")
        {
            GameManager.Instance.caseTrigger = true;

            GameManager.Instance.caseActive = other.gameObject;
            
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
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            other.GetComponent<checkCases>().validé = false;
           
        }
        GameManager.Instance.caseTrigger = false;
    }

}
