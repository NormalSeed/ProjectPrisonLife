using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float distance = 15f;

    [Header("Angle Settings")]
    [SerializeField] private float pitchAngle = 65f;

    private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogWarning("[CameraController] Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        ApplyAngle();
    }

    private void ApplyAngle()
    {
        transform.rotation = Quaternion.Euler(pitchAngle, 0f, 0f);

        float rad = pitchAngle * Mathf.Deg2Rad;
        offset = new Vector3(0f, distance * Mathf.Sin(rad), -distance * Mathf.Cos(rad));
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }
}
