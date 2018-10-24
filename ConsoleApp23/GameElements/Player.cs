using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    public class Player : ICreature
    {
        public bool IsAlive = true;
        public int PlayerDirection { get; private set; }
        public int BonusCollectCount;

        public Point Act(int x, int y, GameMap gameMap, Direction key)
        {
            var map = gameMap.Map;
            var point = new Point(x, y);
            switch (key)
            {
                case Direction.Right:
                    PlayerDirection = 1;
                    return new Point(x + 1, y);
                case Direction.Left:
                    PlayerDirection = -1;
                    return new Point(x - 1, y);
                case Direction.Up:
                    if (gameMap.Overlaps(point.X, point.Y, out ICreature covered) && covered is Ladder)
                        return new Point(x, y - 1);
                    break;
                case Direction.Down:
                    if (gameMap.Overlaps(point.X, point.Y, out covered) && covered is Ladder || covered is Rope)
                        return new Point(x, y + 1);
                    break;
                case Direction.Space:
                    return Dig(x, y, map);
            }
            return new Point(x, y);
        }

        private static bool InBounds(int x, int y, ICreature[,] map)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            return x >= 0 && y >= 0 && x < mapWidth && y < mapHeight;
        }

        public Point Dig(int x, int y, ICreature[,] map)
        {
            if (InBounds(x + PlayerDirection, y, map) && map[x + PlayerDirection, y] == null)
            {
                return new Point(x + PlayerDirection, y);
            }
            return new Point(x, y);
        }
    }
}
