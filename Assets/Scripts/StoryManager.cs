﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public TMP_Text storyText;
    List<Storydata> stories = new List<Storydata>();
    public RectTransform content;
    int chara1temp;
    int chara2temp;
    public Image[] i;

    // Start is called before the first frame update
    void Start()
    {
        //read csv
        TextAsset storydata = Resources.Load<TextAsset>("Story_Manager");  //ouvre le csv

        string[] data = storydata.text.Split(new char[]{'*'});  // lit les lignes séparées par l'étoile
        Debug.Log(data.Length);
        
        for (int i = 1; i<data.Length-1;i++)
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

            Debug.Log("relation" + GameManager.Instance.relationLVL);
            Debug.Log(GameManager.Instance.chara1 + "," + GameManager.Instance.chara2);
            Debug.Log(GameManager.Instance.fromMenuCouple);
        }

        foreach (Storydata sd in stories)
        {
            //Debug.Log(sd.ID + ";" + sd.Story);

            if (sd.Relationship_level == GameManager.Instance.relationLVL && (sd.Chara1 == GameManager.Instance.chara1 || sd.Chara1 == GameManager.Instance.chara2) && (sd.Chara2 == GameManager.Instance.chara2 || sd.Chara2 == GameManager.Instance.chara1) && !GameManager.Instance.fromMenuCouple)
            {

                //Debug.Log(sd.ID);

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
                

            }
            else if (sd.ID == GameManager.Instance.coupleID && GameManager.Instance.fromMenuCouple)
            {
                //charger les bons sprites
                Sprite[] s = Resources.LoadAll<Sprite>("Sprites/posingv1");
                i[0].sprite = s[sd.Chara1];
                i[1].sprite = s[sd.Chara2];
                //afiche le texte
                storyText.text = sd.Story;
            }
          

        }

        
    }

}
