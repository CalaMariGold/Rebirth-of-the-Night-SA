using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
    public interface IPreviewNode
    {
        Texture2D Generate();
    }
}
