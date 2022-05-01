using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    public class MonoBehaviourPool<T> where T : MonoBehaviour
    {
        private readonly List<T> _inUseList;
        private readonly Stack<T> _notInUseStack;

        private readonly T _prefab;
        private readonly Transform _poolParent;

        private bool _initialized;

        public MonoBehaviourPool(T prefab, Transform parent, int initialElementsCount)
        {
            _prefab = prefab;

            if (!_prefab)
            {
                throw new Exception($"{GetType().Name} has failed to initialize, prefab is null! [type={typeof(T)}]");
            }
            
            _poolParent = parent;
            _notInUseStack = new Stack<T>();
            _inUseList = new List<T>();
            
            for (var i = 0; i < initialElementsCount; i++)
            {
               Object.Instantiate(prefab, parent);
            }

            _initialized = true;
        }

        public T Get()
        {
            var pooledObject = _notInUseStack.Count > 0 
                ? _notInUseStack.Pop() 
                : Object.Instantiate(_prefab, _poolParent);
            
            _inUseList.Add(pooledObject);
            return pooledObject;
        }
        
        public void Return(T obj)
        {
            if (!obj) return;
            if (!_inUseList.Contains(obj)) return;
            _inUseList.Remove(obj);
            _notInUseStack.Push(obj);
        }

        public void Return(IEnumerable<T> collection)
        {
            foreach (var obj in collection)
            {
                Return(obj);
            }
        }
    }
}