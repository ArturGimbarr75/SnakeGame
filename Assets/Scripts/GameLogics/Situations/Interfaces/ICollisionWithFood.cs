using Map;

namespace Situations
{
    public interface ICollisionWithFood
    {
        bool OnCollision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy);
    }
}
