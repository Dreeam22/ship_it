using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{

    public void LoadScene(int level)
    {
        
        SceneManager.LoadScene(level);

    }

    public void LoadSceneCouple(int coupleID)
    {
        //regarder sur quel couple on clique      
        GameManager.Instance.coupleID = coupleID;
        SceneManager.LoadScene(3);  
    }

    public void onclickLoad(int x)
    {
        switch (x)
        {
            case 0:
                Destroy(this.gameObject);
                SceneManager.LoadScene("Proto_Scene");
                break;
            case 1:
                SceneManager.LoadScene("Story_Scene");
                break;

            case 2:
                SceneManager.LoadScene("Menu_Principal");
                break;
        }
    }
}
