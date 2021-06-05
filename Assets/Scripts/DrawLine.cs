using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    #region variables
    public int[,] plato;

    public List<GameObject> blocked = new List<GameObject>();
    public List<GameObject> cases = new List<GameObject>();

    public GameObject _parent;
    public GameObject casePrefab;
    public GameObject currentCase;

    public float casewidth;
    public int nbCX = 10, nbCY = 10;
    public Color casesColor;

    public List<string> LD = new List<string>();
    string[] LD1;

    public GameObject linePrefab;
    GameObject currentLine;
    LineRenderer lineRenderer;
    public List<Vector2> fingerPos;
    public List<GameObject> persos;
    bool StartLine = false;
    public GameObject lastCase;
    public List<GameObject> casesDansLesquellesonAffiche;
    int n;


    Vector2 mousepoint;
    Vector2 tp;

    System.Random rnd;

    public int compteur = 0;
    public string p1, p2;
    public Color _checkedColor;
    public Color _lastColor;
    public Animator casesAnimator;

    bool fail = false;

    #endregion

    void Start()
    {
        rnd = new System.Random();
        GameManager.Instance.connected = false;
        GameManager.Instance._winTxt = GameObject.Find("WinTxt").GetComponent<TMP_Text>();
        GameManager.Instance._readTxt = GameObject.Find("ReadTxt").GetComponent<TMP_Text>();
        GameManager.Instance._winObj = GameObject.Find("_winobj");
        GameManager.Instance._unlokedTxt = GameObject.Find("UnlokedTxt").GetComponent<Text>();
        GameManager.Instance._winObj.SetActive(false);

        CreatePlato();

    }


    void CreatePlato()
    {

        n = rnd.Next(1, 7);
        TextAsset LDdata = Resources.Load<TextAsset>("LD" + n);
        LD1 = LDdata.text.Split(new char[] { ';' });


        #region Instanciation tablo
        plato = new int[nbCX, nbCY]; //création plateau 10 par 10

        for (int i = 0; i < nbCX; i++)
        {
            for (int j = 0; j < nbCY; j++)
            {
                currentCase = Instantiate(casePrefab, new Vector3(i - casewidth, j - casewidth, 0), Quaternion.identity, _parent.transform);
                currentCase.name = i.ToString() + ":" + j.ToString();

                foreach (string line in LD1)
                {
                    if (currentCase.name == line)
                    {
                        currentCase.GetComponent<SpriteRenderer>().color = casesColor;
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

    // Update is called once per frame
    void Update()
    {

        //DONE
        #region Gestion input souris

#if UNITY_STANDALONE

        mousepoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D coll;

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.connected == false && Time.timeScale == 1)    //check premier input
        {
            foreach (var x in persos)
            {
                coll = x.GetComponent<Collider2D>();
                if (coll.OverlapPoint(mousepoint))  //check si souris au dessus d'un perso
                {

                    CreateLine(x);
                    StartLine = true;

                    if (x.name == "p1") p1 = "Sasha";
                    if (x.name == "p2") p1 = "Charlie";
                    if (x.name == "p3") p1 = "Alex";
                    if (x.name == "p4") p1 = "Taylor";

                }
            }
        }


        if (Input.GetMouseButton(0) && StartLine == true && GameManager.Instance.connected == false)        //check si input maintenu
        {

            UpdateLine();

            foreach (var x in blocked)
            {
                coll = x.GetComponent<Collider2D>();

                if (coll.OverlapPoint(mousepoint))
                {
                    if (!fail)
                        StartCoroutine("Fail");
                }
            }



        }

        if (Input.GetMouseButtonUp(0) && GameManager.Instance.connected == false && StartLine == true)
            DeleteLine();
#endif
        #endregion
        //DONE
        #region Gestion input tactile
#if UNITY_ANDROID


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            tp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D collT;

            foreach (var x in blocked)
            {
                collT = x.GetComponent<Collider2D>();

                if (collT.OverlapPoint(tp))
                {
                    if (!fail)
                        StartCoroutine("Fail");

                }
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    foreach (var x in persos)   //ne créé le trait que si le doigt est sur un perso 
                    {
                        collT = x.GetComponent<Collider2D>();

                        if (collT.OverlapPoint(tp))
                        {
                            CreateLineTouch(x);
                            if (x.name == "p1") p1 = "Sasha";
                            if (x.name == "p2") p1 = "Charlie";
                            if (x.name == "p3") p1 = "Alex";
                            if (x.name == "p4") p1 = "Taylor";

                            StartLine = true;
                        }
                    }
                    break;


                case TouchPhase.Moved:
                    if (StartLine == true)
                    {
                        //regarde la position de la souris

                        UpdateLineTouch();

                    }
                    break;

                case TouchPhase.Ended:
                    if (GameManager.Instance.connected == false)
                    {
                        DeleteLine();
                    }
                    break;

            }
        }
#endif
        #endregion


        GameManager.Instance.saveCounter = compteur;

        #region remplir coeurs
        if (p1 != null)
        {
            if ((GameManager.Instance.relationLVL + 1) == 0)
            {
               FDBKPuzzleManager.Instance.Hearts[0].fillAmount = compteur*0.1f;
            }

            if ((GameManager.Instance.relationLVL + 1) == 1)
            {
                FDBKPuzzleManager.Instance.Hearts[1].fillAmount = ((compteur-10) * 0.025f);
            }

            if ((GameManager.Instance.relationLVL + 1) == 2)
            {
                FDBKPuzzleManager.Instance.Hearts[2].fillAmount = ((compteur-50) * 0.05f);
            }

        }
        #endregion
    }


    //DONE
    #region Gestion ligne PC
#if UNITY_STANDALONE
    void CreateLine(GameObject perso)
    {
        fingerPos.Clear(); 
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);   //Instantie le prefab dans current line
        lineRenderer = currentLine.GetComponent<LineRenderer>();                    //Récupère l'objet LineRenderer
                                                                 //Clear la liste de positions

        fingerPos.Add(perso.transform.position);         //Ajoute les 2 premiers points 
        fingerPos.Add(perso.transform.position);         //Ajoute les 2 premiers points 

        lineRenderer.positionCount++;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, fingerPos[0]);                                  //Les garde en mémoire dans la liste     
        lineRenderer.SetPosition(1, fingerPos[1]);                                  //Les garde en mémoire dans la liste     

        GameManager.Instance.MouseEnterCases.Add(perso);

    }

    void UpdateLine()
    {
        GameObject casequivaetreaffichee = GameManager.Instance.MouseEnterCases[GameManager.Instance.MouseEnterCases.Count - 1];

        if (casesDansLesquellesonAffiche.Contains(casequivaetreaffichee) && casequivaetreaffichee != lastCase)
        {
            for (int i = casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee); i < casesDansLesquellesonAffiche.Count; i++)
            { 
                casesDansLesquellesonAffiche[i].GetComponent<SpriteRenderer>().color = _lastColor;
            }

            casesDansLesquellesonAffiche.RemoveRange(casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee), casesDansLesquellesonAffiche.Count - casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee));
            lineRenderer.positionCount = casesDansLesquellesonAffiche.Count;
            lastCase = null;
        }
        else if ((lastCase == null || IsCaseAdjacente(lastCase, casequivaetreaffichee)) && casequivaetreaffichee.tag == "Cases") //affiche le trait et passe par la case
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, casequivaetreaffichee.transform.position);
            casesDansLesquellesonAffiche.Add(casequivaetreaffichee);
            lastCase = casequivaetreaffichee;

            casequivaetreaffichee.GetComponent<SpriteRenderer>().color = _checkedColor;
            casequivaetreaffichee.GetComponent<SpriteRenderer>().sortingLayerName = "FDBK";
            casequivaetreaffichee.GetComponent<Animator>().SetTrigger("TrigEnter");


            GameManager.Instance._SFX.clip = GameManager.Instance.trackSFX[1];
            GameManager.Instance._SFX.Play();


            fail = false;
        }
        compteur = casesDansLesquellesonAffiche.Count;

        foreach (var x in persos)
        {
           Collider2D coll = x.GetComponent<Collider2D>();
            if (coll.OverlapPoint(mousepoint) && lastCase!=null && (lastCase.name == "0:9" || lastCase.name == "9:9" || lastCase.name == "0:0"|| lastCase.name == "9:0"))  //check si souris au dessus d'un perso
            {
                if (x.name == "p1") p2 = "Sasha";
                if (x.name == "p2") p2 = "Charlie";
                if (x.name == "p3") p2 = "Alex";
                if (x.name == "p4") p2 = "Taylor";
                Debug.Log(p2);

                if (p1 != p2)
                    GameManager.Instance._connected(p1, p2);
            }
        }
       
    }
#endif
    #endregion

    //DONE
    #region Gestion ligne Android         
#if UNITY_ANDROID 
    void CreateLineTouch(GameObject perso)
    {
        fingerPos.Clear();
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);   //Instantie le prefab dans current line
        lineRenderer = currentLine.GetComponent<LineRenderer>();                    //Récupère l'objet LineRenderer
                                                                                    //Clear la liste de positions

        fingerPos.Add(perso.transform.position);         //Ajoute les 2 premiers points 
        fingerPos.Add(perso.transform.position);         //Ajoute les 2 premiers points 

        lineRenderer.positionCount++;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, fingerPos[0]);                                  //Les garde en mémoire dans la liste     
        lineRenderer.SetPosition(1, fingerPos[1]);                                  //Les garde en mémoire dans la liste     

        GameManager.Instance.MouseEnterCases.Add(perso);

    }

    void UpdateLineTouch()
    {

        GameObject casequivaetreaffichee = GameManager.Instance.MouseEnterCases[GameManager.Instance.MouseEnterCases.Count - 1];

        if (casesDansLesquellesonAffiche.Contains(casequivaetreaffichee) && casequivaetreaffichee != lastCase)
        {
            for (int i = casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee); i < casesDansLesquellesonAffiche.Count; i++)
            {
                casesDansLesquellesonAffiche[i].GetComponent<SpriteRenderer>().color = _lastColor;
            }

            casesDansLesquellesonAffiche.RemoveRange(casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee), casesDansLesquellesonAffiche.Count - casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee));
            lineRenderer.positionCount = casesDansLesquellesonAffiche.Count;
            lastCase = null;
        }
        else if ((lastCase == null || IsCaseAdjacente(lastCase, casequivaetreaffichee)) && casequivaetreaffichee.tag == "Cases")
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, casequivaetreaffichee.transform.position);
            casesDansLesquellesonAffiche.Add(casequivaetreaffichee);
            lastCase = casequivaetreaffichee;

            casequivaetreaffichee.GetComponent<SpriteRenderer>().color = _checkedColor;
            casequivaetreaffichee.GetComponent<SpriteRenderer>().sortingLayerName = "FDBK";
            casequivaetreaffichee.GetComponent<Animator>().SetTrigger("TrigEnter");

            GameManager.Instance._SFX.clip = GameManager.Instance.trackSFX[1];
            GameManager.Instance._SFX.Play();


            fail = false;

        }
        compteur = casesDansLesquellesonAffiche.Count;

        foreach (var x in persos)
        {
            Collider2D coll = x.GetComponent<Collider2D>();
            if (coll.OverlapPoint(tp) && lastCase != null && (lastCase.name == "0:9" || lastCase.name == "9:9" || lastCase.name == "0:0" || lastCase.name == "9:0"))  //check si souris au dessus d'un perso
            {
                if (x.name == "p1") p2 = "Sasha";
                if (x.name == "p2") p2 = "Charlie";
                if (x.name == "p3") p2 = "Alex";
                if (x.name == "p4") p2 = "Taylor";
                Debug.Log(p2);

                if (p1 != p2)
                    GameManager.Instance._connected(p1, p2);
            }
        }
    }
#endif
        #endregion

        List<GameObject> getCasesAdjacentes(GameObject caseActive)
        {
            List<GameObject> CasesAdjacentes = new List<GameObject>();
            if (caseActive)
            {
                int i;
                int.TryParse(caseActive.name.Split(':')[0], out i);

                int j;
                int.TryParse(caseActive.name.Split(':')[1], out j);


                if (GameObject.Find(i + ":" + (j + 1))) CasesAdjacentes.Add(GameObject.Find(i + ":" + (j + 1)));
                if (GameObject.Find(i + ":" + (j - 1))) CasesAdjacentes.Add(GameObject.Find(i + ":" + (j - 1)));
                if (GameObject.Find((i + 1) + ":" + j)) CasesAdjacentes.Add(GameObject.Find((i + 1) + ":" + j));
                if (GameObject.Find((i - 1) + ":" + j)) CasesAdjacentes.Add(GameObject.Find((i - 1) + ":" + j));

            }
            return CasesAdjacentes;
        }

        bool IsCaseAdjacente(GameObject caseActive, GameObject caseATester)
        {
            return getCasesAdjacentes(caseActive).Contains(caseATester);
        }


    public void DeleteLine()
    {
        Destroy(currentLine);
        StartLine = false;
        GameManager.Instance.relationLVL = -1;

        lastCase = null;
        GameManager.Instance.MouseEnterCases.Clear();

        //FDBKPuzzleManager.Instance._tab[GameManager.Instance.chara1].GetComponent<Animator>().SetTrigger("fail");
        //FDBKPuzzleManager.Instance._tab[0].GetComponent<Animator>().SetTrigger("fail");

        foreach (var x in cases) x.GetComponent<SpriteRenderer>().color = _lastColor;

        casesDansLesquellesonAffiche.Clear();
        compteur = 0;

        p1 = null;
        p2 = null;

        for (int i = 0; i < 3; i++) FDBKPuzzleManager.Instance.Hearts[i].fillAmount = 0;


    }

IEnumerator Fail()
    {
        fail = true;
        FDBKPuzzleManager.Instance._tab[GameManager.Instance.chara1].GetComponent<Animator>().SetTrigger("fail");
        foreach (var x in casesDansLesquellesonAffiche)
            x.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var x in casesDansLesquellesonAffiche)
            x.GetComponent<SpriteRenderer>().color = _checkedColor;

       
    }

    } 

