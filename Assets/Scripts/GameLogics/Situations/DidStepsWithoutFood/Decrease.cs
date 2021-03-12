using Logic;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    class Decrease : BaseDidStepsWithoutFood
    {
        public Decrease(int stepsWithoutFood) : base(stepsWithoutFood) { }

        protected override void OnStepsWithoutFoodDid(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl)
        {
            snake.Cordinates.RemoveAt(snake.Cordinates.Count - 1);
            if (snake.Cordinates.Count < 3)
                snake.IsAlive = false;
        }
    }
}
