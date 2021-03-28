using Situations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Menu.Attributes
{
    class SituationsInit
    {
        public static SituationsInit Instance { get; }

        static SituationsInit() { Instance = new SituationsInit(); }
        private SituationsInit()
        {
            AchievedLength = AchievedLengthEnum.None;
            Length = 10;
            Names = new HashSet<string>() { nameof(RandPathwaySnake) };
            CollisionWithBarrier = CollisionWithBarrierEnum.Dead;
            CollisionWithFood = CollisionWithFoodEnum.IncreaseBody;
            CollisionWithSnake = CollisionWithSnakeEnum.Dead;
            DidStepsWithoutFood = DidStepsWithoutFoodEnum.None;
            Steps = 10;
        }

        public AchievedLengthEnum AchievedLength { get; set; }
        public int Length { get; set; }
        public HashSet<string> Names { get; set; }
        public CollisionWithBarrierEnum CollisionWithBarrier { get; set; }
        public CollisionWithFoodEnum CollisionWithFood { get; set; }
        public CollisionWithSnakeEnum CollisionWithSnake { get; set; }
        public DidStepsWithoutFoodEnum DidStepsWithoutFood { get; set; }
        public int Steps { get; set; }

        public enum AchievedLengthEnum
        {
            None,
            AppearNewSnake
        }
        public enum CollisionWithBarrierEnum
        {
            Dead,
            Stops
        }
        public enum CollisionWithFoodEnum
        {
            IncreaseBody,
            LeftCurrentSize
        }
        public enum CollisionWithSnakeEnum
        {
            Dead,
            Stops
        }
        public enum DidStepsWithoutFoodEnum
        {   
            None,
            Decrease
        }

        public struct SituationsObjects
        {
            public IAchievedLength AchievedLength { get; set; }
            public ICollisionWithBarrier CollisionWithBarrier { get; set; }
            public ICollisionWithFood CollisionWithFood { get; set; }
            public ICollisionWithSnake CollisionWithSnake { get; set; }
            public IDidStepsWithoutFood DidStepsWithoutFood { get; set; }
        }

        public SituationsObjects GetSituationsObjects()
        {
            var so = new SituationsObjects();

            switch (AchievedLength)
            {
                case AchievedLengthEnum.AppearNewSnake:
                    so.AchievedLength = new AppearNewSnake(Length, Names.ToList());
                    break;
                case AchievedLengthEnum.None:
                    so.AchievedLength = new None(Length);
                    break;
            }
            switch (CollisionWithBarrier)
            {
                case CollisionWithBarrierEnum.Dead:
                    so.CollisionWithBarrier = new Dead();
                    break;
                case CollisionWithBarrierEnum.Stops:
                    so.CollisionWithBarrier = new Stops();
                    break;
            }
            switch (CollisionWithFood)
            {
                case CollisionWithFoodEnum.IncreaseBody:
                    so.CollisionWithFood = new IncreaseBody();
                    break;
                case CollisionWithFoodEnum.LeftCurrentSize:
                    so.CollisionWithFood = new LeftCurrentSize();
                    break;
            }
            switch (CollisionWithSnake)
            {
                case CollisionWithSnakeEnum.Dead:
                    so.CollisionWithSnake = new SnakeDead();
                    break;
                case CollisionWithSnakeEnum.Stops:
                    so.CollisionWithSnake = new SnakeStops();
                    break;
            }
            switch (DidStepsWithoutFood)
            {
                case DidStepsWithoutFoodEnum.None:
                    so.DidStepsWithoutFood = new NoAction(Steps);
                    break;
                case DidStepsWithoutFoodEnum.Decrease:
                    so.DidStepsWithoutFood = new Decrease(Steps);
                    break;
            }

            return so;
        }
    }
}
