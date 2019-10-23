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

        /// <summary>
        /// Вызывается при старте сцены и подписываемся на смену языка
        /// </summary>
        private void Awake()
        {
            UISettings.Current.LangugeChange += handleLanguageChange;
            string localisedString = TextSource.GetTextInSetLaunguage(Key);
            TextComponent.text = localisedString;
        }
        /// <summary>
        /// Оброботчик смены языка
        /// </summary>
        /// <param name="language">Выбранный язык</param> // не используется пока
        private void handleLanguageChange(UISettingsAttributes.Language language)
        {
            string localisedString = TextSource.GetTextInSetLaunguage(Key);
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
