using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Utils
{
    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        public ObjectPool<T> Pool { get; set; }
    }

    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private T _prefab;
        private int _initialCapacity;
        private Transform _parent;

        private List<T> _pool = new List<T>();

        public ObjectPool(T prefab, int initialCapacity, Transform parent)
        {
            _prefab = prefab;
            _initialCapacity = initialCapacity;
            _parent = parent;
            for (int i = 0; i < _initialCapacity; i++)
            {
                AddNewObject();
            }
        }

        public void Return(T poolable)
        {
            poolable.gameObject.SetActive(false);
            poolable.transform.parent = _parent;
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
                Debug.Log($"{nameof(ObjectPool<T>)}: Pool is empty, cannot get net object.");
                return null;
            }
        }

        private void AddNewObject()
        {
            var poolable = Object.Instantiate(_prefab, _parent);
            if (poolable != null)
            {
                poolable.gameObject.SetActive(false);
                poolable.Pool = this;
                _pool.Add(poolable);
            }
            else
            {
                Debug.LogError($"ObjectPool: cannot instantiate prefab.");
            }
        }
    }
}