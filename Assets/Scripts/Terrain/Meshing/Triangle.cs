using System.Runtime.InteropServices;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    /// <summary>
    /// A triangle composed of three points in 3-D space,
    /// with each point represented as a Vector3.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Triangle
    {
#pragma warning disable 649
        private readonly Vector3 _point0;
        private readonly Vector3 _point1;
        private readonly Vector3 _point2;
#pragma warning restore 649

        /// <summary>
        /// Gets the corresponding point of the triangle.
        /// </summary>
        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return _point0;
                    case 1:
                        return _point1;
                    default:
                        return _point2;
                }
            }
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public Color Color { get; }
    }
}
