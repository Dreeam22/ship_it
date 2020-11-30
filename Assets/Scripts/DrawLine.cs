using Bolt;
using MiscUtil.Collections.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;

    public List<Vector2> fingerPos;

    public List<GameObject> persos;

    bool line = false;

    // Update is called once per frame
    void Update()
    {
        //DONE
        #region Gestion input souris
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))    //check premier input
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D coll;
            foreach (var x in persos)
            {
                coll = x.GetComponent<Collider2D>();

                if (coll.OverlapPoint(wp))
                {
                    line = true;
                    CreateLine();
                }
            }   
        }

        if (Input.GetMouseButton(0) && line == true)        //check si input maintenu
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //regarde la position de la souris
            if (Vector2.Distance(tempFingerPos, fingerPos[fingerPos.Count - 1]) > 0.1f)         // si la position de la souris > que le dernier point dessiné
            {
                UpdateLine(tempFingerPos); 
            }
        }

       if (Input.GetMouseButtonUp(0))
            DeleteLine();
#endif
        #endregion
        //DONE
        #region Gestion input tactile
#if UNITY_ANDROID || UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);         

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    Collider2D coll;
                    foreach (var x in persos)
                    {
                        coll = x.GetComponent<Collider2D>();

                        if (coll.OverlapPoint(wp))
                        {
                            CreateLineTouch();
                            line = true;
                        }
                    }
                    break;
                    

                case TouchPhase.Moved:
                    if (line == true)
                    {
                        Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(touch.position);  //regarde la position de la souris
                        if (Vector2.Distance(tempFingerPos, fingerPos[fingerPos.Count - 1]) > 0.1f)         // si la position de la souris > que le dernier point dessiné
                        {
                            UpdateLineTouch(tempFingerPos);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    DeleteLine();
                    break;

            }
        }
#endif
        #endregion

        //A FAIRE
        #region Gestion trait se retouche
        
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
#if UNITY_ANDROID || UNITY_EDITOR
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
        line = false;
    }

}
