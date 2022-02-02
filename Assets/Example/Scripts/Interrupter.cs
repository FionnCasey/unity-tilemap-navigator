using UnityEngine;

namespace TilemapGridNavigation
{
    public class Interrupter : MonoBehaviour, IGridEntity
    {
        public bool CanMoveThrough(IGridEntity other) => true;

        public bool CanSeeThrough(IGridEntity other) => true;

        public bool CanStack(IGridEntity other) => other is not Interrupter;

        public bool InterruptsMovement(IGridEntity other) => true;
    }
}
