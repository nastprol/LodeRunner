using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class MapCreator
    {
        private Player player;
        private Point playerCoordinate;
        private string map;
        private int bonusTotalCount;

        private HashSet<Point> monsters = new HashSet<Point>();
        private HashSet<Point> ladders = new HashSet<Point>();
        private HashSet<Point> ropes = new HashSet<Point>();
        private HashSet<Point> bonuses = new HashSet<Point>();
        private HashSet<Point> holes = new HashSet<Point>();
        private HashSet<Point> terrain = new HashSet<Point>();

        public MapCreator(string map)
        {
            this.map = map;
            bonusTotalCount = 0;
        }

        public GameMap CreateMap()
        {
            var rows = map.Split('\n');

            if (rows
                .Select(z => z.Length)
                .Distinct()
                .Count() != 1)
                throw new Exception($"Wrong test map '{map}'");

            var result = new ICreature[rows[0].Length, rows.Length];

            for (var x = 0; x < rows[0].Length; x++)
            {
                for (var y = 0; y < rows.Length; y++)
                {
                    var newCreature = GetCreatureBySymbol(rows[y][x], x, y);
                    result[x, y] = newCreature;
                }
            }
            if (player == null)
                throw new FormatException("no player");

            return new GameMap(result, playerCoordinate, player, bonusTotalCount, monsters, ladders,
                terrain, ropes, bonuses, holes);
        }

        private ICreature GetCreatureBySymbol(char element, int x, int y)
        {
            var point = new Point(x, y);
            switch (element)
            {
                case 'P':
                    if (player != null)
                        throw new FormatException("Too many players");
                    player = new Player();
                    playerCoordinate = new Point(x, y);
                    return player;
                case 'T':
                    var terr = new Terrain();
                    terrain.Add(point);
                    return terr;
                case 'B':
                    var bonus = new Bonus();
                    bonusTotalCount++;
                    bonuses.Add(point);
                    return bonus;
                case 'M':
                    var monster = new Monster();
                    monsters.Add(point);
                    return monster;
                case 'L':
                    var ladder = new Ladder();
                    ladders.Add(point);
                    return ladder;
                case 'H':
                    var hole = new Hole();
                    holes.Add(point);
                    return hole;
                case 'R':
                    var rope = new Rope();
                    ropes.Add(point);
                    return rope;
                case ' ':
                    return null;
                default:
                    throw new Exception($"wrong character for ICreature {element}");
            }
        }
    }
}
