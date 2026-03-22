using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField] private JoystickView _joystickView;

    private PlayerModel _model;
    private IPlayerView _view;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _model = new PlayerModel();
        _view = GetComponent<PlayerView>();
        _rigidbody = GetComponent<Rigidbody>();
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
    }
}
