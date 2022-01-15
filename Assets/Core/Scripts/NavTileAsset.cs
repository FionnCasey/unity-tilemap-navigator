using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapGridNavigation
{
    [System.Flags]
    public enum NavFlags
    {
        None = 0,
        BlocksVision = 1 << 1,
        BlocksMovement = 2 << 2,
        All = BlocksVision | BlocksMovement
    }

    [CreateAssetMenu(fileName = "NavTile", menuName = "TilemapGridNav/NavTile")]
    public class NavTileAsset : Tile
    {
        [Tooltip("Used to determine which nodes can be seen through or walked through on a grid.")]
        public NavFlags navFlags;
    }
}
