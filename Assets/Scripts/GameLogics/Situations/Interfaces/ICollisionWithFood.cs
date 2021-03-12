using Map;

namespace Situations
{
    public interface ICollisionWithFood
    {
        void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap);
    }
}
