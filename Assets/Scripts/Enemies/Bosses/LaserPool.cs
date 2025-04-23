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

        LaserSegment newSegment = Instantiate(_laserPrefab, transform);
        newSegment.gameObject.SetActive(false);

        var list = new System.Collections.Generic.List<LaserSegment>(_pool);
        list.Add(newSegment);
        _pool = list.ToArray();

        return newSegment;
    }
}