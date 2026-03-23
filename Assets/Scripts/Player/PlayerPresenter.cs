using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField] private JoystickView _joystickView;

    private PlayerModel _model;
    private IPlayerView _view;
    private Rigidbody _rigidbody;
    private IMiningMode _miningMode;

    private void Awake()
    {
        _model = new PlayerModel();
        _view = GetComponent<PlayerView>();
        _rigidbody = GetComponent<Rigidbody>();
        _miningMode = GetComponent<IMiningMode>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = _joystickView.InputDirection;
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        _rigidbody.velocity = direction * _model.MoveSpeed;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(
                Quaternion.Slerp(_rigidbody.rotation, target, _model.RotationSpeed * Time.fixedDeltaTime)
            );
        }

        _miningMode?.TryMine(transform.position, direction);
    }
}
