using System.Collections.Generic;

namespace TilemapGridNavigation
{
    public struct PathData
    {
        public Stack<GridNode> path;
        public bool isReachable;

        public PathData(Stack<GridNode> path, bool isReachable = true)
        {
            this.path = path;
            this.isReachable = isReachable;
        }
    }
}