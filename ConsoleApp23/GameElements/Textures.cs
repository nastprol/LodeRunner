using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    public class Textures : ICreature
    {
        Point ICreature.Act(int x, int y, GameMap gameMap, Direction keyPress) => new Point(x, y);
    }

    public class Ladder : Textures { }
    public class Rope : Textures { }
    public class Hole : Textures { }
    public class Terrain : Textures { }
    public class Bonus : Textures { }
}

