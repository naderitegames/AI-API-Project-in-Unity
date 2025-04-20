using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

namespace Nader.ObjectPooling
{
    public class ObjectPool<T> where T : Component
    {
        private List<T> pool = new List<T>();
        private T _prefab;

        public T prefab
        {
            private set => _prefab = value;
            get => _prefab;
        }

        private Transform parent;
        public int poolCount = 0;

        public ObjectPool(T prefab, int initialCapacity, Transform _parent, bool setActive)
        {
            parent = _parent;
            this.prefab = prefab;
            for (int i = 0; i < initialCapacity; i++)
            {
                CreateObj(setActive);
            }
        }

        public ObjectPool(T prefab, int initialCapacity, bool setActive)
            : this(prefab, initialCapacity, new GameObject(prefab.name).transform, setActive)
        {
        }

        public ObjectPool(T prefab, int initialCapacity)
            : this(prefab, initialCapacity, new GameObject(prefab.name).transform, false)
        {
        }


        public void DeActiveAll()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].gameObject.SetActive(false);
            }
        }

        public void ClearPool()
        {
            foreach (var value in pool)
            {
                Object.Destroy(value.gameObject);
            }
            pool.Clear();
        }
        public T GetObject()
        {
            foreach (T obj in pool)
            {
                if (!obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            T newObj = CreateObj(true);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        T CreateObj(bool setActive)
        {
            T obj = GameObject.Instantiate(_prefab);
            obj.transform.position = Vector3.zero;
            poolCount++;
            ReturnObject(obj, setActive);
            pool.Add(obj);
            return obj;
        }

        private void ReturnObject(T obj, bool setActive)
        {
            obj.gameObject.name = _prefab.name + " " + poolCount;
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(setActive);
        }

        public List<T> GetActiveObjects()
        {
            List<T> _list = new List<T>();
            foreach (T obj in pool)
            {
                if (obj.gameObject.activeSelf)
                {
                    _list.Add(obj);
                }
            }

            return _list;
        }
    }
}