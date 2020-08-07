namespace Rebirth.Terrain
{
    /// <summary>
    /// Represents a chunk of voxels in space.
    /// </summary>
    public class Chunk
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _depth;
        private readonly int _xOffset;
        private readonly int _yOffset;
        private readonly int _zOffset;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="Chunk"/> class.
        /// </summary>
        /// <param name="width">The width of the chunk.</param>
        /// <param name="height">The height of the chunk.</param>
        /// <param name="depth">The depth of the chunk.</param>
        /// <param name="xOffset">The offset of the chunk in the x-axis.</param>
        /// <param name="yOffset">The offset of the chunk in the y-axis.</param>
        /// <param name="zOffset">The offset of the chunk in the z-axis.</param>
        public Chunk(int width, int height, int depth,
            int xOffset, int yOffset, int zOffset)
        {
            // Set the bounding size of the chunk
            _width = width;
            _height = height;
            _depth = depth;
            // Set the offset in voxel space of the chunk
            _xOffset = xOffset;
            _yOffset = yOffset;
            _zOffset = zOffset;
            // Initialise the array holding voxel data
            VoxelData = new VoxelInfo[width,height,depth];
        }

        /// <summary>
        /// Gets the array of <see cref="VoxelInfo"/> holding the data for the chunk.
        /// </summary>
        public VoxelInfo[,,] VoxelData { get; }

        /// <summary>
        /// Fills the data array with voxels from a provider.
        /// </summary>
        /// <param name="voxelProvider">The object which provides voxel info.</param>
        public void Load(IVoxelProvider voxelProvider)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    for (var z = 0; z < _depth; z++)
                    {
                        VoxelData[x, y, z] = voxelProvider.GetVoxelInfo(
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
