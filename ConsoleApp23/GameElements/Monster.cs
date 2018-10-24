using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{

    public class Monster : ICreature
    {
        private int direction = 1;
        Point ICreature.Act(int x, int y, GameMap gameMap, Direction keyPress)
        {
            var map = gameMap.Map;
            var path = GetPathToPlayer(x, y, gameMap);
            if (path == null)
            {
                if (gameMap.InBounds(x + direction, y)
                    && !(map[x + direction, y] is Terrain))
                {
                    return new Point(x + direction, y);
                }
                else if (gameMap.InBounds(x - direction, y)
                    && !(map[x - direction, y] is Terrain))
                {
                    direction = -direction;
                    return new Point(x + direction, y);
                }
                else return new Point(x, y);
            }
            return path.Skip(1).First();
        }

        public List<Point> GetPathToPlayer(int x, int y, GameMap gameMap)
        {
            var map = gameMap.Map;
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(new Point(x, y)));
            var visited = new HashSet<Point>();

            while (queue.Count != 0)
            {
                var currentList = queue.Dequeue();
                var currentPoint = currentList.Value;
                if (map[currentPoint.X, currentPoint.Y] == gameMap.Player)
                    return currentList.ToList();
                TryEnqueue(currentList, 1, 0, queue, visited, gameMap);
                TryEnqueue(currentList, -1, 0, queue, visited, gameMap);
            }
            return null;
        }
        private void TryEnqueue(SinglyLinkedList<Point> currentList, int dx, int dy,
            Queue<SinglyLinkedList<Point>> queue, HashSet<Point> visited, GameMap map)
        {
            var currentPoint = currentList.Value;
            var nextPoint = new Point(currentPoint.X + dx, currentPoint.Y + dy);
            var nextList = new SinglyLinkedList<Point>(nextPoint, currentList);

            if (!ShouldBeVisited(nextPoint, map, currentPoint, visited))
                return;

            visited.Add(nextPoint);
            queue.Enqueue(nextList);
        }

        private bool ShouldBeVisited(Point point, GameMap gameMap, Point fromPoint, HashSet<Point> visited)
        {
            var map = gameMap.Map;
            if (!gameMap.InBounds(point))
                return false;
            if (visited.Contains(point)
                || map[point.X, point.Y] is Terrain)
                return false;
            var dx = fromPoint.X - point.X;
            if (Math.Abs(dx) == 1)
                return true;
            var dy = fromPoint.Y - point.Y;
            if (dy == -1)
                return true;
            return false;
        }
    }

    public class SinglyLinkedList<T>
    {
        public readonly T Value;
        public readonly SinglyLinkedList<T> Previous;
        public readonly int Length;

        public SinglyLinkedList(T value, SinglyLinkedList<T> previous = null)
        {
            Value = value;
            Previous = previous;
            Length = previous?.Length + 1 ?? 1;
        }
        public List<T> ToList()
        {
            var result = new List<T> { Value };
            var pathItem = Previous;
            while (pathItem != null)
            {
                result.Add(pathItem.Value);
                pathItem = pathItem.Previous;
            }
            result.Reverse();
            return result;
        }
    }
}

