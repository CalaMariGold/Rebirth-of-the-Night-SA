using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Input/Position")]
    [NodeWidth(100)]
    public class PositionNode : Node
    {
        [Output, SerializeField] public float _x;
        [Output, SerializeField] public float _y;
        [Output, SerializeField] public float _z;

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case nameof(_x):
                    return new Generator<float>(i => i.x);
                case nameof(_y):
                    return new Generator<float>(i => i.y);
                case nameof(_z):
                    return new Generator<float>(i => i.z);
                default:
                    return new Generator<float>(_ => 0);
            }
        }
    }
}
