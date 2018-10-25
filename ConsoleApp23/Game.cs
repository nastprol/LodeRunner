using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    public class Game
    {
        private GameMap gameMap;

        public ICreature[,] Map => gameMap.Map;
        private Player Player => gameMap.Player;
        public int MapWidth => gameMap.GetWidth();
        public int MapHeight => gameMap.GetHeight();

        public int BonusTotalCount => gameMap.BonusTotalCount;
        public int BonusLeft => BonusTotalCount - Player.BonusCollectCount;
        public bool IsOver { get; private set; }
        public bool PlayerWon => Player.IsAlive && IsOver;


        private ICreature[,] tempMap; 

        public Game(GameMap map)
        {
            gameMap = map;
        }

        private void Teleport(int telX, int telY, ICreature monster)
        {
            var rand = new Random();
            var x = rand.Next(0, Map.GetLength(0));
            var y = rand.Next(0, Map.GetLength(1));
            while (Map[x, y] is Terrain || Map[x, y] is Player || x == telX && y == telY)
            {
                x = rand.Next(0, Map.GetLength(0));
                y = rand.Next(0, Map.GetLength(1));
            }
            tempMap[x, y] = monster;
            tempMap[telX, telY] = null;
        }

        private void TryMove(int x, int y, int prevX, int prevY)
        {
            var creature = Map[prevX, prevY];
            gameMap.BecomeVisible(prevX, prevY);


            if (creature is Player && Map[x, y] is Bonus)
            {
                Player.BonusCollectCount++;
                tempMap[x, y] = Player;
            }
            else if (creature is Monster && Map[x, y] is Hole)
            {
                Teleport(x, y, creature);
            }
            else if (creature is Monster && Map[x, y] is Player || creature is Player && Map[x, y] is Monster)
            {
                tempMap[x, y] = creature is Monster ? creature : Map[x, y];
                Player.IsAlive = false;
                GameOver();
            }
            else
            {
                gameMap.StandOverCreature(x, y);
                tempMap[x, y] = creature;
            }
        }

        private void GameOver()
        {
            IsOver = true;
        }

        public void UpdateMap(Direction key = Direction.None)
        {
            tempMap = new ICreature[Map.GetLength(0), Map.GetLength(1)];
            var playerCoord = new Point();
            var monsters = new List<Point>();

            for (int x = Map.GetLength(0) - 1; x >= 0; x--)
            {
                for (int y = Map.GetLength(1) - 1; y >= 0; y--)
                {
                    var activeCreature = Map[x, y];
                    if (activeCreature is Textures)
                    {
                        tempMap[x, y] = activeCreature;
                    }
                    if (activeCreature is Player)
                        playerCoord = new Point(x, y);
                    if (activeCreature is Monster)
                        monsters.Add(new Point(x, y));
                }
            }
            var creatures = monsters;
            creatures.Add(playerCoord);

            foreach (var coord in creatures)
            {
                var x = coord.X;
                var y = coord.Y;
                var creature = Map[x, y];
                if (CreatureHasToFall(x, y, creature))
                {
                    TryMove(x, y + 1, x, y);
                }
                else
                {
                    var nextPoint = creature.Act(x, y, gameMap, key);

                    if (nextPoint.X == coord.X + Player.PlayerDirection && key == Direction.Space && Map[x, y] == Player)
                    {
                        tempMap[x, y] = Player;
                        tempMap[nextPoint.X, nextPoint.Y] = new Hole();
                        continue;
                    }

                    if (CreatureCanMoveTo(nextPoint.X, nextPoint.Y, creature))
                        TryMove(nextPoint.X, nextPoint.Y, x, y);
                    else
                        tempMap[x, y] = creature;
                }

                if (!Player.IsAlive || BonusLeft == 0)
                {
                    GameOver();
                    break;
                }
            }

            gameMap = new GameMap(gameMap, tempMap);
            tempMap = new ICreature[Map.GetLength(0), Map.GetLength(1)];
        }

        private bool CreatureHasToFall(int x, int y, ICreature creature)
        {
            return !(creature is Textures) && gameMap.InBounds(x, y + 1) &&
                (Map[x, y + 1] is null
                || creature is Monster && (Map[x, y + 1] is Hole || Map[x, y + 1] is Player)
                || creature is Player && (Map[x, y + 1] is Hole
                                        || Map[x, y + 1] is Bonus
                                        || Map[x, y + 1] is Monster))
                && (!gameMap.Overlaps(x, y, out var covered)
                    || gameMap.Overlaps(x, y, out covered)
                        && !(covered is Ladder || covered is Rope));
        }

        private bool CreatureCanMoveTo(int x, int y, ICreature creature)
        {
            if (creature is Textures || creature is null)
                return false;

            return gameMap.InBounds(x, y) && !(Map[x, y] is Terrain);
        }
    }
}