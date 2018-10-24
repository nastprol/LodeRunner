using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;

namespace MyGame
{
    [TestFixture]
    public class MapCreatingTests
    {
        [Test]
        public void GetSimpleMap()
        {
            var map = "PTMLBR";
            var creatureMap = new MapCreator(map).CreateMap().Map;

            Assert.AreEqual(6, creatureMap.GetLength(0));
            Assert.AreEqual(1, creatureMap.GetLength(1));

            Assert.IsInstanceOf<Player>(creatureMap[0, 0]);
            Assert.IsInstanceOf<Terrain>(creatureMap[1, 0]);
            Assert.IsInstanceOf<Monster>(creatureMap[2, 0]);
            Assert.IsInstanceOf<Ladder>(creatureMap[3, 0]);
            Assert.IsInstanceOf<Bonus>(creatureMap[4, 0]);
        }

        [Test]
        public void NoMovementsTest()
        {
            var map = "PTMLBR";
            var gameMap = new MapCreator(map).CreateMap();
            var creatureMap = gameMap.Map;
            var game = new Game(gameMap);

            Assert.IsInstanceOf<Player>(creatureMap[0, 0]);
            Assert.IsInstanceOf<Terrain>(creatureMap[1, 0]);
            Assert.IsInstanceOf<Monster>(creatureMap[2, 0]);
            Assert.IsInstanceOf<Ladder>(creatureMap[3, 0]);
            Assert.IsInstanceOf<Bonus>(creatureMap[4, 0]);
            Assert.IsInstanceOf<Rope>(creatureMap[5, 0]);

            game.UpdateMap();

            Assert.IsInstanceOf<Player>(creatureMap[0, 0]);
            Assert.IsInstanceOf<Terrain>(creatureMap[1, 0]);
            Assert.IsInstanceOf<Monster>(creatureMap[2, 0]);
            Assert.IsInstanceOf<Ladder>(creatureMap[3, 0]);
            Assert.IsInstanceOf<Bonus>(creatureMap[4, 0]);
            Assert.IsInstanceOf<Rope>(creatureMap[5, 0]);
        }

        [Test]
        public void ExceptionWhenMoreThenOnePlayer()
        {
            var map = "PP";
            var t = false;
            try
            {
                var creatureMap = new MapCreator(map).CreateMap();
            }
            catch (FormatException)
            {
                t = true;
            }
            Assert.IsTrue(t);
        }

        [Test]
        public void ExceptionWhenNoPlayer()
        {
            var map = "TT";
            var t = false;
            try
            {
                var creatureMap = new MapCreator(map).CreateMap();
            }
            catch (FormatException)
            {
                t = true;
            }
            Assert.IsTrue(t);
        }

        [Test]
        public void CorrectBonusCount()
        {
            var map = "PBMTB";
            var creatureMap = new MapCreator(map).CreateMap();
            var game = new Game(creatureMap);
            Assert.AreEqual(2, game.BonusTotalCount);
        }
    }

    [TestFixture]
    public class MonsterActTests
    {
        private static Point FindMonster(ICreature[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Monster)
                        return new Point(i, j);
                }
            }
            return new Point(-1, -1);
        }

        [Test]
        public void MakeSimpleStepToPlayer()
        {
            var map = @"M P";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap();
            var result = FindMonster(game.Map);
            Assert.AreEqual(new Point(1, 0), result);
            game.UpdateMap();
            result = FindMonster(game.Map);
            Assert.AreEqual(new Point(2, 0), result);
        }

        [Test]
        public void MakeStepToPlayer()
        {
            var map = "     \nM    \nTT   \n  T P";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            for (var i = 0; i < 6; i++)
                game.UpdateMap();
            var result = FindMonster(game.Map);
            Assert.AreEqual(new Point(4, 3), result);
        }

        [Test]
        public void NoStepToPlayer()
        {
            var map = " MTP";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap();
            var result = FindMonster(game.Map);
            Assert.AreEqual(new Point(0, 0), result);
        }

        [Test]
        public void DifficultPath()
        {
            var map = "TL     \nTLTTTTT\n L L M \n L LTT \n L L   \nTLTLT T\n       \nP  L   \nTTTTTTT";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            for (var i = 0; i < 12; i++)
                game.UpdateMap();
            var result = FindMonster(game.Map);
            Assert.AreEqual(new Point(0, 7), result);
        }

        [Test]
        public void Teleport()
        {
            var map = "MT   P   ";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap();
            var result = FindMonster(game.Map);
            Assert.AreNotEqual(new Point(1, 0), result);
        }
    }


    [TestFixture]
    public class PlayerActTests
    {
        private static Point FindPlayer(ICreature[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Player)
                        return new Point(i, j);
                }
            }
            return new Point(-1, -1);
        }

        [Test]
        public void MakeSimpleSteps()
        {
            var map =
                @"T P T";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Right);
            var result = FindPlayer(game.Map);
            Assert.AreEqual(new Point(3, 0), result);

            game.UpdateMap(Direction.Left);
            result = FindPlayer(game.Map);
            Assert.AreEqual(new Point(2, 0), result);

            game.UpdateMap(Direction.Up);
            result = FindPlayer(game.Map);
            Assert.AreEqual(new Point(2, 0), result);
        }

        [Test]
        public void PlayerDig()
        {
            var map = "P  B";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Right);
            game.UpdateMap(Direction.Space);

            Assert.IsTrue(game.Map[2, 0] is Hole);
        }

        [Test]
        public void UseLadder()
        {
            var map = "L  B\nLP  ";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Left);
            game.UpdateMap(Direction.Up);
            Assert.AreEqual(new Point(0, 0), FindPlayer(game.Map));
            game.UpdateMap(Direction.Down);
            Assert.AreEqual(new Point(0, 1), FindPlayer(game.Map));
        }

        [Test]
        public void UseRope()
        {
            var map = "  PRR  \nTTT  TT";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Right);
            game.UpdateMap(Direction.Right);
            game.UpdateMap(Direction.Right);

            Assert.AreEqual(new Point(5, 0), FindPlayer(game.Map));
        }

        [Test]
        public void TakeBonus()
        {
            var map = "PBB";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Right);
            Assert.AreEqual(new Point(1, 0), FindPlayer(game.Map));
            Assert.AreEqual(1, game.BonusLeft);
            game.UpdateMap(Direction.Left);
            Assert.AreEqual(game.Map[1, 0], null);
        }

        [Test]
        public void PlayerHasToFall()
        {
            var map = "P B\n   \nTTT";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap();
            Assert.AreEqual(new Point(0, 1), FindPlayer(game.Map));
        }

        [Test]
        public void PlayerFallFromRope()
        {
            var map = "  PRR  \nTTT  TT\n       ";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);

            game.UpdateMap(Direction.Right);
            game.UpdateMap(Direction.Down);
            game.UpdateMap();
            Assert.AreEqual(new Point(3, 2), FindPlayer(game.Map));
        }
    }

    [TestFixture]
    public class GameFinishingTest
    {
        [Test]
        public void GameFinishPlayerDead()
        {
            var map = "MP  B";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap();
            Assert.IsTrue(game.IsOver);
        }

        [Test]
        public void GameFinishNoBonus()
        {
            var map = " P ";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap();
            Assert.IsTrue(game.IsOver);
        }

        [Test]
        public void GameFinishPlayerHaveAllBonus()
        {
            var map = " PB ";
            var gameMap = new MapCreator(map).CreateMap();
            var game = new Game(gameMap);
            game.UpdateMap(Direction.Right);
            Assert.IsTrue(game.IsOver);
        }
    }
}