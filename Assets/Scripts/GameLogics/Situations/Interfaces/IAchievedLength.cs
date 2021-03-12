using Map;

namespace Situations
{
    public interface IAchievedLength
    {
        public int Length { get; }
        void OnAchievedLength(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap);
    }
}
