using UnityEngine;

public class SpriteButtonHandler : MonoBehaviour
{
    [SerializeField] private LevelStartHandler levelStartMenu;
    [SerializeField] private string levelName;

    private void OnMouseDown()
    {
        print("CLICKED");
        if (levelStartMenu != null)
        {
            levelStartMenu.Activate(levelName);
        }
    }
}