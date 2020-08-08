namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space using a 3D array.
    /// </summary>
    public class ArrayChunk : IChunk
    {
        private readonly int _xOffset;
        private readonly int _yOffset;
        private readonly int _zOffset;
        private readonly VoxelInfo[,,] _voxelData;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="ArrayChunk"/> class.
        /// </summary>
        /// <param name="width">The width of the chunk.</param>
        /// <param name="height">The height of the chunk.</param>
        /// <param name="depth">The depth of the chunk.</param>
        /// <param name="xOffset">The offset of the chunk in the x-axis.</param>
        /// <param name="yOffset">The offset of the chunk in the y-axis.</param>
        /// <param name="zOffset">The offset of the chunk in the z-axis.</param>
        public ArrayChunk(int width, int height, int depth,
            int xOffset, int yOffset, int zOffset)
        {
            // Set the bounding size of the chunk
            Width = width;
            Height = height;
            Depth = depth;
            // Set the offset in voxel space of the chunk
            _xOffset = xOffset;
            _yOffset = yOffset;
            _zOffset = zOffset;
            // Initialise the array holding voxel data
            _voxelData = new VoxelInfo[width,height,depth];
        }

        /// <summary>
        /// Gets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        public VoxelInfo this[int x, int y, int z] => _voxelData[x, y, z];
        /// <summary>
        /// Gets the width of the chunk in the x-axis.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Gets the height of the chunk in the y-axis.
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Gets the depth of the chunk in the z-axis.
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Fills the data array with voxels from a provider.
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
                        _voxelData[x, y, z] = voxelProvider.GetVoxelInfo(
                            x + _xOffset,
                            y + _yOffset,
                            z + _zOffset
                            );
                    }
                }
            }
        }
    }
}
