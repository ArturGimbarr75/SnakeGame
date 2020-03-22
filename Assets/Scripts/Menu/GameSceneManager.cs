using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using Logic;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Menu
{
    class GameSceneManager : MonoBehaviour
    {
        public TileBase Grass;
        public TileBase Food;
        public List<TileBase> AdamBody;
        public List<TileBase> DeadBody;
        public List<TileBase> FollowFoodSnakeBody;
        public List<TileBase> PlayerArrowsBody;
        public List<TileBase> PlayerWASDBody;
        public List<TileBase> RandPathwaySnakeBody;

        public Tilemap Snakes;
        public Tilemap Background;
        public Grid GameGrid;

        private List<TileBase> SnakeBody;
        private GameLogicBase GameLogic;
        private PlayingMap Map;

        void Start()
        {
            /* GameLogic = new StandartLogic
                 (
                 GameInits.GameoverPredicates,
                 GameInits.SnakeNames,
                 new AssemblySnakeFactory(),
                 GameInits.MapSize,
                 GameInits.FoodCount,
                 GameInits.LeftDeadSnakeBody
                 );*/
            GameLogic = new StandartLogic
                (
                new HashSet<GameLogicsAttributes.GameoverPredicates>()
                { GameLogicsAttributes.GameoverPredicates.DeadAllSnakes },
                new List<string>() { "Adam" },
                new AssemblySnakeFactory(),
                20,
                10,
                false
                );
            Map = GameLogic.GetCurrentPlayingMap();

            FillBackground();
            SetGridScale();

            ShowFood();
        }

        void Update()
        {

        }

        private void ShowSnakes()
        {

        }

        private void ShowFood()
        {
            foreach (var food in Map.Food.FoodCordinates)
                Snakes.SetTile(ConvertMapCoorToTileCoor(food.X, food.Y), Food);
        }

        private void FillBackground()
        {
            for (int i = 0; i < Map.sideSize; i++)
                for (int j = 0; j < Map.sideSize; j++)
                    Background.SetTile(ConvertMapCoorToTileCoor(i, j), Grass);
        }

        private void SetGridScale()
        {
            const float ConstInHyperbolaFunc = 54;
            float scaleVal = ConstInHyperbolaFunc / Map.sideSize;

            GameGrid.transform.localScale = new Vector3(scaleVal, scaleVal);
        }

        private Vector3Int ConvertMapCoorToTileCoor(int x, int y)
        {
            int shift = Map.sideSize / 2;
            int xTile = x - shift;
            int yTile = y - shift;

            return new Vector3Int(xTile, yTile, 0);
        }

        public enum BodyPart
        {
            HeadUp = 1,
            HeadRight = 8,
            HeadDown = 7,
            HeadLeft = 0,

            DownRight = 2,
            DownLeft = 3,
            UpRight = 9,
            UpLeft = 10,

            Horizontal = 4,
            Vertical = 11,

            TailUp = 12,
            TailRight = 13,
            TailDown = 5,
            TailLeft = 6
        }
    }
}
