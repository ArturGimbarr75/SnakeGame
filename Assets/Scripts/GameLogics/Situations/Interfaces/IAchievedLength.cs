using Logic;
using Map;

namespace Situations
{
    public interface IAchievedLength
    {
        public int Length { get; }
        void OnAchievedLength(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap, GameLogicBase gl);
        void AddSnakes(PlayingMap currentMap);
    }
}
