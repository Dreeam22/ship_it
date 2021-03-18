using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class changeScene : MonoBehaviour
{

    public GameObject[] c;
    GameObject m;
    public GameObject pauseObj;
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "Menu_Couples")
        {
            c = new GameObject[6];
            m = GameObject.Find("COUPLES");


            for (int i = 0; i < 18; i++)
            {
                GameObject R = GameObject.Find("R" + i);
                R.GetComponent<Button>().interactable = false;
            }

           
            foreach (int s in Storydata.ships)
            {
                GameObject R = GameObject.Find("R" + s);
                R.GetComponent<Button>().interactable = true;
                //Debug.Log(R);
            }
           

            for (int j = 0; j < 6; j++)
            {
                c[j] = GameObject.Find("Couple" + j);
                c[j].SetActive(false);
            }
        }
    }

    public void ShowRelationship(int shipNb)
    {
        switch (shipNb)
        {
            case 0:
                m.SetActive(false);
                c[0].SetActive(true);
                break;

                case 1:
                m.SetActive(false);               
                c[1].SetActive(true);
                break;

                case 2:
                m.SetActive(false);
                c[2].SetActive(true);
                break;

                case 3:
                m.SetActive(false);
                c[3].SetActive(true);
                break;

                case 4:
                m.SetActive(false);
                c[4].SetActive(true);
                break;

                case 5:
                m.SetActive(false);
                c[5].SetActive(true);
                break;

            case 6:
                c[0].SetActive(false);
                c[1].SetActive(false);
                c[2].SetActive(false);
                c[3].SetActive(false);
                c[4].SetActive(false);
                c[5].SetActive(false);
                m.SetActive(true);
                break;


        }
    }

    public void LoadSceneCouple(int coupleID)
    {
        //regarder sur quel couple on clique   
        
        GameManager.Instance.coupleID = coupleID;
        GameManager.Instance.fromMenuCouple = true;
        SceneManager.LoadScene(3);  
    }

    public void onclickLoad(int x)
    {
        switch (x)
        {
            case 1:
                GameManager.Instance._audio.clip = GameManager.Instance.a[1];
                GameManager.Instance._audio.Play();
                GameManager.Instance.relationLVL = -1;
                Destroy(this.gameObject);
               break;

            case 3:
                GameManager.Instance.fromMenuCouple = false;
                GameManager.Instance._audio.clip = GameManager.Instance.a[2];
                GameManager.Instance._audio.Play();
               break;
        }
        SceneManager.LoadScene(x);

    }

    public void onCickPause(int i)
    {
        switch (i)
        {
            case 0:
                Time.timeScale = 0;
                pauseObj.SetActive(true);
                return;
            case 1:
                Time.timeScale = 1;
                pauseObj.SetActive(false);
                return;
        }
         
    }

    public void quit()
    {
        Application.Quit();
    }
}
