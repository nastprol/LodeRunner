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

            //Monsters = gameMap.Monsters;
            //Holes = gameMap.Holes;
            //PlayerCoordinate = gameMap.PlayerCoordinate;
            //Bonuses = gameMap.Bonuses;

            //Ladders = gameMap.Ladders;
            //Ropes = gameMap.Ropes;
            //Terrain = gameMap.Terrain;
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

            //Monsters = monsters;
            //Ladders = ladders;
            //Ropes = ropes;
            //Bonuses = bonuses;
            //Holes = holes;
            //Terrain = terrain;
        }

        //public HashSet<Point> Monsters;
        //public HashSet<Point> Bonuses;
        //public HashSet<Point> Holes;

        //public HashSet<Point> Ladders;
        //public HashSet<Point> Ropes;
        //public HashSet<Point> Terrain;
        public Point PlayerCoordinate;

        public readonly Player Player;
        public readonly ICreature[,] Map;

        public int BonusTotalCount { get; private set; }

        private Dictionary<Point, ICreature> coveredByOther;  //delete

        //public Creature IdentifyCreature(int x, int y)
        //{
        //    var point = new Point(x, y);
        //    if (Monsters.ContainsKey(point))
        //        return Creature.Monster;
        //    if (Ladders.ContainsKey(point))
        //        return Creature.Ladder;
        //    if (Ropes.ContainsKey(point))
        //        return Creature.Rope;
        //    if (Bonuses.ContainsKey(point))
        //        return Creature.Bonus;
        //    if (Holes.ContainsKey(point))
        //        return Creature.Hole;
        //    if (Terrain.ContainsKey(point))
        //        return Creature.Terrain;
        //    else
        //        return Creature.Empty;
        //}


        //public bool IsTextures(int x, int y) => Map[x, y] != Player && !Monsters.Contains(new Point(x, y));
        //public bool IsMonster(int x, int y) => Monsters.Contains(new Point(x, y));
        //public bool IsBonus(int x, int y) => Bonuses.Contains(new Point(x, y));
        //public bool IsLadder(int x, int y) => Ladders.Contains(new Point(x, y));
        //public bool IsTerrain(int x, int y) => Terrain.Contains(new Point(x, y));
        //public bool IsHole(int x, int y) => Holes.Contains(new Point(x, y));
        //public bool IsRope(int x, int y) => Ropes.Contains(new Point(x, y));
        //public bool IsPlayer(int x, int y) => PlayerCoordinate == new Point(x, y);


        public bool InBounds(Point point)
        {
            return point.X >= 0 && point.X < Map.GetLength(0) && point.Y >= 0 && point.Y < Map.GetLength(1);
        }

        public bool InBounds(int x, int y)
        {
            var point = new Point(x, y);
            return InBounds(point);
        }

        //public List<Point> GetActiveCreatures()
        //{
        //    return new List<Point>(Monsters)
        //    {
        //        PlayerCoordinate
        //    };
        //}

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