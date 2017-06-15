using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    LOGIN,
    MAIN,
    MATCH,
    LOADOUT,
    SETTINGS
}

public class Menu : MonoBehaviour
{
    [SerializeField]
    Dropdown[] charSelect;
    [SerializeField]
    Canvas[] menuScreens;

    static Menu instance;

    MenuState ms;
    GameObject matchMan;

    public static Menu GetMenu() { return instance; }

    public MenuState GetMS() { return ms; }
    public Dropdown[] GetCharSelect() { return charSelect; }

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

        ms = MenuState.MAIN;
        matchMan = (GameObject)Resources.Load("Prefabs/MatchManager");
    }

    public void Play()
    {
        menuScreens[(int)ms].gameObject.SetActive(false);
        ms = MenuState.MATCH;
        menuScreens[(int)ms].gameObject.SetActive(true);
        Instantiate(matchMan);
    }

    public void LoadoutOpen()
    {
        menuScreens[(int)ms].gameObject.SetActive(false);
        ms = MenuState.LOADOUT;
        menuScreens[(int)ms].gameObject.SetActive(true);
    }

    public void SettingsOpen()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackMain()
    {
        menuScreens[(int)ms].gameObject.SetActive(false);
        ms = MenuState.MAIN;
        menuScreens[(int)ms].gameObject.SetActive(true);
    }

    public void AddPlayer()
    {
        if (MatchManager.GetMM().AddPlayer())
        {
            charSelect[MatchManager.GetMM().GetNPCount() - 1].gameObject.SetActive(true);
        }
    }

    public void StartMatch()
    {
        MatchManager.GetMM().StartMatch();
    }
}