using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MatchLobby : MonoBehaviour
{
    [SerializeField]
    Dropdown[] playerDropdown;
    [SerializeField]
    GameObject[] playerSelect, newPlayer;

    JsonData playerData;

    void Awake()
    {
        playerData = File.ReadAllText("Assets/StreamData/PlayerData.json");
    }

    void Update()
    {
        AddNewPlayer();
    }
    //playerDropdown[i].enabled = true;
    void AddNewPlayer()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButton("P" + (i + 1) + "Submit"))
            {
                playerSelect[i].SetActive(true);
                Dropdown overDrop = playerSelect[i].GetComponent<Dropdown>();
                overDrop.options = new List<Dropdown.OptionData>();

                for (int j = 0; j < playerData.Count; j++)
                {
                    overDrop.options.Add(new Dropdown.OptionData(playerData[j][0].ToString()));
                }

                overDrop.options.Add(new Dropdown.OptionData("New Player"));
            }

            if (playerSelect[i].activeSelf)
            {
                Dropdown overDrop = playerSelect[i].GetComponent<Dropdown>();

                if (Input.GetButton("P" + i + "Submit"))
                {
                    if (overDrop.value == overDrop.options.Count)
                    {
                        newPlayer[i].SetActive(true);
                        playerSelect[i].SetActive(false);
                    }

                    else
                    {
                        playerDropdown[i].options = new List<Dropdown.OptionData>();

                        for (int j = 0; j < playerData[i][1].Count; j++)
                        {
                            playerDropdown[i].options.Add(new Dropdown.OptionData(playerData[i][1][j][0].ToString()));
                        }
                    }
                }
            }

            if (newPlayer[i].activeSelf)
            {
                if (Input.GetButton("P" + i + "Submit"))
                {

                }
            }
        }
    }
}