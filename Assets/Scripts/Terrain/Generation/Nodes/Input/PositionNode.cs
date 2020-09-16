using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Input/Position")]
    [NodeWidth(100)]
    public class PositionNode : Node
    {
        [Output, SerializeField] public int _x;
        [Output, SerializeField] public int _y;
        [Output, SerializeField] public int _z;

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case nameof(_x):
                    return new Generator<int>(i => i.x);
                case nameof(_y):
                    return new Generator<int>(i => i.y);
                case nameof(_z):
                    return new Generator<int>(i => i.z);
                default:
                    return new Generator<int>(_ => 0);
            }
        }
    }
}
