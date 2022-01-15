using System.Collections.Generic;

namespace TilemapGridNavigation
{
    public struct PathData
    {
        public Stack<GridNode> path;

        public PathData(Stack<GridNode> path)
        {
            this.path = path;
        }
    }
}