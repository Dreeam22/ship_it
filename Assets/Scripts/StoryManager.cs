using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public TMP_Text storyText;
    List<Storydata> stories = new List<Storydata>();


    // Start is called before the first frame update
    void Start()
    {
        //read csv
        TextAsset storydata = Resources.Load<TextAsset>("Story_Manager");  //ouvre le csv

        string[] data = storydata.text.Split(new char[]{'\n'});  // lit les lignes séparées par retour à la ligne
        Debug.Log(data.Length);
        
        for (int i =1; i<data.Length-1;i++)
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
            }
        }

        foreach (Storydata sd in stories)
        {
            //Debug.Log(sd.ID + ";" + sd.Story);

            if (sd.Relationship_level == GameManager.Instance.relationLVL && sd.Chara1 == GameManager.Instance.chara1 && sd.Chara2 == GameManager.Instance.chara2)
            {

                Debug.Log(sd.ID);

                //afficher bravo vous avez débloqué blahblah
                //save la relation débloquée
                Storydata.ships.Add(sd.ID);
                SaveSystem.Save();

                //charger les bons sprites

                //afiche le texte
                storyText.text = sd.Story;

            }
            else if (sd.ID == GameManager.Instance.coupleID)
            {
                //charger les bons sprites

                //afiche le texte
                storyText.text = sd.Story;
            }

        }

        
    }

}
