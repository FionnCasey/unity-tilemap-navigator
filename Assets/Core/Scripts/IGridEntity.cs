namespace TilemapGridNavigation
{
    public interface IGridEntity
    {
        /// <summary>
        /// Returns true if this entity does not prevent a given entity from being added to a node.
        /// </summary>
        /// <param name="other">The other entity.</param>
        public bool CanStack(IGridEntity other);

        /// <summary>
        /// Returns true if this entity does not prevent a given entity from moving through a node.
        /// </summary>
        /// <param name="other">The other entity.</param>
        public bool CanMoveThrough(IGridEntity other);

        /// <summary>
        /// Returns true if this entity does not prevent a given entity from seeing through a node.
        /// </summary>
        /// <param name="other">The other entity.</param>
        public bool CanSeeThrough(IGridEntity other);

        /// <summary>
        /// Returns true if this entity should stop the given entity when it moves onto a node.
        /// </summary>
        /// <param name="other">The other entity.</param>
        public bool InterruptsMovement(IGridEntity other);
    }
}
