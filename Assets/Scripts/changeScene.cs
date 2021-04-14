using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class changeScene : MonoBehaviour
{

    public GameObject pauseObj;
    public Dropdown dropdown;

    void Start()
    {
      
        if (SceneManager.GetActiveScene().name == "Menu_Principal") StartCoroutine(Start_Locale());

    }

    public void LoadSceneCouple(int coupleID)
    {
        //regarder sur quel couple on clique   
        
        GameManager.Instance.coupleID = coupleID;
        GameManager.Instance.fromMenuCouple = true;
        SceneManager.LoadScene(3);  
    }

    public void onclickLoad(int x)
    {
        switch (x)
        {
            case 1:
                GameManager.Instance._audio.clip = GameManager.Instance.a[1];
                GameManager.Instance._audio.Play();
                GameManager.Instance.relationLVL = -1;
                Destroy(this.gameObject);
               break;

            case 3:
                GameManager.Instance.fromMenuCouple = false;
                GameManager.Instance._audio.clip = GameManager.Instance.a[2];
                GameManager.Instance._audio.Play();
               break;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(x);

    }

    public void onCickPause(int i)
    {
        switch (i)
        {
            case 0:
                Time.timeScale = 0;
                pauseObj.SetActive(true);
                return;
            case 1:
                Time.timeScale = 1;
                pauseObj.SetActive(false);
                return;
        }
         
    }

    public void quit()
    {
        Application.Quit();
    }

    IEnumerator Start_Locale()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new Dropdown.OptionData(locale.name));
        }
        dropdown.options = options;

        dropdown.value = selected;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public void onClickSFX()
    {
        GameManager.Instance._SFX.clip = GameManager.Instance.trackSFX[0];
        GameManager.Instance._SFX.Play();
    }
}
