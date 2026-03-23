using UnityEngine;

/// <summary>
/// 손으로 직접 채굴하는 기본 채굴 모드.
/// SphereCast로 높이 차이에 무관하게 감지하며, showDebug 시 Scene 뷰에서 Gizmo를 표시합니다.
/// </summary>
public class HandMiningMode : MiningModeBase
{
    [SerializeField] private float range = 1.5f;
    [SerializeField] private float sphereRadius = 0.5f;

    // 디버그 시각화용 (OnDrawGizmos 에서 사용)
    private Vector3 _lastOrigin;
    private Vector3 _lastDirection;
    private float _lastHitDistance = -1f;
    private float _debugDrawDuration = 0.1f;

    public override void TryMine(Vector3 origin, Vector3 direction)
    {
        if (!CanMine || direction.sqrMagnitude < 0.01f) return;

        Vector3 dir = direction.normalized;

        // Scene 뷰 시각화: 노란 선 = 탐지 범위
        if (showDebug)
            Debug.DrawRay(origin, dir * range, Color.yellow, _debugDrawDuration);

        if (Physics.SphereCast(origin, sphereRadius, dir, out RaycastHit hit, range))
        {
            Stone stone = hit.collider.GetComponent<Stone>();
            if (stone == null) return;

            // Scene 뷰 시각화: 빨간 선 = 돌 감지
            if (showDebug)
                Debug.DrawLine(origin, hit.point, Color.red, 0.3f);

            stone.Mine();
            StoneStack?.TryAdd(maxCarry); // 최대 미만이면 등 뒤에 추가, 초과면 무시
            RecordMine();

            _lastOrigin = origin;
            _lastDirection = dir;
            _lastHitDistance = hit.distance;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebug || _lastDirection == Vector3.zero) return;

        // 탐지 구체 시작점
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(_lastOrigin, sphereRadius);

        // 탐지 구체 끝점
        Gizmos.color = _lastHitDistance >= 0f
            ? new Color(1f, 0f, 0f, 0.4f)
            : new Color(1f, 1f, 0f, 0.15f);
        Vector3 endPos = _lastOrigin + _lastDirection * (_lastHitDistance >= 0f ? _lastHitDistance : range);
        Gizmos.DrawSphere(endPos, sphereRadius);

        _lastHitDistance = -1f;
    }
}
