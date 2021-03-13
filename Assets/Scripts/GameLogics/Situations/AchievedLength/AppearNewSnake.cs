using Map;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snake.SnakeAttribute;

namespace Situations
{
    class AppearNewSnake : BaseAchievedLength
    {        
        private List<string> Names;
        private List<PlayingMapAttributes.Snake> Snakes;

        public AppearNewSnake(int length, List<string> names) : base(length)
        {
            Names = names;
            Snakes = new List<PlayingMapAttributes.Snake>();
        }

        public override void OnAchievedLength(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl)
        {
            snake.SnakeB.Statistics.Length = snake.Cordinates.Count();
            List<Cordinates> cordinates = new List<Cordinates>();
            var count = snake.Cordinates.Count;
            for (int i = count - 1; i >= Length / 2; i--)
            {
                cordinates.Add(new Cordinates(snake.Cordinates[i]));
                snake.Cordinates.RemoveAt(i);
            }
            string name = Names[new Random().Next(0, Names.Count)];
            var newSnake = gl.AddSnake(name, cordinates);
            newSnake.Statistics.Length = newSnake.SnakeBody.Count;
            Snakes.Add(new PlayingMapAttributes.Snake(newSnake));    
        }

        public override void AddSnakes(PlayingMap currentMap)
        {
            foreach (var snake in Snakes)
                currentMap.Snake.Add(snake);
            Snakes.Clear();
        }
    }
}
