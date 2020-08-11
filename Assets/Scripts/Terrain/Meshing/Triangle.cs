using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    /// <summary>
    /// A triangle composed of three points in 3-D space,
    /// with each point represented as a Vector3.
    /// </summary>
    public struct Triangle
    {
        private Vector3 _point0;
        private Vector3 _point1;
        private Vector3 _point2;

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
    }
}
