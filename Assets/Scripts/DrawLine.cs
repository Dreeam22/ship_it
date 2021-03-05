﻿using Bolt;
using MiscUtil.Collections.Extensions;
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

    public List< string> LD = new List<string>();
    string[] LD1;

    public GameObject linePrefab;
    GameObject currentLine;
    LineRenderer lineRenderer;
    public List<Vector2> fingerPos;
    public List<GameObject> persos;
    bool StartLine = false;
    public GameObject lastCase;
    public List<GameObject> casesDansLesquellesonAffiche;

    public GameObject tutoObject;
    public GameObject mouse;
    Animator charaAnim;
    int a = 0;
    int n;
    GameObject[] _tab = new GameObject[4];

    Vector2 mousepoint;
    
    System.Random rnd;
    #endregion

    void Start()
    {
       rnd  = new System.Random();
        GameManager.Instance.connected = false;
        GameManager.Instance._winTxt = GameObject.Find("WinTxt").GetComponent<Text>();
        GameManager.Instance._winObj = GameObject.Find("_winobj");
        GameManager.Instance._winObj.SetActive(false);

        GameManager.Instance._shipTxt = GameObject.Find("ShipTxt").GetComponent<Text>();

        charaAnim = GameObject.Find("Persos").GetComponent<Animator>();
        

        CreatePlato();

        if (!Storydata.tuto)
        {
            tutoObject.SetActive(true);
            mouse.SetActive(true);
        }
        else
        {
            tutoObject.SetActive(false);
            mouse.SetActive(false);
        }

        if (!Storydata.prez)
        {
            GameObject.Find("Image").SetActive(true);
        }
        else
            GameObject.Find("Image").SetActive(false); ;

        _tab[0] = GameObject.Find("Sasha");
        _tab[1] = GameObject.Find("Charlie");
        _tab[2] = GameObject.Find("Alex");
        _tab[3] = GameObject.Find("Taylor");
    }


    void CreatePlato()
    {

        n = rnd.Next(1,5);
        TextAsset LDdata = Resources.Load<TextAsset>("LD"+n);
        LD1 = LDdata.text.Split(new char[] { ';' });
        Debug.Log(n);

       /* int n = 0;
        while (n<20)
        {
            int i = rnd.Next(0, nbCX);
            int j = rnd.Next(0, nbCY);
            string caseBloquée = i.ToString() +":"+ j.ToString();

            if (caseBloquée != "0:0" && caseBloquée != "0:9" && caseBloquée != "9:9" && caseBloquée!="9:0")
            {
                if (caseBloquée != (i+1).ToString() + ":" + (j+1).ToString() && caseBloquée != (i - 1).ToString() + ":" + (j - 1).ToString())
                {
                    LD.Add(caseBloquée);
                    n++;
                }
            }
        }*/
        
        #region Instanciation tablo
        plato = new int[nbCX, nbCY]; //création plateau 10 par 10

        for (int i = 0; i < nbCX; i++)
        {
            for (int j = 0; j < nbCY; j++)
            {
                currentCase = Instantiate(casePrefab, new Vector3(i / 1.5f - casewidth, j / 1.5f - casewidth, 0), Quaternion.identity, _parent.transform);
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

        #region Gestion Tuto
        if (tutoObject.activeInHierarchy == true && Input.GetMouseButtonDown(0))
        {
            tutoObject.GetComponent<Animator>().SetTrigger("fadeout");
            
        }

        if (!Storydata.tuto && GameManager.Instance.connected == true)
        {
            Debug.Log("test");
            mouse.SetActive(false);
            Storydata.tuto = true;
            SaveSystem.Save();
        }
        #endregion

        #region Gestion prez perso
        if (!Storydata.prez && GameObject.Find("Persos").activeInHierarchy)
        {      
            GameObject _img = GameObject.Find("Image");

            charaAnim.SetBool("IntroBool", true);

            if (Input.GetMouseButtonDown(0))
            {
                
                charaAnim.SetInteger("introInt", a);
                a++;

                switch (a)
                {
                    case 1:
                        _tab[0].gameObject.transform.SetParent(_img.transform);
                        break;

                    case 2:
                        _tab[1].gameObject.transform.SetParent(_img.transform);
                        break;

                    case 3:
                        _tab[2].gameObject.transform.SetParent(_img.transform);
                        break;

                    case 4:
                        _tab[3].gameObject.transform.SetParent(_img.transform);
                        break;

                    case 5:
                        for (int i =0; i<4;i++)
                        {
                            _tab[i].gameObject.transform.SetParent(GameObject.Find("Persos").transform);
                        }
                        //_img.SetActive(false);
                        break;

                }
                if (a > 5)
                {
                    charaAnim.SetBool("IntroBool", false);
                    Storydata.prez = true;
                    SaveSystem.Save();
                }
                
            }
        }
        #endregion

        //DONE
        #region Gestion input souris

#if UNITY_STANDALONE

        mousepoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D coll;

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.connected == false)    //check premier input
        {
            foreach (var x in persos)
            {
                coll = x.GetComponent<Collider2D>();
                if (coll.OverlapPoint(mousepoint))  //check si souris au dessus d'un perso
                {
                    CreateLine(x);
                    StartLine = true;
                    
                }
            }
        }
        

        if (Input.GetMouseButton(0) && StartLine == true )        //check si input maintenu
        {
            UpdateLine();         

            foreach (var x in blocked)
            {
                coll = x.GetComponent<Collider2D>();

                if (coll.OverlapPoint(mousepoint))
                {
                    DeleteLine();
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
            Vector2 tp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D collT;

            foreach (var x in blocked)
            {
                collT = x.GetComponent<Collider2D>();

                if (collT.OverlapPoint(tp))
                {
                    DeleteLine();
                    //continueLine = false;
                }
            }

            foreach (var x in cases)
            {
                collT = x.GetComponent<Collider2D>();
                if (collT.OverlapPoint(tp))
                {
                    //continueLine = true;
                }
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // A POLISH
                    foreach (var x in persos)   //ne créé le trait que si le doigt est sur un perso 
                    {
                        collT = x.GetComponent<Collider2D>();

                        if (collT.OverlapPoint(tp))
                        {
                            CreateLineTouch();
                            StartLine = true;
                        }
                    }
                    break;
                    

                case TouchPhase.Moved:
                    if (StartLine == true )
                    {
                          //regarde la position de la souris
                       if (Vector2.Distance(tp, fingerPos[fingerPos.Count - 1]) > 0.1f)// si la position de la souris > que le dernier point dessiné
                            {
                            UpdateLineTouch(tp);
                            }

                       foreach (var x in fingerPos)
                           {
                               if (Vector2.Distance(fingerPos[fingerPos.Count-1], x) < 0.1f && x!=fingerPos[fingerPos.Count-1])
                               {
                                   DeleteLine();
                                   Debug.Log("touché");
                               }
                           }
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

        //Gestion level relation
        //afficher des coeurs (1 à 4)
        //calculer la distance restante pour chacun des persos
        //mettre le nombre de coeurs correspondants pour chacun


        GameManager.Instance._shipTxt.text = "Ship level : " + (GameManager.Instance.relationLVL +1);  //affchage relation

    }


    //DONE
    #region Gestion ligne PC
#if UNITY_STANDALONE
    void CreateLine(GameObject perso)
    {
        
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);   //Instantie le prefab dans current line
        lineRenderer = currentLine.GetComponent<LineRenderer>();                    //Récupère l'objet LineRenderer
        fingerPos.Clear();                                                          //Clear la liste de positions

        fingerPos.Add(perso.transform.position);         //Ajoute les 2 premiers points 

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, fingerPos[0]);                                  //Les garde en mémoire dans la liste     

    }
    void UpdateLine()
    {

        GameObject casequivaetreaffichee = GameManager.Instance.MouseEnterCases[GameManager.Instance.MouseEnterCases.Count - 1];  

        if (casesDansLesquellesonAffiche.Contains(casequivaetreaffichee) && casequivaetreaffichee != lastCase )
        {
            casesDansLesquellesonAffiche.RemoveRange(casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee), casesDansLesquellesonAffiche.Count - casesDansLesquellesonAffiche.IndexOf(casequivaetreaffichee));
            lineRenderer.positionCount = casesDansLesquellesonAffiche.Count;
        }
        else if(lastCase == null || IsCaseAdjacente(lastCase, casequivaetreaffichee))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, casequivaetreaffichee.transform.position);
            casequivaetreaffichee.GetComponent<checkCases>().validé = true;
            casesDansLesquellesonAffiche.Add(casequivaetreaffichee);
            lastCase = casequivaetreaffichee;

            casequivaetreaffichee.GetComponent<SpriteRenderer>().color = CountCases.Instance._checkedColor;
            casequivaetreaffichee.GetComponent<SpriteRenderer>().sortingLayerName = "FDBK";
            casequivaetreaffichee.GetComponent<Animator>().SetTrigger("TrigEnter");


            CountCases.Instance.compteur++;

            /*if ((GameManager.Instance.relationLVL + 1) == 0)
                CountCases.Instance._caseTxt.text = "Before next LVL : " + (10 - CountCases.Instance.compteur);

            if ((GameManager.Instance.relationLVL + 1) == 1)
                CountCases.Instance._caseTxt.text = "Before next LVL : " + (50 - CountCases.Instance.compteur);

            if ((GameManager.Instance.relationLVL + 1) == 2)
                CountCases.Instance._caseTxt.text = "Before next LVL : " + (70 - CountCases.Instance.compteur);

            if (GameManager.Instance.relationLVL == 2)
                CountCases.Instance._caseTxt.text = "Max Level !";*/
        }

    }
#endif
    #endregion

    //DONE
    #region Gestion ligne Android         
#if UNITY_ANDROID 
    void CreateLineTouch()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPos.Clear();

        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));

        lineRenderer.SetPosition(0, fingerPos[0]);
        lineRenderer.SetPosition(1, fingerPos[1]);

        edgeCollider.points = fingerPos.ToArray();
    }

    void UpdateLineTouch(Vector2 newFingerPos)
    {
        /*fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);*/

        fingerPos.Add(newFingerPos);
        if (GameManager.Instance.caseTrigger == true)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, GameManager.Instance.casePos);  //ajoute un point dans la suite de la liste & à l'écran
        }

        edgeCollider.points = fingerPos.ToArray();
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
        StartLine = false;
        GameManager.Instance.relationLVL = -1;
        Destroy(currentLine);
        lastCase = null;
        GameManager.Instance.MouseEnterCases.Clear();
        _tab[0].GetComponent<Animator>().SetTrigger("fail");

    }

}
