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
        /// </summary>
        [SerializeField]
        private Text TextComponent;

        [SerializeField]
        private string Key;

        private static LanguageTable table;

        /// <summary>
        /// Вызывается при старте сцены и подписываемся на смену языка
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
        /// Оброботчик смены языка
        /// </summary>
        /// <param name="language">Выбранный язык</param> // не используется пока
        private void handleLanguageChange(UISettingsAttributes.Language language)
        {
            string localisedString = table.GetElementText(Key, UISettings.Current.language);
            TextComponent.text = localisedString;
        }
        /// <summary>
        /// Отписка
        /// </summary>
        private void OnDestroy()
        {
            UISettings.Current.LangugeChange -= handleLanguageChange;
        }
    }
}
