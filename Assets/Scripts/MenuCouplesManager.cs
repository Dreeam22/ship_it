using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCouplesManager : MonoBehaviour
{

    GameObject[] c;
    GameObject m;

    public GameObject[] R;

    public GameObject[] couplefill;
    // Start is called before the first frame update
    void Start()
    {

       c = new GameObject[6];
       m = GameObject.Find("COUPLES");
       
       
       for (int i = 0; i < R.Length; i++)
       {
           R[i].GetComponent<Button>().interactable = false;
       }


        foreach (int s in Storydata.ships)
        {
            R[s].GetComponent<Button>().interactable = true;

        }

        //couple 1
        if (R[0].GetComponent<Button>().interactable|| R[1].GetComponent<Button>().interactable|| R[2].GetComponent<Button>().interactable)
        {
            couplefill[0].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[0].GetComponentInChildren<Text>().text = "Sasha & Charlie";
        }
        if ((R[0].GetComponent<Button>().interactable && R[1].GetComponent<Button>().interactable) || (R[1].GetComponent<Button>().interactable && R[2].GetComponent<Button>().interactable ) || (R[0].GetComponent<Button>().interactable && R[2].GetComponent<Button>().interactable))
            couplefill[0].GetComponent<Image>().fillAmount = 0.66f;
        if (R[0].GetComponent<Button>().interactable && R[1].GetComponent<Button>().interactable && R[2].GetComponent<Button>().interactable)
            couplefill[0].GetComponent<Image>().fillAmount = 1.0f;

        //couple 2
        if (R[3].GetComponent<Button>().interactable  || R[4].GetComponent<Button>().interactable|| R[5].GetComponent<Button>().interactable)
        {
            couplefill[1].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[1].GetComponentInChildren<Text>().text = "Alex & Sasha";
        }
        if ((R[3].GetComponent<Button>().interactable && R[4].GetComponent<Button>().interactable) || (R[4].GetComponent<Button>().interactable && R[5].GetComponent<Button>().interactable) || (R[3].GetComponent<Button>().interactable && R[5].GetComponent<Button>().interactable)) 
            couplefill[1].GetComponent<Image>().fillAmount = 0.66f;
        if (R[3].GetComponent<Button>().interactable && R[4].GetComponent<Button>().interactable && R[5].GetComponent<Button>().interactable) 
            couplefill[1].GetComponent<Image>().fillAmount = 1.0f;

        //couple 3
        if (R[6].GetComponent<Button>().interactable || R[7].GetComponent<Button>().interactable || R[8].GetComponent<Button>().interactable)
        {
            couplefill[2].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[2].GetComponentInChildren<Text>().text = "Taylor & Sasha";
        }
        if ((R[6].GetComponent<Button>().interactable && R[7].GetComponent<Button>().interactable) || (R[7].GetComponent<Button>().interactable && R[8].GetComponent<Button>().interactable) || (R[6].GetComponent<Button>().interactable && R[8].GetComponent<Button>().interactable)) couplefill[2].GetComponent<Image>().fillAmount = 0.66f;
        if (R[6].GetComponent<Button>().interactable && R[7].GetComponent<Button>().interactable && R[8].GetComponent<Button>().interactable) couplefill[2].GetComponent<Image>().fillAmount = 1.0f;

        //couple 4
        if (R[9].GetComponent<Button>().interactable || R[10].GetComponent<Button>().interactable || R[11].GetComponent<Button>().interactable)
        {
            couplefill[3].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[3].GetComponentInChildren<Text>().text = "Alex & Taylor";
        }
        if ((R[9].GetComponent<Button>().interactable && R[10].GetComponent<Button>().interactable) || (R[10].GetComponent<Button>().interactable && R[11].GetComponent<Button>().interactable) || (R[9].GetComponent<Button>().interactable && R[11].GetComponent<Button>().interactable)) couplefill[3].GetComponent<Image>().fillAmount = 0.66f;
        if (R[9].GetComponent<Button>().interactable && R[10].GetComponent<Button>().interactable && R[11].GetComponent<Button>().interactable) couplefill[3].GetComponent<Image>().fillAmount = 1.0f;

        //couple 5
        if (R[12].GetComponent<Button>().interactable || R[13].GetComponent<Button>().interactable || R[14].GetComponent<Button>().interactable)
        {
            couplefill[4].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[4].GetComponentInChildren<Text>().text = "Charlie & Alex";
        }
        if ((R[12].GetComponent<Button>().interactable && R[13].GetComponent<Button>().interactable) || (R[13].GetComponent<Button>().interactable && R[14].GetComponent<Button>().interactable) || (R[12].GetComponent<Button>().interactable && R[14].GetComponent<Button>().interactable)) couplefill[4].GetComponent<Image>().fillAmount = 0.66f;
        if (R[12].GetComponent<Button>().interactable && R[13].GetComponent<Button>().interactable && R[14].GetComponent<Button>().interactable) couplefill[4].GetComponent<Image>().fillAmount = 1.0f;

        //couple 6
        if (R[15].GetComponent<Button>().interactable || R[16].GetComponent<Button>().interactable || R[17].GetComponent<Button>().interactable)
        {
            couplefill[5].GetComponent<Image>().fillAmount = 0.33f;
            couplefill[5].GetComponentInChildren<Text>().text = "Taylor & Charlie";
        }
        if ((R[15].GetComponent<Button>().interactable && R[16].GetComponent<Button>().interactable) || (R[16].GetComponent<Button>().interactable && R[17].GetComponent<Button>().interactable) || (R[15].GetComponent<Button>().interactable && R[17].GetComponent<Button>().interactable)) couplefill[5].GetComponent<Image>().fillAmount = 0.66f;
        if (R[15].GetComponent<Button>().interactable && R[16].GetComponent<Button>().interactable && R[17].GetComponent<Button>().interactable) couplefill[5].GetComponent<Image>().fillAmount = 1.0f;



        for (int j = 0; j < 6; j++)
       {
           c[j] = GameObject.Find("Couple" + j);
           c[j].SetActive(false);
       }

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
