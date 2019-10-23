using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// Класс позволяющий получить текст элементов
    /// интерфейса на необходимом языке
    /// </summary>
    static class TextSource /*LangugeManager*/  // TODO: Подписать на 
    {
        #region public

        /// <summary>
        /// Устанавливает текст на необходимом языке UI элементам
        /// </summary>
        /// <param name="ComponentsNames">Список с названиями элементов</param>
        public static void SetLanguage (List<string> ComponentsNames)
        {
            foreach (string name in ComponentsNames)
            {
                var a = GameObject.Find(name);
                if (a != null)
                    a.GetComponentInChildren<Text>().text = GetTextInSetLaunguage(name);
                else
                    Debug.Assert(false, "Element by name: '" + name + "' not exists");
            }
        }

        #endregion

        #region private

        /// <summary>
        /// Получаем текст из необходимого словаря
        /// </summary>
        /// <param name="elName">Название элемента</param>
        /// <returns>Текст на определенном языке или название элемента</returns> 
        public static string GetTextInSetLaunguage (string elName)
        {
            string textTarg;
            switch (UISettings.Current.language)
            {
                case UISettingsAttributes.Language.English:
                    textTarg = English.ContainsKey(elName)? English[elName] : String.Empty;
                    break;

                case UISettingsAttributes.Language.Lithuanian:
                    textTarg = Lithuanian.ContainsKey(elName) ? Lithuanian[elName] : String.Empty;
                    break;

                case UISettingsAttributes.Language.Russian:
                    textTarg = Russian.ContainsKey(elName) ? Russian[elName] : String.Empty;
                    break;

                default:
                    textTarg = English.ContainsKey(elName) ? English[elName] : String.Empty;
                    Debug.Assert(false, "Unknown Language");
                    break;
            }

            Debug.Assert (!String.IsNullOrEmpty(textTarg), "Can't find translation of:\t" + elName);
            return !String.IsNullOrEmpty(textTarg) ? textTarg : "Error!!! " + elName;
        }

        // TODO: Перенести все в базу данных
        #region Text

        private static Dictionary<string, string> English = new Dictionary<string, string>
        {
            /// Название
            ["English"] = "English",
            ["Lithuanian"] = "Lietuviu",
            ["Russian"] = "Русский",

            /// Часть MainMenu
            ["NewGame"] = "New Game",
            ["Settings"] = "Settings",
            ["Quit"] = "Quit",

            /// Часть SettingsMenu
            ["Back"] = "Back",
            ["Background2D"] = "Background",
            ["Language"] = "Language",
            ["Music"] = "Music",
            ["Sound"] = "Sound",
        };


        private static Dictionary<string, string> Lithuanian = new Dictionary<string, string>
        {
            /// Название
            ["English"] = "English",
            ["Lithuanian"] = "Lietuviu",
            ["Russian"] = "Русский",

            /// Часть MainMenu
            ["NewGame"] = "New Game LT",
            ["Settings"] = "Settings LT",
            ["Quit"] = "Quit LT",

            /// Часть SettingsMenu
            ["Back"] = "Back LT",
            ["Background2D"] = "Background LT",
            ["Language"] = "Language LT",
            ["Music"] = "Music LT",
            ["Sound"] = "Sound LT",
        };

        private static Dictionary<string, string> Russian = new Dictionary<string, string>
        {
            /// Название
            ["English"] = "English",
            ["Lithuanian"] = "Lietuviu",
            ["Russian"] = "Русский",

            /// Часть MainMenu
            ["NewGame"] = "Новая игра",
            ["Settings"] = "Настройки",
            ["Quit"] = "Выход",

            /// Часть SettingsMenu
            ["Back"] = "Назад",
            ["Background2D"] = "Фон",
            ["Language"] = "Язык",
            ["Music"] = "Музыка",
            ["Sound"] = "Звуки",
        };

        #endregion

        #endregion
    }
}
