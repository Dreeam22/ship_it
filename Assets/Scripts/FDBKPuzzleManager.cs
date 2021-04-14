﻿using System.Collections;
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

    public List<Image> BG_desc;
    public List<Image> Hearts;

    bool[] sonjoué;

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

        sonjoué = new bool[3];
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
                        for (int i = 0; i < 4; i++)
                        {
                            _tab[i].gameObject.transform.SetParent(GameObject.Find("Persos").transform);
                        }
                        _img.SetActive(false);
                        break;

                }
                if (a > 5)
                {
                    charaAnim.SetBool("IntroBool", false);
                    Storydata.prez = true;

                }

            }
        }
        #endregion

        #region fdbk niveau relation validé

        if (GameManager.Instance.relationLVL == 0)   Hearts[0].GetComponent<Animator>().SetBool("_filled", true);
        else if (GameManager.Instance.relationLVL == 1) Hearts[1].GetComponent<Animator>().SetBool("_filled", true);
        else if (GameManager.Instance.relationLVL == 2) Hearts[2].GetComponent<Animator>().SetBool("_filled", true);
        else { Hearts[0].GetComponent<Animator>().SetBool("_filled", false); Hearts[1].GetComponent<Animator>().SetBool("_filled", false); Hearts[2].GetComponent<Animator>().SetBool("_filled", false); }
            #endregion
        }

    #region affichage en gros 
    public void onclickZoom(int perso)
    {

        if (charaAnim.GetCurrentAnimatorStateInfo(0).IsName("introchara" + perso))
        {
            charaAnim.SetFloat("Speed", -1.0f);
            charaAnim.Play("introchara" + perso, 0, 1.0f);
            BG_desc[perso - 1].gameObject.SetActive(false);
            charaAnim.SetBool("idle", true);
        }
        else
        {
            charaAnim.SetBool("idle", false);
            charaAnim.SetFloat("Speed", 1.0f);
            charaAnim.Play("introchara" + perso);
            BG_desc[perso - 1].gameObject.SetActive(true);
        }

    }
    #endregion

    void jouerson(int b)
    {
        GameManager.Instance._SFX2.clip = GameManager.Instance.trackSFX[3];
        GameManager.Instance._SFX2.Play();
        sonjoué[b] = true;
    }
}