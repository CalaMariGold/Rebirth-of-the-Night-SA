using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Rebirth.Terrain.Octree;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space using a Dense Octree.
    /// </summary>
    public class OctreeChunk : IChunk
    {
        private readonly DenseOctree<VoxelInfo> _voxelData;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctreeChunk"/> class.
        /// </summary>
        /// <param name="subdivisions">The number of subdivisions in the octree.</param>
        /// <param name="offsetX">The offset of the chunk in the x-axis.</param>
        /// <param name="offsetY">The offset of the chunk in the y-axis.</param>
        /// <param name="offsetZ">The offset of the chunk in the z-axis.</param>
        public OctreeChunk(int subdivisions, int offsetX, int offsetY, int offsetZ)
        {
            // Set the offset in voxel space of the chunk
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
            // Initialise the octree holding voxel data
            _voxelData = new DenseOctree<VoxelInfo>(subdivisions);
        }
        
        /// <summary>
        /// Gets or sets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        public VoxelInfo this[int x, int y, int z]
        {
            get => _voxelData[x, y, z];
            set => _voxelData[x, y, z] = value;
        }

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
        
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int OffsetZ { get; set; }
        
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
                                x + OffsetX,
                                y + OffsetY,
                                z + OffsetZ
                            ),
                            true
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Serializes a chunk to a stream.
        /// </summary>
        /// <param name="writer">An object used to write to the stream.</param>
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(GetType().Name);
            writer.Write(_voxelData.Subdivisions);
            _voxelData.Serialize(writer, info =>
            {
                writer.Write(info.Distance);
                writer.Write(info.VoxelType?.Id ?? -1);
            });
        }

        /// <summary>
        /// Deserializes a chunk from a stream.
        /// </summary>
        /// <param name="reader">An object used to read from the stream.</param>
        /// <param name="voxelTypeProvider">A delegate to look up voxel types by ID.</param>
        /// <returns><c>true</c> if the chunk could be deserialized; otherwise, <c>false</c>.</returns>
        public bool Deserialize(BinaryReader reader, Func<int, VoxelType> voxelTypeProvider)
        {
            if (reader.ReadString() != GetType().Name)
            {
                return false;
            }

            if (reader.ReadInt32() != _voxelData.Subdivisions)
            {
                return false;
            }
            
            _voxelData.Deserialize(reader, () =>
            {
                var distance = reader.ReadSingle();
                var voxelTypeId = reader.ReadInt32();
                var voxelType = voxelTypeId != -1 ? voxelTypeProvider(voxelTypeId) : null;
                return new VoxelInfo
                {
                    Distance = distance,
                    VoxelType = voxelType
                };
            });
            return true;
        }

        public IEnumerator<KeyValuePair<Vector3Int, VoxelInfo>> GetEnumerator()
        {
            return _voxelData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
