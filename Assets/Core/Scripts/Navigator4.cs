using System.Collections.Generic;
using UnityEngine;

namespace TilemapGridNavigation
{
    public class Navigator4 : Navigator
    {
        public Navigator4(NavGrid grid) : base(grid)
        {
        }

        protected override List<GridNode> GetAdjacentNodes(GridNode center, IGridEntity entity)
        {
            List<GridNode> nodes = new();
            Vector2Int nextPos = new(0, 0);
            GridNode current;

            for (int x = -1; x <= 1; x++)
            {
                if (x == 0) continue;

                nextPos = new(center.GridPos.x + x, center.GridPos.y);
                current = grid.GetNodeFromGridPos(nextPos);

                if (!CanMoveThroughNode(current, entity)) continue;

                nodes.Add(current);
            }

            for (int y = -1; y <= 1; y++)
            {
                if (y == 0) continue;

                nextPos = new(center.GridPos.x, center.GridPos.y + y);
                current = grid.GetNodeFromGridPos(nextPos);

                if (!CanMoveThroughNode(current, entity)) continue;

                nodes.Add(current);
            }
            return nodes;
        }
    }
}
