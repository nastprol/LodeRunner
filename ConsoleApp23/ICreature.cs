using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    public interface ICreature
    {
        Point Act(int x, int y, GameMap gameMap = null, Direction direction = Direction.None);
    }
}
