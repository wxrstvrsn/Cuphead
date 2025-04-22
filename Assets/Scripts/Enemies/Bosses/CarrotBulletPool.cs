using UnityEngine;

public class CarrotBulletPool : MonoBehaviour
{
    [SerializeField] private CarrotBullet carrotPrefab;
    [SerializeField] private int poolSize = 10;

    private CarrotBullet[] _pool;

    private void Awake()
    {
        _pool = new CarrotBullet[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            CarrotBullet bullet = Instantiate(carrotPrefab, transform);
            bullet.gameObject.SetActive(false);
            _pool[i] = bullet;
        }
    }

    public CarrotBullet GetBullet()
    {
        foreach (var bullet in _pool)
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }

        // Расширяем пул
        CarrotBullet newBullet = Instantiate(carrotPrefab, transform);
        newBullet.gameObject.SetActive(false);

        var list = new System.Collections.Generic.List<CarrotBullet>(_pool);
        list.Add(newBullet);
        _pool = list.ToArray();

        return newBullet;
    }
}