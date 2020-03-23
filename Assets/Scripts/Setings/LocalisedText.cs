using Assets.Scripts.DataBase;
using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Setings
{
    public class LocalisedText : MonoBehaviour
    {
        /// <summary>
        /// Текстовый компонент элемента UI
        /// Text component of UI element
        /// </summary>
        [SerializeField]
        private Text TextComponent;

        [SerializeField]
        private string Key;

        private static LanguageTable table;

        /// <summary>
        /// Вызывается при старте сцены и подписываемся на смену языка
        /// Is called when the scene starts subscribes for language change
        /// </summary>
        private void Awake()
        {
            if (table == null)
                table = new LanguageTable();

            UISettings.Current.LangugeChange += handleLanguageChange;
            string localisedString = table.GetElementText(Key, UISettings.Current.language);
            TextComponent.text = localisedString;
        }
        /// <summary>
        /// Оброботчик смены языка obrAbotchik
        /// Language change handler
        /// </summary>
        /// <param name="language">Выбранный язык/Chosen language</param> // не используется пока
        private void handleLanguageChange(UISettingsAttributes.Language language)
        {
            string localisedString = table.GetElementText(Key, UISettings.Current.language);
            TextComponent.text = localisedString;
        }
        /// <summary>
        /// Отписка
        /// Unsubscribe
        /// </summary>
        private void OnDestroy()
        {
            UISettings.Current.LangugeChange -= handleLanguageChange;
        }
    }
}
