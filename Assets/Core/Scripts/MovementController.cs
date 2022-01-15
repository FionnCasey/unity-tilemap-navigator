using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TilemapGridNavigation
{
    [RequireComponent(typeof(IGridEntity))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField, Tooltip("The time it takes to move 1 node.")]
        private float moveDuration = 0.15f;

        public IGridEntity Entity { get; private set; }

        /// <summary>
        /// The grid node this unit is currently occupying.
        /// </summary>
        public GridNode CurrentNode { get; private set; }

        /// <summary>
        /// Returns true if this unit is currently moving.
        /// </summary>
        public bool IsMoving { get; private set; }

        public void Awake()
        {
            Entity = GetComponent<IGridEntity>();
        }

        /// <summary>
        /// Returns the world position of the given node while preserving the current z postion.
        /// </summary>
        /// <param name="node">Grid node to get position of.</param>
        private Vector3 GetNodeAnchorPosition(GridNode node) => new(node.WorldPos.x, node.WorldPos.y, transform.position.z);

        /// <summary>
        /// Async task to move this object along a given path.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        /// <returns>Task that can be awaited.</returns>
        public async Task<MoveData> Move(Stack<GridNode> path)
        {
            if (IsMoving) return new(CurrentNode, 0);

            IsMoving = true;
            GridNode currentNode = CurrentNode;
            int distance = 0;

            while (path.Count > 0)
            {
                if (currentNode.InterruptsMovement(Entity))
                {
                    return new(currentNode, distance, true);
                }

                GridNode nextNode = path.Pop();
                float elapsedTime = 0;
                distance++;

                while (elapsedTime < moveDuration)
                {
                    transform.position = Vector3.Lerp(GetNodeAnchorPosition(currentNode), GetNodeAnchorPosition(nextNode), elapsedTime / moveDuration);
                    elapsedTime += Time.deltaTime;
                    await Task.Yield();
                }
                currentNode = nextNode;
            }
            IsMoving = false;
            SetGridNode(currentNode);

            return new(currentNode, distance);
        }

        /// <summary>
        /// Sets this unit at the given grid node in world space and grid space.
        /// </summary>
        /// <remarks>
        /// Will also set the given node's current unit to this.
        /// </remarks>
        /// <param name="node">The grid node to set to.</param>
        public void SetGridNode(GridNode node)
        {
            if (CurrentNode != null)
            {
                CurrentNode.RemoveContent(Entity);
            }
            CurrentNode = node;
            CurrentNode.AddContent(Entity);
            transform.position = new(node.WorldPos.x, node.WorldPos.y, transform.position.z);
        }
    }
}
