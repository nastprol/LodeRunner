//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Drawing;


//namespace MyGame
//{
//    public enum Creature
//    {
//        Empty,
//        Terrain,
//        Ladder,
//        Rope,
//        Monster,
//        Player,
//        Bonus,
//        Hole
//    }

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Console version");
//            //var map = "     \nM    \nTT  L\n  TBP";
//            //var map = "PTMLB";
//            //var map = " PB ";
//            //var map = "B    MB   H  P    ";
//            //var map = "   B   BP";
//            //var map = "     \nP    \nTT  L\n  TB ";
//            //var map = "PL\nTL\nBL";
//            //var map = "TL     \nTLTTTTT\n L L M \n L LTT \n L L   \nTLTLT T\n       \nP  L  B\nTTTTTTT";
//            //var map = "L  B\nLP  ";
//            //var map = " PB B";
//            //var map = "B PRR  \nTTT  TT\n       ";
//            //var mapString = "MB PRR  \nTTT   TT\n        ";
//            //var mapString = "L  B\nLP  ";
//            //var mapString = "B PRR  \nTTT  TT";
//            var mapString = "L  B\nLP  ";



//            var keys = new Dictionary<string, Direction>()
//            {
//                { "r" , Direction.Right }, { "l" ,Direction.Left },
//                { "u", Direction.Up }, {"d", Direction.Down },
//                { "s", Direction.Space }, {"n", Direction.None }
//            };


//            var mapCreator = new MapCreator(mapString);

//            var map = mapCreator.CreateMap();


//            var game = new Game(map);
//            Console.WriteLine("Game is running");
//            PrintMap(game);

//            while (true)
//            {
//                Console.WriteLine("make next move. enter r, l, d, u, n or s");
//                var move = Console.ReadLine();
//                game.UpdateMap(keys[move]);

//                PrintMap(game);

//                if (game.IsOver)
//                {
//                    Console.WriteLine("Game over");
//                    if (game.PlayerWon)
//                        Console.WriteLine("You win");
//                    break;
//                }
//            }
//        }
//        private static void PrintMap(Game game)
//        {
//            var creatures = new Dictionary<Creature, char>()
//            {
//                { Creature.Terrain, 'T' }, { Creature.Ladder, 'L' }, { Creature.Hole, 'H' },
//                { Creature.Player, 'P' }, { Creature.Monster, 'M' }, { Creature.Bonus, 'B' },
//                { Creature.Rope, 'R' }, { Creature.Empty, '.' }
//            };
//            for (int x = 0; x < game.MapHeight; x++)
//            {
//                for (int y = 0; y < game.MapWidth; y++)
//                {
//                    if (game.Map[y, x] is Terrain)
//                        Console.Write('T');
//                    else if (game.Map[y, x] is Ladder)
//                        Console.Write('L');
//                    else if (game.Map[y, x] is Hole)
//                        Console.Write('H');
//                    else if (game.Map[y, x] is Player)
//                        Console.Write('P');
//                    else if (game.Map[y, x] is Monster)
//                        Console.Write('M');
//                    else if (game.Map[y, x] is Rope)
//                        Console.Write('R');
//                    else if (game.Map[y, x] is Bonus)
//                        Console.Write('B');
//                    else
//                        Console.Write('.');
//                    //var toPrint = creatures[game.IdentifyCreature(x, y)];
//                    //Console.Write(toPrint);
//                }
//                Console.WriteLine();
//            }
//        }
//    }
//}







using System;
using System.Windows.Forms;

namespace MyGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //Game.CreateMap();
            Application.Run(new GameForm());
        }
    }
}