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
        void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap);
    }
}
