using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapGridNavigation.Example
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Tilemap navMap;
        [SerializeField]
        private GridUnit unitPrefab;

        private NavGrid grid;
        private Navigator navigator;
        private Highlighter highlighter;
        private Transform unitContainer;
        private List<MovementController> controllers = new();
        private int index = -1;

        private void Start()
        {
            grid = new(navMap);
            navigator = new Navigator4(grid);
            highlighter = FindObjectOfType<Highlighter>();
            unitContainer = GameObject.Find("UnitContainer").transform;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveCurrentUnit();
            } 
            else if (Input.GetMouseButtonDown(1))
            {
                SpawnUnit();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleNavigator();
            }
            else
            {
                HandleKeyInput();
            }
        }

        private async void MoveCurrentUnit()
        {
            if (index == -1 || index >= controllers.Count) return;
            if (controllers[index].IsMoving) return;

            GridNode node = grid.GetNodeFromMousePos();

            if (!node.CanMoveThrough(controllers[index].Entity)) return;

            PathData pathData = navigator.GetPath(controllers[index].CurrentNode, node, controllers[index].Entity);
            highlighter.HighlightNodes(pathData.path, "Path");

            await controllers[index].Move(pathData.path);

            highlighter.ClearHighlights();
        }

        private void SpawnUnit()
        {
            GridNode node = grid.GetNodeFromMousePos();

            if (!node.CanMoveThrough() || node.ContainsEntityType<GridUnit>()) return;

            GridUnit unit = Instantiate(unitPrefab, unitContainer);
            MovementController controller = unit.GetComponent<MovementController>();
            controller.SetGridNode(node);
            controllers.Add(controller);

            if (index == -1)
            {
                ChangeUnit(0);
            }
        }

        private void ToggleNavigator()
        {
            if (navigator is Navigator4)
            {
                navigator = new Navigator8(grid);
            }
            else
            {
                navigator = new Navigator4(grid);
            }
            highlighter.ClearHighlights();
        }

        private void HandleKeyInput()
        {
            var input = Input.inputString;

            if (string.IsNullOrEmpty(input)) return;

            if (int.TryParse(input, out int key) && key > 0 && key - 1 < controllers.Count)
            {
                ChangeUnit(key - 1);
            }
        }

        private void ChangeUnit(int nextIndex)
        {
            if (index > -1)
            {
                controllers[index].GetComponentInChildren<SpriteRenderer>().color = new Color(0.2227216f, 0.5814894f, 0.8584906f);
            }
            index = nextIndex;
            controllers[index].GetComponentInChildren<SpriteRenderer>().color = new Color(0.7830189f, 0.3162252f, 0.1735938f);
        }
    }
}
