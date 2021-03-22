using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{


    public TMP_Text storyText;
    List<Storydata> stories_trou = new List<Storydata>();
    List<Storydata> stories = new List<Storydata>();
    public RectTransform content;
    Button motButton;
    public Button buttonPrefab;

    public Image[] i;


    // Start is called before the first frame update
    void Start()
    {

        //read csv
        TextAsset storydata_trou = Resources.Load<TextAsset>("Story_Manager-trous");  //ouvre le csv

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

        foreach (Storydata sd in stories_trou)
        {

            if (sd.Relationship_level == GameManager.Instance.relationLVL && (sd.Chara1 == GameManager.Instance.chara1 || sd.Chara1 == GameManager.Instance.chara2) && (sd.Chara2 == GameManager.Instance.chara2 || sd.Chara2 == GameManager.Instance.chara1) && !GameManager.Instance.fromMenuCouple)
            {

                 //afficher bravo vous avez débloqué blahblah

                //save la relation débloquée
                Storydata.ships.Add(sd.ID);
                SaveSystem.Save();

                //charger les bons sprites
                Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv1");
                i[0].sprite = s[sd.Chara1];
                i[1].sprite = s[sd.Chara2];    

                //afiche le texte
                storyText.text = sd.Story;

                //agrandir la scroll view en fonction de la longueur du txt
                float size = storyText.rectTransform.rect.height;
                content.rect.Set(0, 0, 0, size);

                //Instancier les bons bouttons selon le couple
                switch(sd.ID)
                {
                    case 0:
                        for (int i = 1; i < 3; i++)
                        { 
                            motButton = Instantiate(buttonPrefab, new Vector3(100+(i*100),600-(i*100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "crush";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "oblivious";

                        }
                        break;

                    case 1:
                        for (int i =1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "clingy";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "hugging";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "affection";
                        }
                        break;

                    case 2:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "liked";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "panic";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "rush of heat";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "down to earth";
                        }
                        break;

                    case 3:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "nonsense";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "best friends";

                        }
                        break;
                    case 4:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "together";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "crashed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "definition";

                        }
                        break;
                    case 5:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "romantically and sexually";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "noticed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "investigating";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "Stupid couple";

                        }
                        break;

                    case 6:
                        for (int i = 1; i < 3; i++)
                        {
                           motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "book";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "entertain";

                        }
                        break;
                    case 7:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "smile";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "lunch break";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "favor";

                        }
                        break;
                    case 8:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "books and movies";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "show up";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "topic";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "happiness";

                        }
                        break;
                    case 9:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "idiot";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "comfort zone";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "bullshit";


                        }
                        break; 
                    case 10:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "twice";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "beaten";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "missed";


                        }
                        break;
                    case 11:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "cocky";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "tree";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "love";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "arms";
                            
                        }
                        break;
                    case 12:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "vulnerable";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "impress";


                        }
                        break;
                    case 13:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "accomplishment";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "feelings";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "serious";


                        }
                        break;
                    case 14:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "control";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "glanced";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "trust";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "obvious";

                        }
                        break;
                    case 15:
                        for (int i = 1; i < 3; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "rivals";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "throwing shades";
                        }
                        break;
                    case 16:
                        for (int i = 1; i < 4; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "steps up";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "panics";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "grumpy";
                        }
                        break;
                    case 17:
                        for (int i = 1; i < 5; i++)
                        {
                            motButton = Instantiate(buttonPrefab, new Vector3(100 + (i * 100), 600 - (i * 100)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            motButton.name = i.ToString();

                            if (i == 1) motButton.GetComponentInChildren<TMP_Text>().text = "distant";
                            if (i == 2) motButton.GetComponentInChildren<TMP_Text>().text = "stressed";
                            if (i == 3) motButton.GetComponentInChildren<TMP_Text>().text = "all night";
                            if (i == 4) motButton.GetComponentInChildren<TMP_Text>().text = "happy";
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
                Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv1");
                i[0].sprite = s[sd.Chara1];
                i[1].sprite = s[sd.Chara2];

                //afiche le texte
                storyText.text = sd.Story;

                //agrandir la scroll view en fonction de la longueur du txt
                float size = storyText.rectTransform.rect.height;
                content.rect.Set(0, 0, 0, size);
            }
        }
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

        Destroy(button);

    }

}
