using System;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Control Flow/Boolean Logic")]
    public class BooleanLogicNode : Node
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private bool _input1;
        
        [NodeEnum, SerializeField] private LogicOperator _operator;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private bool _input2;

        [Output, SerializeField] private bool _output;
        
        private enum LogicOperator
        {
            And,
            Or,
            Xor,
            Nand,
            Nor,
            Nxor
        }

        public override object GetValue(NodePort port)
        {
            var input1 = GetInputValue<Generator<bool>>(nameof(_input1), _ => _input1);
            var input2 = GetInputValue<Generator<bool>>(nameof(_input2), _ => _input2);
            return new Generator<bool>(i =>
            {
                switch (_operator)
                {
                    case LogicOperator.And:
                        return input1(i) && input2(i);
                    case LogicOperator.Or:
                        return input1(i) || input2(i);
                    case LogicOperator.Xor:
                        return input1(i) ^ input2(i);
                    case LogicOperator.Nand:
                        return !(input1(i) && input2(i));
                    case LogicOperator.Nor:
                        return !(input1(i) || input2(i));
                    case LogicOperator.Nxor:
                        return !(input1(i) ^ input2(i));
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}
