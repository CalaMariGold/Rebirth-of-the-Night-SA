using Rebirth.Terrain.Octree;
using Rebirth.Terrain.Voxel;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space using a Dense Octree.
    /// </summary>
    public class OctreeChunk : IChunk
    {
        private readonly int _xOffset;
        private readonly int _yOffset;
        private readonly int _zOffset;
        private readonly DenseOctree<VoxelInfo> _voxelData;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctreeChunk"/> class.
        /// </summary>
        /// <param name="subdivisions">The number of subdivisions in the octree.</param>
        /// <param name="xOffset">The offset of the chunk in the x-axis.</param>
        /// <param name="yOffset">The offset of the chunk in the y-axis.</param>
        /// <param name="zOffset">The offset of the chunk in the z-axis.</param>
        public OctreeChunk(int subdivisions, int xOffset, int yOffset, int zOffset)
        {
            // Set the offset in voxel space of the chunk
            _xOffset = xOffset;
            _yOffset = yOffset;
            _zOffset = zOffset;
            // Initialise the octree holding voxel data
            _voxelData = new DenseOctree<VoxelInfo>(subdivisions);
        }
        
        /// <summary>
        /// Gets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        public VoxelInfo this[int x, int y, int z] => _voxelData[x, y, z];
        /// <summary>
        /// Gets the width of the chunk in the x-axis.
        /// </summary>
        public int Width => 1 << _voxelData.Subdivisions;
        /// <summary>
        /// Gets the height of the chunk in the y-axis.
        /// </summary>
        public int Height => 1 << _voxelData.Subdivisions;
        /// <summary>
        /// Gets the depth of the chunk in the z-axis.
        /// </summary>
        public int Depth => 1 << _voxelData.Subdivisions;
        
        /// <summary>
        /// Fills the data octree with voxels from a provider.
        /// </summary>
        /// <param name="voxelProvider">The object which provides voxel info.</param>
        public void Load(IVoxelProvider voxelProvider)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var z = 0; z < Depth; z++)
                    {
                        _voxelData.SetByIndex(
                            x, y, z,
                            voxelProvider.GetVoxelInfo(
                                x + _xOffset,
                                y + _yOffset,
                                z + _zOffset
                            ),
                            true
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Returns a 1-D array containing the voxel Distance data in the chunk's 3-D data array.
        /// Indexing is determined with the following mapping:
        /// <code><![CDATA[
        ///    index(x, y, z) = z << (chunkWidthBits + chunkHeightBits) | y << chunkWidthBits | x
        /// ]]></code>
        /// where (x, y, z) are local data array indices.
        /// </summary>
        public VoxelComputeInfo[] CalcDistanceArray()
        {
            var chunkData = new VoxelComputeInfo[Width * Height * Depth];
            // TODO: iterate octree recursively to improve efficiency
            // Currently O(n log n) but could be improved to O(n)
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var z = 0; z < Depth; z++)
                    {
                        var index = z << (_voxelData.Subdivisions * 2) | y << _voxelData.Subdivisions | x;
                        chunkData[index] = _voxelData[x, y, z].ToCompute();
                    }
                }
            }
            return chunkData;
        }
    }
}
