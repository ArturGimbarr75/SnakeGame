using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


using Text = Assets.Scripts.Settings.TextSource; // TODO: избавиться от всех
using Settings = Assets.Scripts.Settings.UISettings;
using Attribute = Assets.Scripts.Settings.UISettingsAttributes;
using Assets.Scripts.Settings;

public class SettingsMenuManager : MonoBehaviour
{
    /// <summary>
    /// Используется для присвоения элементам текста на необходимом языке
    /// </summary>
    static private List<string> ComponentsNames = new List<string> //TODO: Наверное больше не нужен
    {
        "Back",
        "Background2D",
        "Language",
        "Music",
        "Sound",
    };

    void Start()
    {
        Text.SetLanguage(ComponentsNames);
        ConfigSetingsInfo();
    }

    
    void Update()
    {
        
    }

    #region UIInfo

    public Toggle T_Sound;
    public Toggle T_Music;
    public Dropdown D_Background;
    public Dropdown D_Language;

    private void ConfigSetingsInfo()
    {
        T_Sound.isOn = Settings.Current.isSoundOn;
        T_Music.isOn = Settings.Current.isMusicOn;
        // TODO: все ниже в отдельный метод и подписаться на ивент
        D_Background.ClearOptions();
        D_Background.AddOptions(new List<string>(
            from el in (Attribute.Background2D[])Enum.GetValues(typeof(Attribute.Background2D))
            select el.ToString()));
        D_Background.value = (int)Settings.Current.background;
        D_Language.ClearOptions();
        D_Language.AddOptions(new List<string>( // TODO: Придумать как демонстрировать языки
            from el in (Attribute.Language[])Enum.GetValues(typeof(Attribute.Language))
            select TextSource.GetTextInSetLaunguage(el.ToString()))); // TODO: Добавить переводы
        D_Language.value = (int)Settings.Current.language;
    }

    #endregion

    #region Buttons

    public void Sound(bool isOn)
    {
        Settings.Current.isSoundOn = isOn;
        {// TODO: убрать | Используется для смены языков
            if ((int)UISettings.Current.language == 2) 
                UISettings.Current.language = 0;
            else
                UISettings.Current.language++;
        }// до сих
    }

    public void Music(bool isOn)
    {
        Settings.Current.isMusicOn = !Settings.Current.isMusicOn;
    }

    public void Background()
    {

    }

    public void Language()
    {
        Text.SetLanguage(ComponentsNames);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}
