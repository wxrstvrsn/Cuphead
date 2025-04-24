using UnityEngine;

public class SpriteHoverMouseEffect : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Color _defaultColor;

    [SerializeField] private Color hoverColor = Color.gray;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _defaultColor = _renderer.color;
    }

    private void OnMouseEnter()
    {
        _renderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        _renderer.color = _defaultColor;
    }
}