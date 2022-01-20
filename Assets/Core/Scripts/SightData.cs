using System.Collections.Generic;

namespace TilemapGridNavigation
{
    public struct SightData
    {
        public List<GridNode> nodesInRange;
        public List<GridNode> visibleNodes;

        public SightData(List<GridNode> nodesInRange, List<GridNode> visibleNodes)
        {
            this.nodesInRange = nodesInRange;
            this.visibleNodes = visibleNodes;
        }
    }
}
