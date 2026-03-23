using UnityEngine;

/// <summary>
/// 채굴 모드 인터페이스. 손 채굴 / 드릴 / 채굴 트럭 등 교체 가능.
/// </summary>
public interface IMiningMode
{
    /// <summary>origin 위치에서 direction 방향으로 채굴을 시도합니다.</summary>
    void TryMine(Vector3 origin, Vector3 direction);
}
