using System;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Output/Slice Preview")]
    public class SlicePreviewNode : Node, IPreviewNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private float _input;

        [SerializeField] private PreviewParameters _parameters;

        [Serializable]
        private class PreviewParameters
        {
            public float BlackPoint = 0;
            public float WhitePoint = 1;

            public Vector2Int PreviewSize = new Vector2Int(32, 32);
            public Vector3Int RootPosition;
        }

        public Texture2D Generate()
        {
            var texture = new Texture2D(
                _parameters.PreviewSize.x, 
                _parameters.PreviewSize.y
            );
            var generator = GetInputValue<Generator<float>>(nameof(_input), _ => _input);
            for (var x = 0; x < texture.width; x++)
            {
                for (var y = 0; y < texture.height; y++)
                {
                    var value = generator(new Vector3Int(
                        x + _parameters.RootPosition.x,
                        _parameters.RootPosition.y,
                        y + _parameters.RootPosition.z
                    ));
                    value = (value - _parameters.BlackPoint) / (_parameters.WhitePoint - _parameters.BlackPoint);
                    texture.SetPixel(x, y, new Color(value, value, value));
                }
            }
            texture.Apply();
            return texture;
        }
    }
}
