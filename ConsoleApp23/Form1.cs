using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyGame
{
    class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private Game game;
        private readonly Timer timer;
        private Label pos;
        private Label InputCommand;
        private Label RemainingCommand;
        public readonly int CellSize = 50;

        public GameForm()
        {
            var map = "MTPRLB ";
            game = new Game(new MapCreator(map).CreateMap());
            ClientSize = new Size(850, 600);
            timer = new Timer { Interval = 5 };
            DirectoryInfo imagesDirectory = new DirectoryInfo("images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            DoubleBuffered = true;
            timer.Tick += (sender, args) =>
            { Invalidate(); };
            timer.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (translate.ContainsKey(e.KeyCode))
            {
                game.UpdateMap(translate[e.KeyCode]);
            }
        }

        private readonly Dictionary<Keys, Direction> translate = new Dictionary<Keys, Direction>
        {
            {Keys.Up, Direction.Up},
            {Keys.Down, Direction.Down},
            {Keys.Left, Direction.Left},
            {Keys.Right, Direction.Right},
            {Keys.Space, Direction.Space},
            {Keys.None, Direction.None}
        };

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            for (var i = 0; i < game.MapWidth; i++)
                for (var j = 0; j < game.MapHeight; j++)
                {
                    var name = "";
                    if (game.Map[i, j] is Terrain)
                        name = "ladder";
                    else if (game.Map[i, j] is Ladder)
                        name = "ladder";
                    else if (game.Map[i, j] is Hole)
                        name = "hole";
                    else if (game.Map[i, j] is Player)
                        name = "player";
                    else if (game.Map[i, j] is Monster)
                        name = "monster";
                    else if (game.Map[i, j] is Rope)
                        name = "rope";
                    else if (game.Map[i, j] is Bonus)
                        name = "bonus";
                    else
                        name = "empty";
                    g.DrawImage(bitmaps[name + ".png"],
                        i * CellSize, j * CellSize + 50);
                }
        }
    }
}