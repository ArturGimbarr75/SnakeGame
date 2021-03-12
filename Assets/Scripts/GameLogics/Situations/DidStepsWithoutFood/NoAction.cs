using Logic;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    class NoAction : BaseDidStepsWithoutFood
    {
        public NoAction(int stepsWithoutFood = 0) : base(stepsWithoutFood) { }

        protected override void OnStepsWithoutFoodDid(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl)
        {
            
        }
    }
}
