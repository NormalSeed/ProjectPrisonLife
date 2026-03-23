using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [Serializable]
    public class PoolConfig
    {
        public string key;
        public GameObject prefab;
        [Min(1)] public int initialCount = 10;
    }

    /// <summary>
    /// 씬의 "ObjectPool" 게임오브젝트에 부착하는 풀 관리자.
    /// Inspector에서 PoolConfig 리스트를 설정하면 Start 시 자동으로 풀을 생성합니다.
    /// 코드에서 Register&lt;T&gt;() 로 런타임 등록도 가능합니다.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();

        private readonly Dictionary<string, PoolBase> _pools = new Dictionary<string, PoolBase>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Player ↔ Stone 물리 충돌 비활성화
            Physics.IgnoreLayerCollision(
                LayerMask.NameToLayer("Player"),
                LayerMask.NameToLayer("Stone"),
                true
            );
        }

        private void Start()
        {
            foreach (PoolConfig config in poolConfigs)
                CreatePool(config);
        }

        private void CreatePool(PoolConfig config)
        {
            if (config.prefab == null)
            {
                Debug.LogWarning($"[PoolManager] Pool '{config.key}' 에 프리팹이 할당되지 않았습니다.");
                return;
            }

            PooledObject component = config.prefab.GetComponent<PooledObject>();
            if (component == null)
            {
                Debug.LogWarning($"[PoolManager] 프리팹 '{config.prefab.name}' 에 PooledObject 컴포넌트가 없습니다.");
                return;
            }

            Type objType = component.GetType();
            Type poolType = typeof(ObjectPool<>).MakeGenericType(objType);
            PoolBase pool = (PoolBase)Activator.CreateInstance(poolType, config.prefab, config.initialCount, transform);

            _pools[config.key] = pool;
            Debug.Log($"[PoolManager] Pool '{config.key}' 생성 완료 ({config.initialCount}개의 {objType.Name})");
        }

        /// <summary>풀에서 오브젝트를 가져옵니다.</summary>
        public T Get<T>(string key) where T : PooledObject
        {
            if (!_pools.TryGetValue(key, out PoolBase pool))
            {
                Debug.LogWarning($"[PoolManager] Pool '{key}' 를 찾을 수 없습니다.");
                return null;
            }

            if (pool is ObjectPool<T> typedPool)
                return typedPool.Get();

            Debug.LogWarning($"[PoolManager] Pool '{key}' 의 타입이 일치하지 않습니다. Expected ObjectPool<{typeof(T).Name}>.");
            return null;
        }

        /// <summary>코드에서 직접 풀을 등록합니다.</summary>
        public void Register<T>(string key, GameObject prefab, int initialCount) where T : PooledObject
        {
            if (_pools.ContainsKey(key))
            {
                Debug.LogWarning($"[PoolManager] Pool '{key}' 가 이미 등록되어 있습니다.");
                return;
            }
            _pools[key] = new ObjectPool<T>(prefab, initialCount, transform);
            Debug.Log($"[PoolManager] Pool '{key}' 코드 등록 완료 ({initialCount}개의 {typeof(T).Name})");
        }
    }
}
