using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaded : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += MatchManager.GetMM().OnSceneLoaded;
    }
}