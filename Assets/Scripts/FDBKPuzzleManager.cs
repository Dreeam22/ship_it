using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FDBKPuzzleManager : MonoBehaviour
{
    public static FDBKPuzzleManager Instance;

    Animator charaAnim;
    public GameObject tutoObject;
    public GameObject mouse;

    int a = 0;
    public GameObject[] _tab;
    public GameObject[] buttonDesc;

    public List<Image> BG_desc;
    public List<Image> Hearts;

    bool sonjoué = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

        // Start is called before the first frame update
        void Start()
    {

        charaAnim = GameObject.Find("Persos").GetComponent<Animator>();


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
            GameObject.Find("Image_Persos").SetActive(true);
        }
        else
            GameObject.Find("Image_Persos").SetActive(false); ;



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
            mouse.SetActive(false);
            Storydata.tuto = true;

            SaveSystem.Save();
        }
        #endregion

        #region Gestion prez perso
        if (!Storydata.prez && GameObject.Find("Persos").activeInHierarchy)
        {
            GameObject _img = GameObject.Find("Image_Persos");

            charaAnim.SetBool("IntroBool", true);
            for (int i = 0; i < buttonDesc.Length; i++) buttonDesc[i].SetActive(false);

            if (Input.GetMouseButtonDown(0))
            {

                charaAnim.SetInteger("introInt", a);
                a++;

                if (a >= 5)
                {
                    _img.SetActive(false);
                    charaAnim.SetBool("IntroBool", false);
                    Storydata.prez = true;

                }

            }
        }
        if (Storydata.prez) for (int i = 0; i < buttonDesc.Length; i++) buttonDesc[i].SetActive(true);

        #endregion

        #region fdbk niveau relation validé

        if (GameManager.Instance.relationLVL == 0) Hearts[0].GetComponent<Animator>().SetBool("_filled", true);
        else if (GameManager.Instance.relationLVL == 1) Hearts[1].GetComponent<Animator>().SetBool("_filled", true);
        else if (GameManager.Instance.relationLVL == 2) Hearts[2].GetComponent<Animator>().SetBool("_filled", true);
        else { Hearts[0].GetComponent<Animator>().SetBool("_filled", false); Hearts[1].GetComponent<Animator>().SetBool("_filled", false); Hearts[2].GetComponent<Animator>().SetBool("_filled", false); }
        #endregion

        if (GameManager.Instance.saveCounter == 10 && !sonjoué   ) {

            jouerson();
        }
        if (GameManager.Instance.saveCounter == 50 && !sonjoué)
        {
            jouerson();
        }
        if (GameManager.Instance.saveCounter == 70 && !sonjoué)
        {
            jouerson();
        }

        if (GameManager.Instance.saveCounter != 10 && GameManager.Instance.saveCounter != 50 && GameManager.Instance.saveCounter != 70) sonjoué = false;
        }

    #region affichage en gros 
    public void onclickZoom(int perso)
    {

        if (charaAnim.GetCurrentAnimatorStateInfo(0).IsName("introchara" + perso))
        {
            charaAnim.SetFloat("Speed", -1.0f);
            charaAnim.Play("introchara" + perso, 0, 1.0f);
            charaAnim.SetBool("idle", true);
        }
        else
        {
            charaAnim.SetBool("idle", false);
            charaAnim.SetFloat("Speed", 1.0f);
            charaAnim.Play("introchara" + perso);
        }

    }
    #endregion

    void jouerson()
    {
        GameManager.Instance._SFX2.clip = GameManager.Instance.trackSFX[3];
        GameManager.Instance._SFX2.Play();
        sonjoué = true;
    }
}
