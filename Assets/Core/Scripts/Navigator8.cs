using System.Collections.Generic;
using UnityEngine;

namespace TilemapGridNavigation
{
    public class Navigator8 : Navigator
    {
        public Navigator8(NavGrid grid) : base(grid)
        {
        }

        protected override List<GridNode> GetAdjacentNodes(GridNode center, IGridEntity entity)
        {
            List<GridNode> nodes = new();
            Vector2Int nextPos = new(0, 0);
            GridNode current;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    nextPos = new(center.GridPos.x + x, center.GridPos.y + y);
                    current = grid.GetNodeFromGridPos(nextPos);

                    if (!CanMoveThroughNode(current, entity)) continue;

                    nodes.Add(current);
                }
            }
            return nodes;
        }
    }
}
