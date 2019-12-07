using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    class GameSettingsManager : MonoBehaviour
    {
        public List<Dropdown> SnakeTypesDropdowns;
        public Toggle LeftDeadBody;
        public Dropdown GameMode;
        public InputField FoodCount;
        public InputField MapSize;
        public Toggle EndGameCheckboxes;

        private void Start()
        {
            
        }
    }
}
