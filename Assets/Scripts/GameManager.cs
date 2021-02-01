using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text _winTxt, _shipTxt;
    public GameObject _winObj;

    public bool connected = false;

    public ParticleSystem finished;

    public int saveCounter;
    public int relationLVL = 0;
    public int chara1, chara2;

    public int coupleID;

    public bool fromMenuCouple = false;

    public Vector2 casePos;
    public bool caseTrigger = false;
    public bool continueDraw = false;

    public AudioSource _audio;
    public AudioClip[] a;

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
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(connected);
        //Game states
        switch (saveCounter)
        {
            case 10:
                relationLVL = 0;             
                break;
            case 50:
                relationLVL = 1;
                break;
            case 70:
                relationLVL = 2;
                break;

        }
       
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
        _winTxt.text = "Connected " + p1 + " with " + p2 ;
        //Save nb cases -> fait dans countcases

        switch(p1)
        {
            case "p1":
                chara1 = 0;
                break;
            case "p2":
                chara1 = 1;
                break;
            case "p3":
                chara1 = 2;
                break;
            case "p4":
                chara1 = 3;
                break;
        }

        switch(p2)
        {
            case "p1":
                chara2 = 0;
                break;
            case "p2":
                chara2 = 1;
                break;
            case "p3":
                chara2 = 2;
                break;
            case "p4":
                chara2 = 3;
                break;
        }


    }   

    
}
