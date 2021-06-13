using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class StoryManager : MonoBehaviour
{

    public TMP_Text storyText;
    public GameObject CongratsTxt;
    List<Storydata> stories_trou = new List<Storydata>();
    List<Storydata> stories = new List<Storydata>();
    public RectTransform content;
    Button motButton;
    public Button buttonPrefab;
    public GameObject poti1, poti2;

    public Image Illu;

    public Image[] i;

    TextAsset storydata_trou, storydata;

    public Button buttonNext;
    public bool next = false;
    public List<Button> buttonTrou = new List<Button>();

    public Scrollbar _scrollbar;

    public ParticleSystem heart;
    public ParticleSystem nextpart;

    Vector3 buttonVec = new Vector3(10, 10, 0);


    // Start is called before the first frame update
    void Start()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("fr"))
        {
            storydata_trou = Resources.Load<TextAsset>("Story_Manager_frenche - trou");  //ouvre le csv
            storydata = Resources.Load<TextAsset>("Story_Manager_frenche");
        }
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en"))
        {
            storydata_trou = Resources.Load<TextAsset>("Story_Manager-trous");  //ouvre le csv
            storydata = Resources.Load<TextAsset>("Story_Manager");  //ouvre le csv
        }

        //read csv
        

        string[] data_trou = storydata_trou.text.Split(new char[]{'*'});  // lit les lignes séparées par l'étoile
        
        for (int i = 1; i<data_trou.Length-1;i++)
        {
            string[] row = data_trou[i].Split(new char[] { ';' });  //lit les colonnes séparées par ;

            if (row[1] != "")
            {
                Storydata sd_trou = new Storydata();

                int.TryParse(row[0], out sd_trou.ID);
                int.TryParse(row[1], out sd_trou.Chara1);
                int.TryParse(row[2], out sd_trou.Chara2);
                int.TryParse(row[3], out sd_trou.Relationship_level);
                sd_trou.Story = row[4];

                stories_trou.Add(sd_trou);
                //Storydata.ships.Add(sd.ID);
                //SaveSystem.Save();
            }
        }

        
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

        foreach (Storydata sd in stories_trou)
        {

            if (sd.Relationship_level == GameManager.Instance.relationLVL && (sd.Chara1 == GameManager.Instance.chara1 || sd.Chara1 == GameManager.Instance.chara2) && (sd.Chara2 == GameManager.Instance.chara2 || sd.Chara2 == GameManager.Instance.chara1) && !GameManager.Instance.fromMenuCouple)
            {

                //save la relation débloquée
                Storydata.ships.Add(sd.ID);




                if (sd.Chara1 == 0 && sd.Chara2 == 1 || sd.Chara1 == 1 && sd.Chara2 == 0)
                {
                    Illu.sprite = Resources.Load<Sprite>("Sprites/illuSasharlie");
                    //charger les bons sprites
                    i[0].gameObject.SetActive(false);
                    i[1].gameObject.SetActive(false);
                }
                else if (sd.Chara1 == 0 && sd.Chara2 == 2 || sd.Chara1 == 2 && sd.Chara2 == 0)
                {
                    Illu.sprite = Resources.Load<Sprite>("Sprites/sashalex 1");
                    i[0].gameObject.SetActive(false);
                    i[1].gameObject.SetActive(false);
                }
                else
                {
                    Illu.sprite = Resources.Load<Sprite>("Sprites/fdneutre");
                    //charger les bons sprites
                    Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv2");
                    i[0].sprite = s[sd.Chara1];
                    i[1].sprite = s[sd.Chara2];
                }

                poti1.GetComponent<Animator>().Play((sd.Chara1.ToString() +"_"+ sd.Relationship_level.ToString()));
                poti2.GetComponent<Animator>().Play((sd.Chara2.ToString() + "_" + sd.Relationship_level.ToString()));

                //afiche le texte
                storyText.text = sd.Story;

                //Instancier les bons bouttons selon le couple
                switch(sd.ID)
                {
                    case 0:
                        for (int i = 1; i < 3; i++)
                        { 
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "crush";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "oblivious";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;

                    case 1:
                        for (int i =1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "clingy";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "hugging";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "affection";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;

                    case 2:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "liked";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "panic";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "rush of heat";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "down to earth";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;

                    case 3:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "nonsense";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "best friends";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 4:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "together";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "crashed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "definition";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 5:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "romantically and sexually";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "noticed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "investigating";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "Stupid couple";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;

                    case 6:
                        for (int i = 1; i < 3; i++)
                        {
                           motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "book";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "entertain";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 7:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "smile";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "lunch break";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "favor";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                    case 8:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "books and movies";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "show up";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "topic";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "happiness";

                            motButton.animator.Play("ButtonAnim" + (i-1));

                        }
                        break;
                    case 9:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "idiot";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "comfort zone";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "bullshit";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break; 
                    case 10:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "twice";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "beaten";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "missed";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                    case 11:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "cocky";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "tree";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "love";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "arms";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 12:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "vulnerable";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "impress";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 13:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "accomplishment";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "feelings";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "serious";

                            motButton.animator.Play("ButtonAnim" + (i - 1));

                        }
                        break;
                    case 14:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "control";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "glanced";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "trust";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "obvious";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                    case 15:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "rivals";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "throwing shades";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                    case 16:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "steps up";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "panics";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "grumpy";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                    case 17:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, buttonVec, Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();
                            buttonTrou.Add(motButton);

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "distant";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "stressed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "all night";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "happy";

                            motButton.animator.Play("ButtonAnim" + (i - 1));
                        }
                        break;
                }

            }

        }

        foreach (Storydata sd in stories)
        {
            if (sd.ID == GameManager.Instance.coupleID && GameManager.Instance.fromMenuCouple)
            {
                //charger les bons sprites
                Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv2");
                i[0].sprite = s[sd.Chara1];
                i[1].sprite = s[sd.Chara2];

                //afiche le texte
                storyText.text = sd.Story;


            }
        }

        
    }

    void Update()
    {

        for (int i = 0; i < buttonTrou.Count; i++)
        {
            if (buttonTrou[i] == null)
            {
                buttonTrou.Remove(buttonTrou[i]);
            }
        }

        if (buttonTrou.Count == 0)
        {
            next = true;
        }
        else
        {
            next = false;
        }

        if (next ) StartCoroutine("UnlockCouple");
    }
    public void OnClickMots(GameObject button)
    {
        //repérer la zone vide
        //si vide
        //remplir du nom de l'objet
        string num = button.name;
        string replaceText = button.GetComponentInChildren<TMP_Text>().text;

        StoryManager strm = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        string corrstring = strm.storyText.text.Replace(num,replaceText);

        GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.text = corrstring;

        GameObject.Find("StoryManager").GetComponent<StoryManager>().heart.Play();

        wordposition(replaceText);
        Destroy(button);

    }

    public void wordposition(string mot)
    {
        //recupérer index of le mot
        int index = GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.text.IndexOf(mot);
        Debug.Log(index);
        int totalmots = GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.text.Length;
        //récupérer la taille du scroll rect
        float size = GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.rectTransform.rect.height;
        float widght = GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.rectTransform.rect.width;

        //déduire la position du mot dans le scroll rect
        //trouver sa ligne avec la height
        float nbRetour= GameObject.Find("StoryManager").GetComponent<StoryManager>().storyText.text.Split(new char[] { '\n' }).Length;
        float nbligne = ((totalmots / 50)+(nbRetour/2));
        Debug.Log(nbligne);

        //Si index of le mot > 1000
        //Lancer le fbk
        GameObject p = GameObject.Find("Pointeur");
        if (index > 1000)
        {
            GameObject.Find("StoryManager").GetComponent<StoryManager>()._scrollbar.value = 0;
        }
        else
            GameObject.Find("StoryManager").GetComponent<StoryManager>()._scrollbar.value = 1;

    }


    IEnumerator UnlockCouple()
    {
        //WP you unlocked ...
        CongratsTxt.SetActive(true);    
        SaveSystem.Save();
        
        yield return new WaitForSecondsRealtime(0.3f);
        GameObject.Find("StoryManager").GetComponent<StoryManager>().nextpart.Play();
        buttonNext.interactable = true;
        
        //save
        
    }
}
