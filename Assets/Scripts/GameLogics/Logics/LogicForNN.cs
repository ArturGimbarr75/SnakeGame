using Assets.Scripts.GameLogics;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using static Snake.SnakeAttribute;
using System.IO;
using UnityEngine;

namespace Logic
{
    class LogicForNN : CustomisingLogic
    {
        public LogicForNN() : base
        (
            new HashSet<GameLogicsAttributes.GameoverPredicates>()
            {
                GameLogicsAttributes.GameoverPredicates.DeadAllSnakes
            },
            new List<string>()
            {
                nameof(FollowFoodAdam),
                nameof(FollowFoodAdam),
                /*nameof(FollowFoodAdam),
                nameof(FollowFoodAdam),
                nameof(FollowFoodAdam),
                nameof(FollowFoodAdam),
                nameof(FollowFoodAdam),
                nameof(FollowFoodAdam)*/
            },
            new AssemblySnakeFactory(),
            50,
            20,
            true
        ){ }

        public override PlayingMap GetNextPlayingMap()
        {
            var map = base.GetNextPlayingMap();

            List<MapForNN> mapNN = new List<MapForNN>();
            foreach (var s in map.Snake)
            {
                var info = GetMapFromPoint(s.Cordinates[0], 11, map);
                mapNN.Add(info);
            }
            WriteToFile(mapNN);

            return map;
        }

        private static void WriteToFile(List<MapForNN> mapNN)
        {
            string data = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(Application.dataPath, "Resources", "TrainData", "Data.json")))
            {
                var json = reader.ReadToEnd();
                var list = JsonConvert.DeserializeObject<List<MapForNN>>(json);
                mapNN.ForEach(x =>list.Add(x));
                data = JsonConvert.SerializeObject(list);
                reader.Close();
            }

            File.WriteAllText(Path.Combine(Application.dataPath, "Resources", "TrainData", "Data.json"), string.Empty);

            using (StreamWriter writer = new StreamWriter(Path.Combine(Application.dataPath, "Resources", "TrainData", "Data.json")))
            {       
                writer.WriteLine(data);
                writer.Close();
            }
        }

        public static MapForNN GetMapFromPoint(Cordinates point, int size, PlayingMap playingMap)
        {
            int xPadding = point.X - size / 2;
            int yPadding = point.Y - size / 2;

            List<MapCoordConverter> coordList = new List<MapCoordConverter>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    int x = xPadding + i;
                    int y = yPadding + j;

                    if (x < 0)
                        x += playingMap.sideSize;
                    if (y < 0)
                        y += playingMap.sideSize;

                    if (x >= playingMap.sideSize)
                        x -= playingMap.sideSize;
                    if (x >= playingMap.sideSize)
                        x -= playingMap.sideSize;

                    coordList.Add(new MapCoordConverter()
                    {
                        PMapCoord = new Cordinates(x, y),
                        NNMapCoord = new Cordinates(i, j)
                    });
                }

            return CreateMap(size, coordList, playingMap, point);
        }

        private static MapForNN CreateMap(int size, List<MapCoordConverter> coordList, PlayingMap pMap, Cordinates point)
        {
            var map = new MapForNN(size);

            foreach (var c in pMap.Barriers)
            {
                var coord = coordList.Where(x => x.PMapCoord == c).ToArray();
                if (coord.Length > 0)
                {
                    map.Map[coord[0].NNMapCoord.X, coord[0].NNMapCoord.Y] = ObjectTypes.Barrier;
                }
            }
            foreach (var c in pMap.Food.FoodCordinates)
            {
                var coord = coordList.Where(x => x.PMapCoord == c).ToArray();
                if (coord.Length > 0)
                {
                    map.Map[coord[0].NNMapCoord.X, coord[0].NNMapCoord.Y] = ObjectTypes.Food;
                }
            }
            foreach (var snake in pMap.Snake)
                foreach (var c in snake.Cordinates)
                {
                    var coord = coordList.Where(x => x.PMapCoord == c).ToArray();
                    if (coord.Length > 0)
                    {
                        map.Map[coord[0].NNMapCoord.X, coord[0].NNMapCoord.Y] = ObjectTypes.Snake;
                    }
                }

            FindNearestFood(ref map, pMap, point);
            return map;
        }

        private static void FindNearestFood(ref MapForNN map, PlayingMap pMap, Cordinates point)
        {
            Cordinates nearFood = pMap.Food.FoodCordinates[0];
            double minDist = Math.Sqrt(
                    Math.Pow(point.X - (pMap.Food.FoodCordinates[0].X), 2)
                    + Math.Pow(point.Y - (pMap.Food.FoodCordinates[0].Y), 2)); ;

            for (int xFactor = -1; xFactor <= 1; xFactor++)
                for (int yFactor = -1; yFactor <= 1; yFactor++)
                    CheckOtherSide(pMap, xFactor, yFactor, point, ref minDist, ref nearFood);

            if (point.X < nearFood.X)
                map.NearestFoodHorizontal = SnakePathway.Right;
            else if (point.X > nearFood.X)
                map.NearestFoodHorizontal = SnakePathway.Left;
            else
                map.NearestFoodHorizontal = null;

            if (point.Y < nearFood.Y)
                map.NearestFoodVertical = SnakePathway.Down;
            else if (point.Y > nearFood.Y)
                map.NearestFoodVertical = SnakePathway.Up;
            else
                map.NearestFoodVertical = null;
        }

        private static void CheckOtherSide(PlayingMap map, int xFactor, int yFactor, Cordinates point, ref double minDistance, ref Cordinates nearestFoodCor)
        {
            foreach (var food in map.Food.FoodCordinates)
            {
                double dist = Math.Sqrt(
                    Math.Pow(point.X - (food.X + map.sideSize * xFactor), 2)
                    + Math.Pow(point.Y - (food.Y + map.sideSize * yFactor), 2));

                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearestFoodCor = food;
                    nearestFoodCor.X += xFactor * map.sideSize;
                    nearestFoodCor.Y += yFactor * map.sideSize;
                }
            }
        }

        private struct MapCoordConverter
        {
            public Cordinates PMapCoord;
            public Cordinates NNMapCoord;
        }
        
    }
}
