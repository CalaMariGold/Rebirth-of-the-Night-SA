using UnityEngine;

namespace Rebirth.Terrain
{
    /// <summary>
    /// Provides Unity mesh objects for voxel-based terrain chunks.
    /// </summary>
    public interface IMeshGenerator
    {
        /// <summary>
        /// Generates a mesh from the data in a <seealso cref="Chunk"/>.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        /// <returns>A Unity mesh which can be added to a scene.</returns>
        Mesh GenerateMesh(Chunk chunk);
    }
}
