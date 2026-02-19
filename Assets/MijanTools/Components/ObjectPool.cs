using System.Collections.Generic;
using UnityEngine;

namespace MijanTools.Components
{
    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        public ObjectPool<T> Pool { get; set; }
    }

    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private T _prefab;
        private int _initialCapacity;
        private List<T> _pool;

        public Transform Parent { get; private set; }

        public ObjectPool(T prefab, int initialCapacity, Transform parent)
        {
            _prefab = prefab;
            _initialCapacity = initialCapacity;
            _pool = new List<T>();
            Parent = parent;
            for (int i = 0; i < _initialCapacity; i++)
            {
                AddNewObject();
            }
        }

        public void Return(T poolable)
        {
            poolable.gameObject.SetActive(false);
            poolable.transform.parent = Parent;
            _pool.Add(poolable);
        }

        public T Get()
        {
            if (_pool.Count == 0)
            {
                AddNewObject();
            }

            if (_pool.Count > 0)
            {
                var poolable = _pool[0];
                _pool.RemoveAt(0);
                poolable.gameObject.SetActive(true);
                poolable.gameObject.transform.parent = null;
                return poolable;
            }
            else
            {
                Debug.LogWarning($"{nameof(ObjectPool<T>)}: Pool is empty, cannot get new object.");
                return null;
            }
        }

        private void AddNewObject()
        {
            var poolable = Object.Instantiate(_prefab, Parent);
            if (poolable != null)
            {
                poolable.gameObject.SetActive(false);
                poolable.Pool = this;
                _pool.Add(poolable);
            }
            else
            {
                Debug.LogError($"{nameof(ObjectPool<T>)}: Cannot instantiate prefab.");
            }
        }
    }
}