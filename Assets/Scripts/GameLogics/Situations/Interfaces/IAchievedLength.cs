using Map;

namespace Situations
{
    public interface IAchievedLength
    {
        int Length { get; }
        void OnAchievedLength (PlayingMapAttributes.Snake snake, PlayingMap mapCopy);
    }
}
