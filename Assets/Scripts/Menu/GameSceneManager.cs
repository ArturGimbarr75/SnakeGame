using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using Logic;
using Map;
using Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    class GameSceneManager : MonoBehaviour
    {
        private TileBase Grass;
        private TileBase Food;
        private TileBase Barrier;
        private Dictionary<string, List<TileBase>> SnakeBodies;
        private const string DEAD_SNAKE_BODY = "DeadSnake";

        public Tilemap SnakesTileMap;
        public Tilemap BackgroundTileMap;
        public Grid GameGrid;
        public GameObject RowPrefab;

        private List<Text> TextRows;
        private List<TileBase> SnakeBody;
        private GameLogicBase GameLogic;
        private PlayingMap Map;
        private float TimePause;
        private bool EndGame;

        void Start()
        {
            EndGame = false;
            TimePause = Time.time + GameInits.Pause;
            GameLogic = new CustomisingLogic
                (
                GameInits.GameoverPredicates,
                GameInits.SnakeNames,
                GameInits.Assembly,
                GameInits.MapSize,
                GameInits.FoodCount,
                GameInits.LeftDeadSnakeBody
                );
            Map = GameLogic.GetCurrentPlayingMap();

            SetTileBases();

            FillBackground();
            SetGridScale();

            ShowFood();
            ShowSnakes();
            ShowBarriers();

            SetUpStatisticsTable();
        }

        void Update()
        {
            if (EndGame || GameLogic.IsGameEnded())
            {
                var statistics = GameLogic.GetSnakeStatistics();
                GameInits.SnakeStatistics = statistics;
                SceneManager.LoadScene(3);
            }

            if (TimePause <= Time.time && !GameLogic.IsGameEnded())
            {
                TimePause = Time.time + GameInits.Pause;

                Map = GameLogic.GetNextPlayingMap();
                SnakesTileMap.ClearAllTiles();
                ShowFood();
                ShowSnakes();
                ShowBarriers();
                UpdateStatisticsTable();
            }
        }

        public void OnEndGameClick()
        {
            EndGame = true;
        }

        private void SetUpStatisticsTable()
        {
            var statistics = GameLogic.GetSnakeStatistics();
            TextRows = new List<Text>();
            foreach (var s in statistics)
            {
                var row = GameObject.Instantiate(RowPrefab);
                var rowContent = RowPrefab.GetComponentsInChildren<Text>();
                Text score = null, name = null;
                for (int i = 0; i < rowContent.Length; i++)
                {
                    if (rowContent[i].name.Equals("Name"))
                        name = rowContent[i];
                    else
                        score = rowContent[i];
                }
                row.transform.parent = RowPrefab.transform.parent;
                name.text = s.Name;
                score.text = s.EatenFood.ToString();
            }
            // 2-ая строка всегда будет префабом
            // 2-nd row always will be prefab
            RowPrefab.transform.parent.GetChild(1).gameObject.SetActive(false);
            for (int i = 0; i < RowPrefab.transform.parent.childCount; i++)
                if (i != 1)
                    TextRows.Add(RowPrefab.transform.parent.GetChild(i).gameObject.transform.GetChild(1).GetComponents<Text>().Where(x => x.name == "Score").Select(x => x).First());
            // По какой-то причине последний эллемент перепрыгивает в начало
            // For some reason last element jumps to begin
            var temp = TextRows[0];
            TextRows.RemoveAt(0);
            TextRows.Add(temp);
        }

        private void UpdateStatisticsTable()
        {
            var statistics = GameLogic.GetSnakeStatistics();

            for (int i = 0; i < statistics.Count && i < TextRows.Count; i++)
                TextRows[i].text = statistics[i].EatenFood.ToString();
        }

        private void SetTileBases()
        {
            SnakeBodies = new Dictionary<string, List<TileBase>>();
            var snakeFactory = new AssemblySnakeFactory();
            var allSnakes = snakeFactory.GetAllSnakeTypes();
            allSnakes.Add(DEAD_SNAKE_BODY);
            foreach (var snake in allSnakes)
            {
                var body = new List<TileBase>();
                for (int i = 0; i < 14; i++)
                {
                    string path = String.Format("Assets\\IMG\\SnakeSprites\\Simple\\{0}\\{0}_{1}.asset", snake, i);
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(path, typeof(TileBase));
                    if (prefab == null)
                    {
                        if (SnakeBodies.ContainsKey(snake))
                            SnakeBodies.Remove(snake);
                        break;                       
                    }
                    body.Add((TileBase)Instantiate(prefab)); //TODO: обработать исключение при отсутсвии спрайта
                }
                if (body.Count == 14)
                    SnakeBodies.Add(snake, body);
            }


            Food = (TileBase)Instantiate(AssetDatabase.LoadAssetAtPath("Assets\\IMG\\Food\\Simple\\Apple.asset", typeof(TileBase)));
            Grass = (TileBase)Instantiate(AssetDatabase.LoadAssetAtPath("Assets\\IMG\\Background\\Simple\\Grass.asset", typeof(TileBase)));
            Barrier = (TileBase)Instantiate(AssetDatabase.LoadAssetAtPath("Assets\\IMG\\Barriers\\Simple\\Barrier.asset", typeof(TileBase)));

        }

        private void ShowSnakes()
        {
            foreach (var snake in Map.Snake)
            {
                /// Выбор спрайта для отрисовки
                if (!snake.IsAlive)
                    SnakeBody = SnakeBodies[DEAD_SNAKE_BODY];
                else if (SnakeBodies.ContainsKey(snake.Name))
                    SnakeBody = SnakeBodies[snake.Name];

                ShowSnakesHead(snake);
                ShowSnakesBody(snake);
                ShowSnakesTail(snake);

            }
        }

        private void ShowSnakesHead (PlayingMapAttributes.Snake snake)
        {
            if (snake.Cordinates[0].X == snake.Cordinates[1].X + 1)
            {
                SnakesTileMap.SetTile(
                    ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                    SnakeBody[(int)BodyPart.HeadLeft]);
            }
            else if (snake.Cordinates[0].X == snake.Cordinates[1].X - 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadRight]);
            }
            else if (snake.Cordinates[0].Y == snake.Cordinates[1].Y + 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadUp]);
            }
            else if (snake.Cordinates[0].Y == snake.Cordinates[1].Y - 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadDown]);
            }
            else if (snake.Cordinates[0].X > snake.Cordinates[1].X)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadRight]);
            }
            else if (snake.Cordinates[0].X < snake.Cordinates[1].X)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadLeft]);
            }
            else if (snake.Cordinates[0].Y < snake.Cordinates[1].Y)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadUp]);
            }
            else if (snake.Cordinates[0].Y > snake.Cordinates[1].Y)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[0].X, snake.Cordinates[0].Y),
                   SnakeBody[(int)BodyPart.HeadDown]);
            }
        }

        private void ShowSnakesBody (PlayingMapAttributes.Snake snake)
        {
            for (int i = 1; i < snake.Cordinates.Count - 1; i++)
            {
                var next = snake.Cordinates[i - 1];
                var curr = snake.Cordinates[i];
                var bef = snake.Cordinates[i + 1];

                var t1 = next - curr;
                var t2 = bef - curr;

                const int min = -1, max = 1;
                /// Выравниваем значения
                while (t1.X < min) t1.X += Map.sideSize;
                while (t1.X > max) t1.X -= Map.sideSize;
                while (t1.Y < min) t1.Y += Map.sideSize;
                while (t1.Y > max) t1.Y -= Map.sideSize;
                while (t2.X < min) t2.X += Map.sideSize;
                while (t2.X > max) t2.X -= Map.sideSize;
                while (t2.Y < min) t2.Y += Map.sideSize;
                while (t2.Y > max) t2.Y -= Map.sideSize;

                if (t1 - t2 == new SnakeAttribute.Cordinates (0, -2) || t1 - t2 == new SnakeAttribute.Cordinates(0, 2))
                {
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y),SnakeBody[(int)BodyPart.Vertical]);
                    continue;
                }
                if (t1 - t2 == new SnakeAttribute.Cordinates (-2, 0) || t1 - t2 == new SnakeAttribute.Cordinates(2, 0))
                {
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.Horizontal]);
                    continue;
                }

                if (t1 + t2 == new SnakeAttribute.Cordinates (1, 1))
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.DownRight]);
                else if (t1 + t2 == new SnakeAttribute.Cordinates(-1, 1))
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.DownLeft]);
                else if (t1 + t2 == new SnakeAttribute.Cordinates(1, -1))
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.UpRight]);
                else if (t1 + t2 == new SnakeAttribute.Cordinates(-1, -1))
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.UpLeft]);
                else
                    SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(curr.X, curr.Y), SnakeBody[(int)BodyPart.HeadDown]);
            }
        }

        private void ShowSnakesTail (PlayingMapAttributes.Snake snake)
        {
            if (snake.Cordinates[snake.Cordinates.Count - 1].X == snake.Cordinates[snake.Cordinates.Count - 2].X + 1)
            {
                SnakesTileMap.SetTile(
                    ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                    SnakeBody[(int)BodyPart.TailLeft]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].X == snake.Cordinates[snake.Cordinates.Count - 2].X - 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailRight]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].Y == snake.Cordinates[snake.Cordinates.Count - 2].Y + 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailUp]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].Y == snake.Cordinates[snake.Cordinates.Count - 2].Y - 1)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailDown]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].X > snake.Cordinates[snake.Cordinates.Count - 2].X)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailRight]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].X < snake.Cordinates[snake.Cordinates.Count - 2].X)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailLeft]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].Y < snake.Cordinates[snake.Cordinates.Count - 2].Y)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailUp]);
            }
            else if (snake.Cordinates[snake.Cordinates.Count - 1].Y > snake.Cordinates[snake.Cordinates.Count - 2].Y)
            {
                SnakesTileMap.SetTile(
                   ConvertMapCoorToTileCoor(snake.Cordinates[snake.Cordinates.Count - 1].X, snake.Cordinates[snake.Cordinates.Count - 1].Y),
                   SnakeBody[(int)BodyPart.TailDown]);
            }
        }

        private void ShowFood()
        {
            foreach (var food in Map.Food.FoodCordinates)
                SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(food.X, food.Y), Food);
        }

        private void ShowBarriers()
        {
            foreach (var barrier in Map.Barriers)
                SnakesTileMap.SetTile(ConvertMapCoorToTileCoor(barrier.X, barrier.Y), Barrier);
        }

        private void FillBackground()
        {
            for (int i = 0; i < Map.sideSize; i++)
                for (int j = 0; j < Map.sideSize; j++)
                    BackgroundTileMap.SetTile(ConvertMapCoorToTileCoor(i, j), Grass);
        }

        private void SetGridScale()
        {
            const float ConstInHyperbolaFunc = 54;
            float scaleVal = ConstInHyperbolaFunc / Map.sideSize;

            GameGrid.transform.localScale = new Vector3(scaleVal, scaleVal);
        }

        private Vector3Int ConvertMapCoorToTileCoor(int x, int y)
        {
            int shift = 1;
            int xTileFirst = -Map.sideSize / 2;
            int yTileFirst = Map.sideSize / 2;
            int xTile = xTileFirst + x;
            int yTile = yTileFirst - y - shift;

            return new Vector3Int(xTile, yTile, 0);
        }

        /// <summary>
        /// Указания на места соединения частей змейки
        /// </summary>
        private enum BodyPart
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
