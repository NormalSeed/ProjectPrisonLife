using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("Follow Settings")]
    [SerializeField] private float _distance = 15f;

    [Header("Angle Settings")]
    [SerializeField] private float _pitchAngle = 65f;

    private Vector3 _offset;

    private void Start()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                _target = player.transform;
            else
                Debug.LogWarning("[CameraController] Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        ApplyAngle();
    }

    private void ApplyAngle()
    {
        transform.rotation = Quaternion.Euler(_pitchAngle, 0f, 0f);

        float rad = _pitchAngle * Mathf.Deg2Rad;
        _offset = new Vector3(0f, _distance * Mathf.Sin(rad), -_distance * Mathf.Cos(rad));
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        transform.position = _target.position + _offset;
    }
}
