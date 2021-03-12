using Logic;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    abstract class BaseDidStepsWithoutFood : IDidStepsWithoutFood
    {
        public int StepsWithoutFood { get; set; }
        protected Dictionary<int, int> DidStepsWithoutFoodById { get; set; }

        public BaseDidStepsWithoutFood(int stepsWithoutFood)
        {
            StepsWithoutFood = stepsWithoutFood;
            DidStepsWithoutFoodById = new Dictionary<int, int>();
        }

        protected abstract void OnStepsWithoutFoodDid(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl);

        public void OnStepDid(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl)
        {
            if (!DidStepsWithoutFoodById.ContainsKey(snake.SnakeB.ID))
                DidStepsWithoutFoodById.Add(snake.SnakeB.ID, 0);

            if (snake.FoundFoodAfterStep)
                DidStepsWithoutFoodById[snake.SnakeB.ID] = 0;
            else
                DidStepsWithoutFoodById[snake.SnakeB.ID]++;

            if (DidStepsWithoutFoodById[snake.SnakeB.ID] == StepsWithoutFood)
            {
                DidStepsWithoutFoodById[snake.SnakeB.ID] = 0;
                OnStepsWithoutFoodDid(snake, currentMap, previousMap, gl);
            }
        }
    }
}
