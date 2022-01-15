using UnityEngine;

namespace TilemapGridNavigation
{
    public class GridUnit : MonoBehaviour, IGridEntity
    {
        public bool CanMoveThrough(IGridEntity other) => false;

        public bool CanSeeThrough(IGridEntity other)
        {
            if (other is GridUnit)
            {
                return other.Equals(this);
            }
            return true;
        }

        public bool CanStack(IGridEntity other) => other is not GridUnit;

        public bool InterruptsMovement(IGridEntity other) => false;
    }
}
