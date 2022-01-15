using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapGridNavigation
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField, Tooltip("The Tilemap to spawn tiles on.")]
        private Tilemap highlightMap;
        
        [SerializeField, Tooltip("Highlight tile prefab to instantiate.")]
        private TileBase highlightTile;

        [SerializeField]
        private HighlightPaletteAsset palette;

        public void ClearHighlights()
        {
            highlightMap.ClearAllTiles();
        }

        public void HighlightNode(GridNode node, Color colourName)
        {
            if (!highlightMap.HasTile((Vector3Int)node.GridPos))
            {
                highlightMap.SetTile((Vector3Int)node.GridPos, highlightTile);
            }
            highlightMap.SetColor((Vector3Int)node.GridPos, colourName);
        }

        public void HighlightNode(GridNode node, string name)
        {
            if (palette == null)
            {
                Debug.LogError("Highlight palette has not been set");
                return;
            }

            Color colour = palette.GetColour(name);

            if (!highlightMap.HasTile((Vector3Int)node.GridPos))
            {
                highlightMap.SetTile((Vector3Int)node.GridPos, highlightTile);
            }
            highlightMap.SetColor((Vector3Int)node.GridPos, colour);
        }

        public void HighlightNodes(IEnumerable<GridNode> nodes, Color colour)
        {
            foreach (GridNode node in nodes) HighlightNode(node, colour);
        }

        public void HighlightNodes(IEnumerable<GridNode> nodes, string colourName)
        {
            foreach (GridNode node in nodes) HighlightNode(node, colourName);
        }
    }
}
