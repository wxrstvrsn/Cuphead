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
            /*Debug.Log("HANDLE SHOOTING");*/
            _coolDownTimer = 0;

            int index = FindActiveBullet();
            bullets[index].transform.position = (_lookUp) ? bulletHomePointUp.position : bulletHomePoint.position;
            if (_lookUp) bullets[index].GetComponent<Bullet>().SetDirection(0.0f);

            bullets[index].GetComponent<Bullet>().SetDirection(Mathf.Sign(transform.localScale.x));
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

        return 0;
    }
}