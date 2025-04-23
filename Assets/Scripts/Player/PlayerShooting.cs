using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.15f;
    [SerializeField] private Transform bulletHomePoint;
    [SerializeField] private Transform bulletHomePointUp;
    [SerializeField] private GameObject[] bullets;

    private float _cooldownTimer;
    private bool _shootUp;

    private void Update()
    {
        _shootUp = Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKeyDown(KeyCode.X) && _cooldownTimer >= attackCooldown)
        {
            _cooldownTimer = 0f;

            int index = FindBullet();
            if (index >= 0)
            {
                Vector2 dir = _shootUp ? Vector2.up : new Vector2(transform.localScale.x, 0f);
                Transform spawnPoint = _shootUp ? bulletHomePointUp : bulletHomePoint;

                bullets[index].transform.position = spawnPoint.position;
                bullets[index].GetComponent<Bullet>().SetDirection(dir);
            }
        }

        _cooldownTimer += Time.deltaTime;
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return -1;
    }
}