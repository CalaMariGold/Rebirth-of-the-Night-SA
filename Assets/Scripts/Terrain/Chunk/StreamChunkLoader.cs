using System;
using System.IO;
using System.Text;
using Rebirth.Terrain.Voxel;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Provides voxel data to a chunk from a stream.
    /// </summary>
    public class StreamChunkLoader : IChunkLoader, IDisposable
    {

        private readonly BinaryReader _reader;
        private readonly Func<int, IVoxelType> _voxelTypeProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="StreamChunkLoader"/> class.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <param name="voxelTypeProvider">A delegate to look up voxel types by ID.</param>
        /// <param name="leaveOpen">
        /// Specifies whether to leave open the stream after the <see cref="StreamChunkLoader"/> is disposed.
        /// </param>
        public StreamChunkLoader(Stream stream,
            Func<int, IVoxelType> voxelTypeProvider, bool leaveOpen = false)
        {
            _reader = new BinaryReader(stream, Encoding.Default, leaveOpen);
            _voxelTypeProvider = voxelTypeProvider;
        }

        /// <summary>
        /// Load a chunk with data.
        /// </summary>
        /// <param name="chunk">The chunk to fill.</param>
        public void Load(IChunk chunk)
        {
            chunk.Deserialize(_reader, _voxelTypeProvider);
        }
        
        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
