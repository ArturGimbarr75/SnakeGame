using Logic;
using Map;
using System.Collections.Generic;

namespace Situations
{
    public interface IDidStepsWithoutFood
    {
        public int StepsWithoutFood { get; set; }
        void OnStepDid(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl);
    }
}
