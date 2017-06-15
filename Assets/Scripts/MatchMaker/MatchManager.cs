using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    struct NewPlayerData
    {
        int playerNo;

        public NewPlayerData(int _playerNo)
        {
            playerNo = _playerNo;
        }
    }

    static MatchManager instance;

    string[] chars;
    List<NewPlayerData> newPlayers;
    List<Character> players;

    public static MatchManager GetMM() { return instance; }

    public int GetNPCount() { return newPlayers.Count; }

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

        DontDestroyOnLoad(gameObject);

        chars = new string[3]
        {
            "BallMan",
            "BallDude",
            "BallGuy"
        };

        newPlayers = new List<NewPlayerData>();
        players = new List<Character>();
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene == SceneManager.GetSceneByBuildIndex(1))
        {
            for (int i = 0; i < newPlayers.Count; i++)
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Characters/" + chars[Menu.GetMenu().GetCharSelect()[i].value]), new Vector3(i * 2, 2, i * 2), Quaternion.identity);
                players.Add(go.GetComponent<Character>());
                go.GetComponent<Character>().SetPlayerNo(players.Count);
            }
        }
    }

    public void StartMatch()
    {
        SceneManager.LoadScene(1);
    }

    public bool AddPlayer()
    {
        if (newPlayers.Count < 4)
        {
            newPlayers.Add(new NewPlayerData(players.Count));

            return true;
        }

        return false;
    }

    public void RemovePlayer()
    {

    }
}