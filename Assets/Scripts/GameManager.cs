using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int[,] plato;

    public GameObject _parent;
    public GameObject casePrefab;
    public GameObject currentCase;

    public float casewidth = 4.5f;
    public int  nbCX = 10 , nbCY = 10;

    public List<GameObject> blocked = new List<GameObject>();
    public List<GameObject> cases = new List<GameObject>();

    public bool connected = false;

    public ParticleSystem finished;

    Text _winTxt;
    GameObject _winObj;
    public int LD;
    string[] LD1;

    public int saveCounter;
    public int relation = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _winTxt = GameObject.Find("WinTxt").GetComponent<Text>();
        _winObj = GameObject.Find("_winobj");
        _winObj.SetActive(false);

        CreatePlato();     
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(connected);
        //Game states
        switch (saveCounter)
        {
            case 10:
                relation = 1;
                break;
            case 50:
                relation = 2;
                break;
            case 70:
                relation = 3;
                break;

        }
    }

    void CreatePlato()
    {

        #region Instanciation tablo
        plato = new int[nbCX, nbCY]; //création plateau 10 par 10
        

        switch (LD)
        {
            case 0:
                LD1 = System.IO.File.ReadAllLines(@"F:\Cours\_Project\ship_it\Assets\Resources\LD1.txt");
                break;
            case 1:
                LD1 = System.IO.File.ReadAllLines(@"F:\Cours\_Project\ship_it\Assets\Resources\LD2.txt");
                break;
        }


        for (int i = 0; i < nbCX; i++)
        {
            for (int j = 0; j < nbCY; j++)
            {
                currentCase = Instantiate(casePrefab, new Vector3(i - casewidth, j - casewidth, 0), Quaternion.identity, _parent.transform);
                currentCase.name = i + j.ToString();

                foreach (string line in LD1)
                {
                    if (currentCase.name == line)
                    {
                        currentCase.GetComponent<SpriteRenderer>().color = Color.blue;
                        //currentCase.GetComponent<BoxCollider2D>().enabled = false;
                        currentCase.tag = "blocked";
                    }
                }

            }
        }


        foreach (GameObject bloké in GameObject.FindGameObjectsWithTag("blocked"))
        {
            blocked.Add(bloké);
        }

        foreach (GameObject kases in GameObject.FindGameObjectsWithTag("Cases"))
            cases.Add(kases);
        #endregion
    }

    public void _connected(string p1, string p2)
    {
        connected = true;
        //lancer FDBK
        finished.Play();
        //afficher texte
        _winObj.SetActive(true);
        _winTxt.text = "Connected " + p1 + " with " + p2 ;
        //Save nb cases -> fait dans countcases
 

    }


    public void onclickLoad(int x)
    {   
        switch(x)
        {
            case 0:
                Destroy(this.gameObject);
                SceneManager.LoadScene("Proto_Scene");
                break;
            case 1:
                SceneManager.LoadScene("Story_Scene");
                break;
        }
    }

    
}
