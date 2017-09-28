using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    static MatchManager instance;
    
    List<Character> players;
    bool gay = true;

    public static MatchManager GetMM() { return instance; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(this);
        }
        
        players = new List<Character>();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Characters/Character"), new Vector3(i * 2, 2, i * 2), Quaternion.identity);
                //go.SetActive(false);
                Character goChar = go.GetComponent<Character>();
                goChar = players[i];
                Debug.Log(go + " " + goChar + " " + players[i]);
                goChar.LoadCharacter();
            }
        }
    }

    public void StartMatch()
    {
        SceneManager.LoadScene(1);
    }

    public void AddPlayer(Character character)
    {
        if (players.Count < 4)
        {
            players.Add(character);
            Debug.Log(character.GetPlayerNo());
        }
        if (gay == true)
        {
            Destroy (this);
        }
    }
}