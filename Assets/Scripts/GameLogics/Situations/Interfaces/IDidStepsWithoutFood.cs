using Map;

namespace Situations
{
    public interface IDidStepsWithoutFood
    {
        int StepsWithoutFood { get; }
        void OnStepsDid(PlayingMapAttributes.Snake snake, PlayingMap mapCopy);
    }
}
