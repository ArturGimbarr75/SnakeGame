﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// Класс для использования настроек
    /// </summary>
    public class UISettings
    {
        //public static readonly UISettingsClass Default = new UISettingsClass();
        public static UISettingsClass Current = new UISettingsClass(); // TODO: Сделать сохранения и чтение для этого объекта
    }

    /// <summary>
    /// Настройки пользовательского интерфейса
    /// </summary>
    public class UISettingsClass
    {
        private UISettingsAttributes.Language _language = UISettingsAttributes.Language.English;
        public UISettingsAttributes.Language language
        {
            get
            {
                return _language;
            }
            set
            {
                if (value == _language)
                    return;
                _language = value;
                LangugeChange?.Invoke(value);
            }
        }
        public UISettingsAttributes.Background2D background { get; set; }
        public bool isSoundOn { get; set; }
        public bool isMusicOn { get; set; }
        public Action<UISettingsAttributes.Language> LangugeChange;

        /// <summary>
        /// Устанавливаем базовые значения
        /// </summary>
        public UISettingsClass()
        {
            language = UISettingsAttributes.Language.English;
            background = UISettingsAttributes.Background2D.None;
            isSoundOn = true;
            isMusicOn = true;
        }
    }

    /// <summary>
    /// Класс хранящий структуры, перечесления
    /// необходимые для настроек
    /// </summary>
    public static class UISettingsAttributes
    {
        /// <summary>
        /// Возможные языки игры
        /// </summary>
        public enum Language
        {
            English,
            Lithuanian,
            Russian
        }

        /// <summary>
        /// Возможный фон игрового поля
        /// </summary>
        public enum Background2D
        {
            None,
            Chess,
            Line
        }
    }
}
