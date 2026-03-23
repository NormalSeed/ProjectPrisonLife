namespace Pool
{
    /// <summary>
    /// 제네릭 ObjectPool 을 딕셔너리에 저장하기 위한 비제네릭 기반 클래스.
    /// </summary>
    public abstract class PoolBase
    {
        public abstract void ReturnObject(PooledObject obj);
    }
}
