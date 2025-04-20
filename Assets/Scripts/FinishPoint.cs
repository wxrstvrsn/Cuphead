using System;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // меняем сцену
            SceneController.instance.NextLevel();
        }
    }
}
