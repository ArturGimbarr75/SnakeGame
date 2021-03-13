using Map;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    abstract class BaseAchievedLength : IAchievedLength
    {
        public int Length { get => _length; }
        private int _length;
        private const int MIN_LENGTH_VALUE = 6;

        public BaseAchievedLength(int length)
        {
            _length = length < MIN_LENGTH_VALUE ? MIN_LENGTH_VALUE : length;
        }

        public abstract void OnAchievedLength(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl);

        public abstract void AddSnakes(PlayingMap currentMap);
    }
}
