using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform bulletHomePoint;
    [SerializeField] private Transform bulletHomePointUp;
    [SerializeField] private GameObject[] bullets;

    private Player _player;
    private PlayerAnimation _playerAnimation;
    private float _coolDownTimer = Mathf.Infinity;

    private bool _isShooting;
    private bool _lookUp;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        _isShooting = Input.GetKey(KeyCode.X);
        _lookUp = Input.GetKey(KeyCode.UpArrow);

        UpdateAnimation();
        HandleShooting();

        _coolDownTimer += Time.deltaTime;
    }

    private void HandleShooting()
    {
        if (_isShooting && _coolDownTimer > attackCooldown)
        {
            _coolDownTimer = 0;

            int index = FindActiveBullet();
            if (index == -1) return;

            Vector2 direction = _lookUp ? Vector2.up : new Vector2(Mathf.Sign(transform.localScale.x), 0.0f);
            Transform spawnPoint = _lookUp ? bulletHomePointUp : bulletHomePoint;

            bullets[index].transform.position = spawnPoint.position;
            bullets[index].GetComponent<Bullet>().SetDirection(direction);
        }
    }

    private void UpdateAnimation()
    {
        _playerAnimation.UpdateShootingAnimation(_isShooting, _player.IsRunning(), _lookUp);
    }

    private int FindActiveBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }

        return -1;
    }
}