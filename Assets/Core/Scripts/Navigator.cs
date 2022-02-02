using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TilemapGridNavigation
{
    public abstract class Navigator
    {
        private const int STRAIGHT_MOVEMENT_COST = 10;
        private const int DIAGONAL_MOVE_COST = 14;

        protected readonly NavGrid grid;

        /// <summary>
        /// Returns a list of all nodes surrounding a given node.
        /// </summary>
        /// <param name="center">The center node.</param>
        /// <param name="entity">(Optional) The entity on the grid.</param>
        protected abstract List<GridNode> GetAdjacentNodes(GridNode center, IGridEntity entity = null);

        /// <summary>
        /// Creates a new instance using the given NavGrid.
        /// </summary>
        /// <param name="grid">The NavGrid to navigate.</param>
        public Navigator(NavGrid grid)
        {
            this.grid = grid;
        }

        /// <summary>
        /// Returns a list of all nodes within a certain distance of a given node.
        /// </summary>
        /// <param name="center">The center node.</param>
        /// <param name="distance">The maximum distance in nodes.</param>
        /// <param name="entity">(Optional) The entity on the grid.</param>
        public List<GridNode> GetReachableNodes(GridNode center, int distance, IGridEntity entity = null)
        {
            return grid.Nodes.Where(node =>
            {
                return node.CanMoveThrough(entity) 
                    && GridNode.GetDistance(center, node) <= distance
                    && GetPath(center, node, entity).Count <= distance;
            }).ToList();
        }

        /// <summary>
        /// Returns a stack of nodes representing the shortest path betweeen 2 nodes.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="end">The end node.</param>
        /// <param name="entity">(Optional) The grid entity traversing the grid.</param>
        public Stack<GridNode> GetPath(GridNode start, GridNode end, IGridEntity entity = null)
        {
            grid.ResetPathfinding();

            Stack<GridNode> path = new();
            HashSet<GridNode> openList = new();
            HashSet<GridNode> closedList = new();
            List<GridNode> adjacentNodes = new();
            GridNode current;

            start.G = 0;
            start.H = GridNode.GetDistance(start, end);
            start.F = start.H;
            openList.Add(start);

            while (openList.Count > 0)
            {
                current = openList.OrderBy(node => node.F).First();

                // End has been reached. No need to go further.
                if (current == end)
                {
                    // Loop back through parents of the current node to create a path.
                    while (current != start)
                    {
                        path.Push(current);
                        current = current.Parent;
                    }
                    return path;
                }

                adjacentNodes = GetAdjacentNodes(current, entity);

                // Evaluate the nearest nodes.
                foreach (GridNode node in adjacentNodes)
                {
                    // We've already looked at this node, so skip it.
                    if (closedList.Contains(node)) continue;

                    int g = current.G + GridNode.GetDistance(current, node);
                    
                    // If the current node is a better parent for this adjecent node, set it as the parent.
                    if (g < node.G)
                    {
                        node.G = g;
                        node.H = GridNode.GetDistance(node, end) * GetMovementCost(current, node);
                        node.F = node.G + node.H;
                        node.Parent = current;
                    }

                    // If we haven't looked at the adjacent node yet, add it to the open list.
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }

                // We've looked at this node, so remove it from the open list and add to the closed list.
                openList.Remove(current);
                closedList.Add(current);

            }
            // No path found, return empty stack.
            return path;
        }

        /// <summary>
        /// Returns true if an entity can move through the given node.
        /// </summary>
        /// <param name="node">The node to move through.</param>
        /// <param name="entity">The entity moving.</param>
        protected virtual bool CanMoveThroughNode(GridNode node, IGridEntity entity)
        {
            return node != null
                && node.CanMoveThrough()
                && (entity == null || node.CanMoveThrough(entity));
        }

        /// <summary>
        /// Returns the movement cost of moving to a node.
        /// </summary>
        /// <param name="from">The node we're coming from.</param>
        /// <param name="to">The node to calculate for.</param>
        protected int GetMovementCost(GridNode from, GridNode to)
        {
            int distanceX = GridNode.GetDistanceX(from, to);
            int distanceY = GridNode.GetDistanceY(from, to);
            int remaining = Mathf.Abs(distanceX - distanceY);
            return (DIAGONAL_MOVE_COST * Mathf.Min(distanceX, distanceY)) + (STRAIGHT_MOVEMENT_COST * remaining);
        }
    }
}
