using Map;

namespace Situations
{
    public interface ICollisionWithBarrier
    {
        void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap);
    }
}
