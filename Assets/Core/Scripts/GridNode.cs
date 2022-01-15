using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TilemapGridNavigation
{
    /// <summary>
    /// Data class for storing information about individual nodes.
    /// </summary>
    public class GridNode
    {
        private readonly NavFlags navFlags;

        private List<IGridEntity> content;

        /// <summary>
        /// This node's position in the grid.
        /// </summary>
        public Vector2Int GridPos { get; }

        /// <summary>
        /// This node's position in the world.
        /// </summary>
        public Vector3 WorldPos { get; }

        public List<IGridEntity> Content => content;

        public int G { get; set; } = int.MaxValue;
        public int H { get; set; } = int.MaxValue;
        public int F { get; set; } = int.MaxValue;
        public GridNode Parent { get; set; } = null;

        public GridNode(NavTileAsset navTile, Vector2Int gridPos, Vector3 worldPos)
        {
            navFlags = navTile.navFlags;
            GridPos = gridPos;
            WorldPos = worldPos;
            content = new();
        }

        /// <summary>
        /// Returns the distance in grid units between a and b.
        /// </summary>
        public static int GetDistance(GridNode a, GridNode b) => GetDistanceX(a, b) + GetDistanceY(a, b);

        /// <summary>
        /// Returns the X distance in grid units between a and b.
        /// </summary>
        public static int GetDistanceX(GridNode a, GridNode b) => Mathf.Abs(a.GridPos.x - b.GridPos.x);

        /// <summary>
        /// Returns the Y distance in grid units between a and b.
        /// </summary>
        public static int GetDistanceY(GridNode a, GridNode b) => Mathf.Abs(a.GridPos.y - b.GridPos.y);

        /// <summary>
        /// Resets pathfinding properties.
        /// </summary>
        public void ResetPathfinding()
        {
            G = int.MaxValue;
            H = int.MaxValue;
            F = int.MaxValue;
            Parent = null;
        }

        /// <summary>
        /// Returns true if any content entities are of type T.
        /// </summary>
        public bool ContainsEntityType<T>()
        {
            return content.Any(entity => entity is T);
        }

        public bool CanStack(IGridEntity entity)
        {
            return content.All(e => e.CanStack(entity));
        }

        public void AddContent(IGridEntity entity)
        {
            if (CanStack(entity))
            {
                content.Add(entity);
            }
        }

        public void RemoveContent(IGridEntity entity) => content.Remove(entity);

        /// <summary>
        /// Returns true if this node doesn't block movement.
        /// </summary>
        public virtual bool CanMoveThrough() => !navFlags.HasFlag(NavFlags.BlocksMovement);

        /// <summary>
        /// Returns true if this node doesn't block movement for a given entity.
        /// </summary>
        /// <param name="entity">The entity moving.</param>
        public virtual bool CanMoveThrough(IGridEntity entity)
        {
            return CanMoveThrough() && content.All(e => e.CanMoveThrough(entity));
        }

        /// <summary>
        /// Returns true if this node doesn't block line of sight.
        /// </summary>
        public virtual bool CanSeeThrough() => !navFlags.HasFlag(NavFlags.BlocksVision);

        /// <summary>
        /// Returns true if this node doesn't block movement for a given entity.
        /// </summary>
        /// <param name="entity">The entity looking.</param>
        public virtual bool CanSeeThrough(IGridEntity entity)
        {
            return CanSeeThrough() && content.All(e => e.CanSeeThrough(entity));
        }

        /// <summary>
        /// Returns true if this node interrupts movement for a given entity.
        /// </summary>
        /// <param name="entity">The entity looking.</param>
        public virtual bool InterruptsMovement(IGridEntity entity)
        {
            return content.Any(e => e.InterruptsMovement(entity));
        }
    }
}