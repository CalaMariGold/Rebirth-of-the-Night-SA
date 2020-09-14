using System;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    public abstract class MemoizingNode<T> : Node
    {
        [NonSerialized]
        private Vector3Int? _key;
        
        [NonSerialized]
        private T _value;
        public sealed override object GetValue(NodePort port)
        {
            var generator = GetDelegate(port);
            return new Generator<T>(i =>
            {
                if (_key == i)
                {
                    return _value;
                }

                _key = i;
                _value = generator(i);
                return _value;
            });
        }

        protected abstract Generator<T> GetDelegate(NodePort port);
    }
    
    public abstract class TerrainNode<T> : MemoizingNode<T>
    {
        protected abstract T Generate(Vector3Int location);

        protected override Generator<T> GetDelegate(NodePort port)
        {
            return Generate;
        }
    }

    public abstract class TerrainNode<T, TOut> : MemoizingNode<TOut>
    {
        protected abstract TOut Generate(Vector3Int location, T input);

        protected Generator<TOut> CreateDelegate(Generator<T> input)
        {
            return i => Generate(i, input(i));
        }
    }
    
    public abstract class TerrainNode<T1, T2, TOut> : MemoizingNode<TOut>
    {
        protected abstract TOut Generate(Vector3Int location, T1 input1, T2 input2);

        protected Generator<TOut> CreateDelegate(Generator<T1> input1, Generator<T2> input2)
        {
            return i => Generate(i, input1(i), input2(i));
        }
    }

    public delegate T Generator<out T>(Vector3Int position);
}
