using UnityEngine;

namespace Pool
{
    /// <summary>
    /// 오브젝트 풀에서 관리되는 오브젝트의 기반 클래스.
    /// 풀링 가능한 오브젝트는 이 클래스를 상속하여 OnActivated / OnDeactivated 를 재정의합니다.
    /// </summary>
    public abstract class PooledObject : MonoBehaviour
    {
        private PoolBase _ownerPool;

        internal void AssignPool(PoolBase pool) => _ownerPool = pool;

        /// <summary>풀로 반환합니다.</summary>
        public void ReturnToPool()
        {
            if (_ownerPool == null)
            {
                gameObject.SetActive(false);
                return;
            }
            _ownerPool.ReturnObject(this);
        }

        /// <summary>풀에서 꺼내져 활성화될 때 호출됩니다.</summary>
        public virtual void OnActivated() { }

        /// <summary>풀로 반환되어 비활성화될 때 호출됩니다.</summary>
        public virtual void OnDeactivated() { }
    }
}
