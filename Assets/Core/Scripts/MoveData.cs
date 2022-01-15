namespace TilemapGridNavigation
{
    public struct MoveData
    {
        public GridNode endNode;
        public int distanceTravelled;
        public bool wasInterrupted;

        public MoveData(GridNode endNode, int distanceTravelled, bool wasInterrupted = false)
        {
            this.endNode = endNode;
            this.distanceTravelled = distanceTravelled;
            this.wasInterrupted = wasInterrupted;
        }
    }
}
