using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapGridNavigation
{
    /// <summary>
    /// Class to generate and store grid data.
    /// </summary>
    public class NavGrid
    {
        private readonly Tilemap navMap;
        private readonly Camera mainCam;

        private Dictionary<Vector2Int, GridNode> grid;   

        /// <summary>
        /// A list of all grid nodes.
        /// </summary>
        public List<GridNode> Nodes => grid.Values.ToList();

        /// <summary>
        /// Create a new instance using a Tilemap of NavTileAssets.
        /// </summary>
        /// <param name="navMap">Tilemap of NavTileAssets.</param>
        /// <param name="hideNavMapOnInit">Optional: If true, will disable the nav map game object on loading (default: true).</param>
        public NavGrid(Tilemap navMap, bool hideNavMapOnInit = true)
        {
            // Cache reference to main camera for performance.
            // May not be necessary in future Unity versions.
            mainCam = Camera.main;
            this.navMap = navMap;
            GenerateGrid(); 

            if (hideNavMapOnInit)
            {
                navMap.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Generates a grid of GridNodes using the current Tilemap.
        /// </summary>
        private void GenerateGrid()
        {
            grid = new();
            BoundsInt bounds = navMap.cellBounds;
            TileBase[] tiles = navMap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    NavTileAsset tile = (NavTileAsset)tiles[x + y * bounds.size.x];

                    if (!tile) continue;

                    Vector2Int gridPos = new(bounds.position.x + x, bounds.position.y + y);
                    Vector3 worldPos = navMap.GetCellCenterWorld((Vector3Int)gridPos);
                    GridNode node = new(tile, gridPos, worldPos);
                    grid.Add(gridPos, node);
                }
            }
        }

        /// <summary>
        /// Resets G, H and F values for all nodes.
        /// </summary>
        public void ResetPathfinding()
        {
            foreach (var node in grid.Values)
            {
                node.ResetPathfinding();
            }
        }

        /// <summary>
        /// Returns true if a node exists at the given grid position.
        /// </summary>
        /// <param name="gridPos">The grid position to check.</param>
        public bool NodeExists(Vector2Int gridPos) => grid.ContainsKey(gridPos);

        /// <summary>
        /// Returns the grid node at the current mouse position.
        /// </summary>
        /// <remarks>
        /// Retuns null if no node is available.
        /// </remarks>
        public GridNode GetNodeFromMousePos()
        {
            Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            return GetNodeFromWorldPos(new(worldPos.x, worldPos.y, 0));
        }

        /// <summary>
        /// Returns the grid node at the given world position.
        /// </summary>
        /// <remarks>
        /// Retuns null if no node is available.
        /// </remarks>
        public GridNode GetNodeFromWorldPos(Vector3 worldPos)
        {
            Vector3Int gridPos = navMap.WorldToCell(worldPos);
            return GetNodeFromGridPos((Vector2Int)gridPos);
        }

        /// <summary>
        /// Returns the grid node at the given grid position.
        /// </summary>
        /// <remarks>
        /// Retuns null if no node is available.
        /// </remarks>
        public GridNode GetNodeFromGridPos(Vector2Int gridPos)
        {
            if (NodeExists(gridPos)) return grid[gridPos];
            return null;
        }
    }
}
