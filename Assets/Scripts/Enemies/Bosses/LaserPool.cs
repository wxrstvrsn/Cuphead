using UnityEngine;

public class LaserPool : MonoBehaviour
{
    [SerializeField] private LaserSegment _laserPrefab;
    [SerializeField] private int _poolSize = 20;

    private LaserSegment[] _pool;

    private void Awake()
    {
        _pool = new LaserSegment[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            _pool[i] = Instantiate(_laserPrefab, transform);
            _pool[i].gameObject.SetActive(false);
        }
    }

    public LaserSegment GetLaserSegment()
    {
        foreach (var segment in _pool)
        {
            if (!segment.gameObject.activeInHierarchy)
                return segment;
        }

        // если все заняты — расширяем пул
        var newSegment = Instantiate(_laserPrefab, transform);
        newSegment.gameObject.SetActive(false);

        var newPool = new LaserSegment[_pool.Length + 1];
        _pool.CopyTo(newPool, 0);
        newPool[^1] = newSegment;
        _pool = newPool;

        return newSegment;
    }
}