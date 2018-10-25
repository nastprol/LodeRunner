using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    public class GameMap
    {
        public int GetWidth() => Map.GetLength(0);
        public int GetHeight() => Map.GetLength(1);
        public GameMap() { }

        public GameMap(GameMap gameMap, ICreature[,] map)
        {
            Map = map;
            PlayerCoordinate = gameMap.PlayerCoordinate;
            Player = gameMap.Player;
            BonusTotalCount = gameMap.BonusTotalCount;
            coveredByOther = gameMap.coveredByOther;
        }

        public GameMap(ICreature[,] map, Point playerCoord, Player player, int bonusCount,
           HashSet<Point> monsters, HashSet<Point> ladders, HashSet<Point> terrain,
           HashSet<Point> ropes, HashSet<Point> bonuses, HashSet<Point> holes)
        {
            Map = map;
            PlayerCoordinate = playerCoord;
            Player = player;
            BonusTotalCount = bonusCount;
            coveredByOther = new Dictionary<Point, ICreature>();
        }

        public Point PlayerCoordinate;

        public readonly Player Player;
        public readonly ICreature[,] Map;

        public int BonusTotalCount { get; private set; }

        private Dictionary<Point, ICreature> coveredByOther; 

        public bool InBounds(Point point)
        {
            return point.X >= 0 && point.X < Map.GetLength(0) && point.Y >= 0 && point.Y < Map.GetLength(1);
        }

        public bool InBounds(int x, int y)
        {
            var point = new Point(x, y);
            return InBounds(point);
        }

        public void StandOverCreature(int x, int y)
        {
            if (Map[x, y] != null)
            {
                coveredByOther.Add(new Point(x, y), Map[x, y]);
            }
        }

        public ICreature BecomeVisible(int x, int y)
        {
            ICreature covered = null;
            var point = new Point(x, y);
            if (coveredByOther.ContainsKey(point))
            {
                covered = coveredByOther[point];
                coveredByOther.Remove(point);
            }
            return covered;
        }

        public bool Overlaps(int x, int y, out ICreature covered)
        {
            var point = new Point(x, y);
            var contains = coveredByOther.ContainsKey(point);
            covered = contains ? coveredByOther[point] : null;
            return contains;
        }
    }
}