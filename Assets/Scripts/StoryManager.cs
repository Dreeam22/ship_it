using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public TMP_Text storyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.relation)
        {
            case 0:
                break;
            case 1:
                storyText.text = System.IO.File.ReadAllText(@"F:\Cours\_Project\ship_it\Assets\Resources\AmiC1.txt");
                break;
            case 2:
                storyText.text = System.IO.File.ReadAllText(@"F:\Cours\_Project\ship_it\Assets\Resources\AmiC1.txt");
                break;

            case 3:
                storyText.text = System.IO.File.ReadAllText(@"F:\Cours\_Project\ship_it\Assets\Resources\AmiC1.txt");
                break;
        }
    }
}
