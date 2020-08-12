using System.Runtime.InteropServices;
using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents data about a voxel
    /// to be passed to a compute shader.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VoxelComputeInfo
    {
        /// <summary>
        /// Gets or sets the distance from the surface of the terrain.
        /// </summary>
        public float Distance;
        
        /// <summary>
        /// Gets or sets the colour of the voxel.
        /// </summary>
        public Color Colour;
    }
}
