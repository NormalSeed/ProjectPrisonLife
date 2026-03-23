using Pool;
using UnityEngine;

/// <summary>
/// 플레이어 등 뒤에 쌓이는 운반 중인 돌. 오브젝트 풀로 관리됩니다.
/// </summary>
public class CarriedStone : PooledObject
{
    public override void OnActivated() { }

    public override void OnDeactivated()
    {
        // 풀로 반환될 때 ObjectPool 하위로 복귀
        transform.SetParent(PoolManager.Instance.transform);
    }
}
