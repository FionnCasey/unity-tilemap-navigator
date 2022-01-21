# unity-tilemap-navigator
This project aims to provide easy-to-use A* pathfinding and line of sight detection for Unity's tilemap system.

## About
*In progress*

## Features
- Assets for adding navigation to existing tilemaps or creating new navigation tiles.
- 4 or 8 way navigation for isometric or rectangular grids.
- Grid based line of sight detection.
- Movement controller for traversing a grid.
- Grid highlighter component with custom colour palettes.

## Examples
**Isometric Pathfinding** - 4 Directions  
![Navigtaion - 4 Directions](https://media.giphy.com/media/PFLUaA6AO8JsQeoKsP/giphy.gif)

**Isometric Pathfinding** - 8 Directions  
![Navigtaion - 8 Directions](https://media.giphy.com/media/pXUqyqCmZXGybc36pQ/giphy.gif)

**Line of Sight Detection**  
![Line of Sight Detection](https://media.giphy.com/media/di41XOdbfdq9uvkzj3/giphy.gif)

## Setup Instructions
*In progress*

### Example Code
***GameManager.cs***  
```cs
using UnityEngine;
using UnityEngine.Tilemaps;
using TilemapGridNavigation;

public class GameManager : MonoBehaviour
{
    public Tilemap navMap; //Tilemap component using the provided NavTileAssets
    public Highlighter highlighter; //Highlighter component (optional)

    private NavGrid grid; //Stores info for all grid nodes.
    private Navigator navigator; //Pathfinding
    private Sightfinder sightfinder; //Line of sight detection

    private GridNode start; //Start node for path
    private GridNode end; //Destination node for path

    private void Start()
    {
        grid = new NavGrid(navMap); //NavGrid will read a tilemap to create pathfinding nodes
        navigator = new Navigator4(grid); //4 directional pathfinding (use Navigator8 for 8 directions)
        sightfinder = new Sightfinder(grid); //Line of sight detection
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridNode node = grid.GetNodeFromMousePos();

            if (node.CanMoveThrough()) SelectNode(node);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) ClearSelection();
    }

    private void SelectNode(GridNode node)
    {
        if (start == null)
        {
            start = node;
        }
        else if (end == null)
        {
            end = node;
            PathData pathData = navigator.GetPath(start, end);
            highlighter.HighlightNodes(pathData.path, Color.green);
        }
    }

    private void ClearSelection()
    {
        start = null;
        end = null;
        highlighter.ClearHighlights();
    }
}
```