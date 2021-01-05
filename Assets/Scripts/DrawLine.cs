using Bolt;
using MiscUtil.Collections.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    public GameObject linePrefab;
    GameObject currentLine;

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    public List<Vector2> fingerPos;

    public List<GameObject> persos;
    
    bool StartLine = false, continueLine = true;


    // Update is called once per frame
    void Update()
    {
        //DONE
        #region Gestion input souris

#if UNITY_STANDALONE

        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D coll;

        if (Input.GetMouseButtonDown(0))    //check premier input
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

        if (Input.GetMouseButton(0) && StartLine == true && continueLine == true)        //check si input maintenu
        {

            if (Vector2.Distance(wp, fingerPos[fingerPos.Count - 1]) > 0.1f /*&& Vector2.Distance(wp, fingerPos[fingerPos.Count - 1]) < 0.8f*/) // si la position de la souris > que le dernier point dessiné
            {
                UpdateLine(wp);
            }

            foreach (var x in fingerPos)
            {
                if (Vector2.Distance(fingerPos[fingerPos.Count-1], x) < 0.1f && x!=fingerPos[fingerPos.Count-1])
                {
                    DeleteLine();
                    Debug.Log("touché");
                }
            }

            foreach (var x in GameManager.Instance.blocked)
            {
                coll = x.GetComponent<Collider2D>();

                if (coll.OverlapPoint(wp))
                {
                    DeleteLine();
                }
            }

            

        }

        /*foreach (var x in GameManager.Instance.cases)
        {
            coll = x.GetComponent<Collider2D>();
            if (coll.OverlapPoint(wp))
            {
                continueLine = true;
            }
        }*/

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

            foreach (var x in GameManager.Instance.blocked)
            {
                collT = x.GetComponent<Collider2D>();

                if (collT.OverlapPoint(tp))
                {
                    DeleteLine();
                    //continueLine = false;
                }
            }

            foreach (var x in GameManager.Instance.cases)
            {
                collT = x.GetComponent<Collider2D>();
                if (collT.OverlapPoint(tp))
                {
                    continueLine = true;
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
                    if (StartLine == true && continueLine == true)
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
        fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);  //ajoute un point dans la suite de la liste & à l'écran

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
        fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);

        edgeCollider.points = fingerPos.ToArray();
    }
#endif
    #endregion

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    void DeleteLine()
    {
        Destroy(currentLine);
        StartLine = false;
    }

}
