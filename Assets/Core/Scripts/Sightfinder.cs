using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TilemapGridNavigation
{
    public class Sightfinder
    {
        private NavGrid grid;

        public Sightfinder(NavGrid grid)
        {
            this.grid = grid;
        }

        // Source: https://www.redblobgames.com/grids/line-drawing.html
        private static List<Vector2Int> GetLinePoints(Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> points = new List<Vector2Int>();
            int dx = end.x - start.x;
            int dy = end.y - start.y;
            int nx = Mathf.Abs(dx);
            int ny = Mathf.Abs(dy);
            int signX = dx > 0 ? 1 : -1;
            int signY = dy > 0 ? 1 : -1;

            Vector2Int p = new Vector2Int(start.x, start.y);

            for (int ix = 0, iy = 0; ix < nx || iy < ny;)
            {
                float decision = (1 + 2 * ix) * ny - (1 + 2 * iy) * nx;
                if (decision == 0)
                {
                    // next step is diagonal
                    p.x += signX;
                    p.y += signY;
                    ix++;
                    iy++;
                }
                else if (decision < 0)
                {
                    // next step is horizontal
                    p.x += signX;
                    ix++;
                }
                else
                {
                    // next step is vertical
                    p.y += signY;
                    iy++;
                }
                points.Add(p);
            }
            return points;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public SightData GetVisibleNodes(SightInput input)
        {
            List<GridNode> nodesInRange = grid.Nodes.Where(node =>
            {
                if (!node.CanSeeThrough()) return false;

                int distance = GridNode.GetDistance(node, input.center);
                return distance >= input.minRange && distance <= input.maxRange;
            }).ToList();

            if (!input.requiresLOS)
            {
                return new(nodesInRange, nodesInRange);
            }

            List<GridNode> visibleNodes = nodesInRange.Where(node =>
            {
                List<Vector2Int> line = GetLinePoints(input.center.GridPos, node.GridPos);

                foreach (Vector2Int pos in line)
                {
                    // If there's a unit at the end of the line, it should still be visible.
                    if (pos == line[^1]) continue;

                    if (!grid.NodeExists(pos)) return false;

                    GridNode lineNode = grid.GetNodeFromGridPos(pos);

                    // If a node on the line can't be seen through, the end node isn't visible.
                    if ((input.entity != null && !lineNode.CanSeeThrough(input.entity)) || !lineNode.CanSeeThrough()) return false;
                }
                return true;
            }).ToList();

            return new(nodesInRange, visibleNodes);
        }
    }
}
