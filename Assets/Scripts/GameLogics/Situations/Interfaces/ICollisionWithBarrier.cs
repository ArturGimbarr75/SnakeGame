using Map;

namespace Situations
{
    public interface ICollisionWithBarrier
    {
        void OnColision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy);
    }
}
