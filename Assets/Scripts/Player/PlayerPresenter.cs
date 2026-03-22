using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private PlayerModel _model;
    private IPlayerView _view;

    private void Awake()
    {
        _model = new PlayerModel();
        _view = GetComponent<PlayerView>();
    }
}
