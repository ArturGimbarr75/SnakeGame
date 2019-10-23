using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Snake;

namespace Assets.Scripts.GameLogics
{
    public interface ISnakeFactory
    {
        SnakeBase GetSnakeByName(string snakeName, List<SnakeAttribute.Cordinates> cordinates);
    }
}
