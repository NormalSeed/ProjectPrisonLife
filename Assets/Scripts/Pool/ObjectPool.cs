using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    /// <summary>
    /// 특정 타입의 PooledObject 를 관리하는 제네릭 오브젝트 풀.
    /// </summary>
    public class ObjectPool<T> : PoolBase where T : PooledObject
    {
        private readonly Queue<T> _available = new Queue<T>();
        private readonly GameObject _prefab;
        private readonly Transform _parent;

        public int CountAvailable => _available.Count;

        public ObjectPool(GameObject prefab, int initialCount, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            Prewarm(initialCount);
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Spawn();
                obj.gameObject.SetActive(false);
                _available.Enqueue(obj);
            }
        }

        private T Spawn()
        {
            GameObject go = Object.Instantiate(_prefab, _parent);
            T obj = go.GetComponent<T>();
            obj.AssignPool(this);
            return obj;
        }

        /// <summary>풀에서 오브젝트를 꺼냅니다. 풀이 비어 있으면 새로 생성합니다.</summary>
        public T Get()
        {
            T obj = _available.Count > 0 ? _available.Dequeue() : Spawn();
            obj.gameObject.SetActive(true);
            obj.OnActivated();
            return obj;
        }

        /// <summary>오브젝트를 풀로 반환합니다.</summary>
        public void Return(T obj)
        {
            obj.OnDeactivated();
            obj.gameObject.SetActive(false);
            _available.Enqueue(obj);
        }

        public override void ReturnObject(PooledObject obj)
        {
            if (obj is T typed)
                Return(typed);
            else
                Debug.LogWarning($"[ObjectPool] 타입 불일치: expected {typeof(T).Name}, got {obj.GetType().Name}");
        }
    }
}
