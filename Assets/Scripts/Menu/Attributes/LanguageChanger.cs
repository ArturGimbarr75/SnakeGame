using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.Menu.Attributes
{
    class LanguageChanger : MonoBehaviour
    {
        public void ChangeLanguage()
        {
            UISettings.Current.language++;

            if ((int)UISettings.Current.language == 3)
                UISettings.Current.language = 0;
        }
    }
}
