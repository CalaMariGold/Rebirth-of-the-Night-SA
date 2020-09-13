using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    public abstract class TerrainNode : Node
    {
        public delegate T Generator<out T>(Vector3Int position);
    }
}
