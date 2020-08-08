using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    /// <summary>
    /// Provides Unity mesh objects for voxel-based terrain chunks.
    /// </summary>
    public interface IMeshGenerator
    {
        /// <summary>
        /// Generates a mesh from the data in an <seealso cref="IChunk"/>.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        /// <returns>A Unity mesh which can be added to a scene.</returns>
        Mesh GenerateMesh(IChunk chunk);
    }
}
