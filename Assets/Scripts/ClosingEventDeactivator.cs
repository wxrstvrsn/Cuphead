using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosingEventDeactivator : MonoBehaviour
{
    private void Deactivate()
    {
        gameObject.SetActive(false);
        
    }

    private void StartNarratorB()
    {
        AudioManager.Instance.PlayNarratorB();
    }
}