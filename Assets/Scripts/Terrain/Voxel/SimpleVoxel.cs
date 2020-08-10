using System;
using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents a simple coloured voxel type.
    /// </summary>
    [Serializable]
    public class SimpleVoxel : IVoxelType
    {
        [SerializeField] private int _id;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private string _name;

        public int Id => _id;
        public Color Colour => _color;
        public string Name => _name;
    }
}
