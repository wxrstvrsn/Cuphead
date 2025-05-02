using UnityEngine;

public class SpriteButtonHandler : MonoBehaviour
{
    [SerializeField] private LevelStartHandler levelStartMenu;
    [SerializeField] private string levelName;
    [SerializeField] private string soundtrack;
    
    private void OnMouseDown()
    {
        print("CLICKED");
        if (levelStartMenu != null)
        {
            levelStartMenu.Activate(levelName, soundtrack);
        }
    }
}