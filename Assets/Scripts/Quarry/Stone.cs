using Pool;

/// <summary>
/// 오브젝트 풀로 관리되는 광석.
/// Mine() 호출 시 풀로 반환되고, StoneQuarry 에 채굴 사실을 알립니다.
/// </summary>
public class Stone : PooledObject
{
    private StoneQuarry _quarry;
    private int _slotIndex;

    /// <summary>StoneQuarry 가 슬롯 배치 후 호출합니다.</summary>
    public void Initialize(StoneQuarry quarry, int slotIndex)
    {
        _quarry = quarry;
        _slotIndex = slotIndex;
    }

    public override void OnActivated()
    {
        // 다음 활성화를 위해 초기화 (Initialize 로 다시 설정됨)
        _quarry = null;
        _slotIndex = -1;
    }

    public override void OnDeactivated()
    {
        _quarry?.OnStoneMined(_slotIndex);
    }

    /// <summary>플레이어가 채굴 시 호출. 풀로 반환합니다.</summary>
    public void Mine()
    {
        ReturnToPool();
    }
}
