using System;

namespace Rebirth.Terrain.Octree
{
    /// <summary>
    /// Represents data in 3 dimensions using a Dense Octree.
    /// </summary>
    /// <typeparam name="T">The type of the data being stored.</typeparam>
    public class DenseOctree<T>
    {
        private T _value;
        private readonly DenseOctree<T>[] _nodes = new DenseOctree<T>[8];

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseOctree{T}"/> class.
        /// </summary>
        /// <param name="subdivisions">The number of subdivisions in the octree.</param>
        public DenseOctree(int subdivisions)
        {
            Dirty = 0;
            Subdivisions = subdivisions;
            if (subdivisions == 0)
            {
                return;
            }
            for (var i = 0; i < 8; i++)
            {
                _nodes[i] = new DenseOctree<T>(subdivisions - 1);
            }
        }

        /// <summary>
        /// Gets a byte which encodes which nodes have been modified using binary flags.
        /// </summary>
        public byte Dirty { get; private set; }
        /// <summary>
        /// Gets the number of subdivisions in the octree.
        /// </summary>
        public int Subdivisions { get; }
        
        /// <summary>
        /// Gets or sets the data stored at a given location within the octree.
        /// </summary>
        /// <param name="x">The x-coordinate of the data in the tree.</param>
        /// <param name="y">The y-coordinate of the data in the tree.</param>
        /// <param name="z">The z-coordinate of the data in the tree.</param>
        public T this[int x, int y, int z]
        {
            get
            {
                if (Subdivisions == 0)
                {
                    return _value;
                }
                var idx = GetNodeIndex(x, y, z);
                return _nodes[idx][x, y, z];
            }
            set => SetByIndex(x, y, z, value);
        }

        /// <summary>
        /// Sets the data stored at a given location in the octree,
        /// with the option not to mark the tree as modified.
        /// </summary>
        /// <param name="x">The x-coordinate of the data in the tree.</param>
        /// <param name="y">The y-coordinate of the data in the tree.</param>
        /// <param name="z">The z-coordinate of the data in the tree.</param>
        /// <param name="value">The value to set at the specified location.</param>
        /// <param name="clean">
        /// Specifies whether to bypass the flag
        /// indicating that the tree has been modified.
        /// </param>
        public void SetByIndex(int x, int y, int z, T value, bool clean = false)
        {
            if (Subdivisions == 0)
            {
                _value = value;
                return;
            }
            var idx = GetNodeIndex(x, y, z);
            if (!clean)
            {
                Dirty |= Convert.ToByte(1 << idx);
            }
            _nodes[idx].SetByIndex(x, y, z, value, clean);
        }

        /// <summary>
        /// Calculate the root node index of a given location in the tree.
        /// </summary>
        /// <param name="x">The x-coordinate of the data in the tree.</param>
        /// <param name="y">The y-coordinate of the data in the tree.</param>
        /// <param name="z">The z-coordinate of the data in the tree.</param>
        /// <returns>
        /// An <seealso cref="int"/> corresponding to the index of a node in the tree.
        /// </returns>
        private int GetNodeIndex(int x, int y, int z)
        {
            var index = (x >> (Subdivisions - 1)) & 1;
            index = index << 1 | ((y >> (Subdivisions - 1)) & 1);
            index = index << 1 | ((z >> (Subdivisions - 1)) & 1);
            return index;
        }
    }
}
