using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class levelLoader : MonoBehaviour
{
    public Animator transitions;
    public float transitionTime = 1f;

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


    public void onClickPause(int i)
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

    public void LoadNextLevel(int scene)
    {
        StartCoroutine(LoadLevel(scene));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transitions.SetTrigger("Start");
        if (levelIndex == 1)
        {
            GameManager.Instance._audio.clip = GameManager.Instance.a[1];
            GameManager.Instance._audio.Play();
        }       

        yield return new WaitForSecondsRealtime(transitionTime);     

        switch (levelIndex)
        {
            case 0:
                GameManager.Instance._audio.clip = GameManager.Instance.a[0];
                GameManager.Instance._audio.Play();
                break;

            case 1:
                GameManager.Instance._audio.clip = GameManager.Instance.a[2];
                GameManager.Instance._audio.Play();
                GameManager.Instance.relationLVL = -1;
                break;

            case 2:
                GameManager.Instance.fromMenuCouple = false;
                if (GameManager.Instance.relationLVL == 0) GameManager.Instance._audio.clip = GameManager.Instance.a[3]; 
                if (GameManager.Instance.relationLVL == 1) GameManager.Instance._audio.clip = GameManager.Instance.a[4]; 
                if (GameManager.Instance.relationLVL == 2) GameManager.Instance._audio.clip = GameManager.Instance.a[5]; 

                GameManager.Instance._audio.Play();
                break;
        }
        Time.timeScale = 1;

        SceneManager.LoadScene(levelIndex);
    }
}
