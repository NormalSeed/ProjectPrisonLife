using UnityEngine;

/// <summary>
/// 모든 채굴 모드의 공통 기반. 쿨타임, 최대 운반 수량, 디버그 시각화를 제공합니다.
/// </summary>
public abstract class MiningModeBase : MonoBehaviour, IMiningMode
{
    [SerializeField] protected float cooldown = 0.5f;
    [SerializeField] protected bool showDebug = true;
    [SerializeField] protected int maxCarry = 20;

    protected CarriedStoneStack StoneStack { get; private set; }

    private float _lastMineTime = float.NegativeInfinity;

    protected bool CanMine => Time.time - _lastMineTime >= cooldown;
    protected void RecordMine() => _lastMineTime = Time.time;

    protected virtual void Awake()
    {
        StoneStack = GetComponent<CarriedStoneStack>();
    }

    public abstract void TryMine(Vector3 origin, Vector3 direction);
}
