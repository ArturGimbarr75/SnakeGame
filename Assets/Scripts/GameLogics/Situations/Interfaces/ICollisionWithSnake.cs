using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    public interface ICollisionWithSnake
    {
        void OnColision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy, PlayingMap map);
    }
}
