using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

using Text = Assets.Scripts.Settings.TextSource;
using Assets.Scripts.Settings;


public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        UISettings.Current.LangugeChange += handleLanguageChange;
    }

    private void handleLanguageChange (UISettingsAttributes.Language language)
    {
        Debug.Log("Languge change to: " + language.ToString());
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UISettings.Current.LangugeChange -= handleLanguageChange;
    }

    #region Buttons
    /// <summary>
    /// Переход к сцене с выбором режимов игры
    /// </summary>
    public void NewGame()
    {
        { // TODO: убрать | Используется для смены языков
          /*if ((int)UISettings.Current.language == 2)
              UISettings.Current.language = 0;
          else
              UISettings.Current.language++;*/
            SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// Переход к сцене со статистикой
    /// </summary>
    public void SnakeStatistics()
    {
        SceneManager.LoadScene(5);
    }

    /// <summary>
    /// Переход к сцене со статистикой
    /// </summary>
    public void PlayersStatistics()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
