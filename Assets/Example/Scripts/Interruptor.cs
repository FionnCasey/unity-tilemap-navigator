using UnityEngine;

namespace TilemapGridNavigation
{
    public class Interruptor : MonoBehaviour, IGridEntity
    {
        public bool CanMoveThrough(IGridEntity other) => true;

        public bool CanSeeThrough(IGridEntity other) => true;

        public bool CanStack(IGridEntity other) => other is not Interruptor;

        public bool InterruptsMovement(IGridEntity other) => true;
    }
}
