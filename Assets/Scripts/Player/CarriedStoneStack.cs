using System.Collections.Generic;
using Pool;
using UnityEngine;

/// <summary>
/// 플레이어 등 뒤에 채굴한 돌을 한 개씩 쌓아 표시합니다.
/// CarriedStone 오브젝트 풀을 사용합니다.
/// </summary>
public class CarriedStoneStack : MonoBehaviour
{
    [SerializeField] private Vector3 stackOrigin = new Vector3(0f, -0.8f, -0.8f);
    [SerializeField] private float stackStepY = 0.16f;

    private const string PoolKey = "carriedStone";
    private readonly List<CarriedStone> _stack = new List<CarriedStone>();

    public int Count => _stack.Count;

    /// <summary>
    /// maxCount 미만이면 등 뒤에 돌을 추가하고 true 반환.
    /// maxCount 이상이면 추가하지 않고 false 반환.
    /// </summary>
    public bool TryAdd(int maxCount)
    {
        if (_stack.Count >= maxCount) return false;

        CarriedStone stone = PoolManager.Instance.Get<CarriedStone>(PoolKey);
        stone.transform.SetParent(transform);
        stone.transform.localPosition = stackOrigin + Vector3.up * (_stack.Count * stackStepY);
        stone.transform.localRotation = Quaternion.identity;
        _stack.Add(stone);
        return true;
    }

    /// <summary>쌓인 돌을 모두 풀로 반환합니다.</summary>
    public void Clear()
    {
        foreach (CarriedStone stone in _stack)
            if (stone != null) stone.ReturnToPool();
        _stack.Clear();
    }
}
