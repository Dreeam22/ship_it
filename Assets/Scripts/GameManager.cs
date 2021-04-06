using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Text _winTxt, _shipTxt, _unlokedTxt;
    public GameObject _winObj;

    public bool connected = false;

    public ParticleSystem finished;

    public int saveCounter;
    public int relationLVL = -1;

    public int chara1, chara2;

    public int coupleID;

    public bool fromMenuCouple = false;

    public GameObject caseActive;
    public bool caseTrigger = false;
    public bool continueDraw = false;

    public AudioSource _audio;
    public AudioClip[] a;

    public GameObject goCase;

    public List<GameObject> MouseEnterCases;

    List<Storydata> stories = new List<Storydata>();

    public Image[] i;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        SaveSystem.Load();
    }


    private void Start()
    {      
        _audio.clip = a[0];
        _audio.Play();

        //read csv
        TextAsset storydata = Resources.Load<TextAsset>("Story_Manager");  //ouvre le csv

        string[] data = storydata.text.Split(new char[] { '*' });  // lit les lignes séparées par l'étoile

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ';' });  //lit les colonnes séparées par ;

            if (row[1] != "")
            {
                Storydata sd = new Storydata();

                int.TryParse(row[0], out sd.ID);
                int.TryParse(row[1], out sd.Chara1);
                int.TryParse(row[2], out sd.Chara2);
                int.TryParse(row[3], out sd.Relationship_level);
                sd.Story = row[4];

                stories.Add(sd);
                //Storydata.ships.Add(sd.ID);
                //SaveSystem.Save();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (saveCounter >= 10) relationLVL = 0;
        if (saveCounter >= 50) relationLVL = 1;
        if (saveCounter >= 70) relationLVL = 2;

        if (saveCounter < 10) relationLVL = -1;

    }

    

    public void _connected(string p1, string p2)
    {
        connected = true;
        caseTrigger = false;
        //lancer FDBK
        finished = GameObject.Find("Finished").GetComponent<ParticleSystem>();
        finished.Play();
        //afficher texte
        _winObj.SetActive(true);
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en"))
            _winTxt.text = "Connected " + p1 + " with " + p2 ;
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("fr"))
            _winTxt.text = "Lié " + p1 + " avec " + p2;


        switch (p1)
        {
            case "Sasha":
                chara1 = 0;
                break;
            case "Charlie":
                chara1 = 1;
                break;
            case "Alex":
                chara1 = 2;
                break;
            case "Taylor":
                chara1 = 3;
                break;
        }

        switch(p2)
        {
            case "Sasha":
                chara2 = 0;
                break;
            case "Charlie":
                chara2 = 1;
                break;
            case "Alex":
                chara2 = 2;
                break;
            case "Taylor":
                chara2 = 3;
                break;
        }

        i[0] = GameObject.Find("Chara1").GetComponent<Image>();
        i[1] = GameObject.Find("Chara2").GetComponent<Image>();

        //checker si déjà lu
        foreach (Storydata sd in stories)
        {
            if (sd.Relationship_level == relationLVL && (sd.Chara1 == chara1 || sd.Chara1 == chara2) && (sd.Chara2 == chara2 || sd.Chara2 == chara1))
            {
                if (Storydata.ships.Contains(sd.ID))
                {
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en"))
                        _unlokedTxt.text = "";
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("fr"))
                        _unlokedTxt.text = "";



                }
                else
                {
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en"))
                        _unlokedTxt.text = "NEW !";
                    if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("fr"))
                        _unlokedTxt.text = "NOUVEAU !";
                }
            }

            //charger les bons sprites
            Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv1");
            i[0].sprite = s[chara1];
            i[1].sprite = s[chara2];
        }
    }


}

