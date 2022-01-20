namespace TilemapGridNavigation
{
    public enum SightType
    {
        Standard,
        Linear,
        Diagnonal
    }

    public struct SightInput
    {
        public GridNode center;
        public IGridEntity entity;
        public SightType sightType;
        public int minRange;
        public int maxRange;
        public bool requiresLOS;

        public SightInput(GridNode center, SightType sightType, int minRange, int maxRange, IGridEntity entity = null, bool requiresLOS = true)
        {
            this.center = center;
            this.entity = entity;
            this.sightType = sightType;
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.requiresLOS = requiresLOS;
        }
    }
}
