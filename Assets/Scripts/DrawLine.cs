using Bolt;
using MiscUtil.Collections.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{

    public int[,] plato;

    public List<GameObject> blocked = new List<GameObject>();
    public List<GameObject> cases = new List<GameObject>();

    public GameObject _parent;
    public GameObject casePrefab;
    public GameObject currentCase;

    public float casewidth;
    public int nbCX = 10, nbCY = 10;

    public int LD;
    public string[] LD1;

    public GameObject linePrefab;
    GameObject currentLine;

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    public List<Vector2> fingerPos;

    public List<GameObject> persos;

    bool StartLine = false;

    Collider2D _platocoll;
    
    void Start()
    {

        GameManager.Instance.connected = false;
        GameManager.Instance._winTxt = GameObject.Find("WinTxt").GetComponent<Text>();
        GameManager.Instance._winObj = GameObject.Find("_winobj");
        GameManager.Instance._winObj.SetActive(false);

        GameManager.Instance._shipTxt = GameObject.Find("ShipTxt").GetComponent<Text>();

        _platocoll = GameObject.Find("plato").GetComponent<Collider2D>();

        CreatePlato();
    }


    void CreatePlato()
    {
        TextAsset LDdata = Resources.Load<TextAsset>("LD1");
        LD1 = LDdata.text.Split(new char[] { ';' });

        #region Instanciation tablo
        plato = new int[nbCX, nbCY]; //création plateau 10 par 10

        for (int i = 0; i < nbCX; i++)
        {
            for (int j = 0; j < nbCY; j++)
            {
                currentCase = Instantiate(casePrefab, new Vector3(i / 1.5f - casewidth, j / 1.5f - casewidth, 0), Quaternion.identity, _parent.transform);
                currentCase.name = i + j.ToString();

                foreach (string line in LD1)
                {
                    if (currentCase.name == line)
                    {
                        //Debug.Log(line);
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

    // Update is called once per frame
    void Update()
    {
        
        //DONE
        #region Gestion input souris

#if UNITY_STANDALONE

        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D coll;

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.connected == false)    //check premier input
        {
            foreach (var x in persos)
            {
                coll = x.GetComponent<Collider2D>();
                if (coll.OverlapPoint(wp))  //check si souris au dessus d'un perso
                {
                    StartLine = true;
                    CreateLine();
                }
            }
        }
        

        if (Input.GetMouseButton(0) && StartLine == true /*&& _platocoll.OverlapPoint(wp)*/ )        //check si input maintenu
        {

            if (Vector2.Distance(wp, fingerPos[fingerPos.Count - 1]) > 0.1f /*&& Vector2.Distance(wp, fingerPos[fingerPos.Count - 1]) < 0.8f*/) // si la position de la souris > que le dernier point dessiné
            {
                UpdateLine(wp);
            }

            foreach (var x in fingerPos)
            {
                if (Vector2.Distance(fingerPos[fingerPos.Count-1], x) < 0.1f && x!=fingerPos[fingerPos.Count-1]) //si le trait se retouche
                {
                    DeleteLine();
                    Debug.Log("touché");
                }
            }

            foreach (var x in blocked)
            {
                coll = x.GetComponent<Collider2D>();

                if (coll.OverlapPoint(wp))
                {
                    DeleteLine();
                }
            }

            

        }

        if (Input.GetMouseButtonUp(0) && GameManager.Instance.connected == false)
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
                       if (Vector2.Distance(tp, fingerPos[fingerPos.Count - 1]) > 0.1f && _platocoll.OverlapPoint(tp))// si la position de la souris > que le dernier point dessiné
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


        GameManager.Instance._shipTxt.text = "Ship level : " + (GameManager.Instance.relationLVL+1);

    }


    //DONE
    #region Gestion ligne PC
#if UNITY_STANDALONE
    void CreateLine()
    {
        
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);   //Instantie le prefab dans current line
        lineRenderer = currentLine.GetComponent<LineRenderer>();                    //Récupère l'objet LineRenderer
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();                  //Récupère le collider
        fingerPos.Clear();                                                          //Clear la liste de positions

        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));         //Ajoute les 2 premiers points 
        fingerPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.SetPosition(0, fingerPos[0]);                                  //Les garde en mémoire dans la liste
        lineRenderer.SetPosition(1, fingerPos[1]);

        edgeCollider.points = fingerPos.ToArray();

    }
    void UpdateLine(Vector2 newFingerPos)
    {

        //old
        /*fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);*/  //ajoute un point dans la suite de la liste & à l'écran


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


    public void DeleteLine()
    {
        Destroy(currentLine);
        StartLine = false;
    }

}
