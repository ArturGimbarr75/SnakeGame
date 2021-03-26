using Logic;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    class None : BaseAchievedLength
    {
        public None(int length) : base(length) {}

        public override void AddSnakes(PlayingMap currentMap)
        {
            
        }

        public override void OnAchievedLength(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl)
        {
            
        }
    }
}
