using System;
using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Output/Voxel Slice Preview")]
    public class VoxelSlicePreviewNode : Node, IPreviewNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private VoxelType _voxelType;

        [SerializeField] private PreviewParameters _parameters;

        private enum Axis
        {
            // ReSharper disable once InconsistentNaming
            XZAxis, XYAxis, ZYAxis
        }
        
        [Serializable]
        private class PreviewParameters
        {
            public Vector2Int PreviewSize = new Vector2Int(32, 32);
            public Vector3Int RootPosition = Vector3Int.zero;
            [NodeEnum] public Axis Axis;
        }

        public Texture2D Generate()
        {
            var texture = new Texture2D(
                _parameters.PreviewSize.x,
                _parameters.PreviewSize.y
            );
            var generator = GetInputValue<Generator<VoxelType>>(nameof(_voxelType), _ => _voxelType);
            for (var i = 0; i < texture.width; i++)
            {
                for (var j = 0; j < texture.height; j++)
                {
                    var location = _parameters.RootPosition;
                    switch (_parameters.Axis)
                    {
                        case Axis.XZAxis:
                            location.x += i;
                            location.z += j;
                            break;
                        case Axis.ZYAxis:
                            location.y += j;
                            location.z += i;
                            break;
                        case Axis.XYAxis:
                            location.x += i;
                            location.y += j;
                            break;
                    }
                    var value = generator(location);
                    texture.SetPixel(i, j, value.Colour);
                }
            }
            texture.Apply();
            return texture;
        }
    }
}
