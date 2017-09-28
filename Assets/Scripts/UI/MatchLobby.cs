using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    IN,
    OUT,
    LOCKED
}

public enum StickButton
{
    NEGATIVE,
    ZILCH,
    POSITIVE
}

public class MatchLobby : MonoBehaviour
{
    [SerializeField]
    Transform[] cPos;
    [SerializeField]
    GameObject[] pIn, pOut, lockedText;

    Coroutine cycleTimer;
    JsonData charData;
    WeaponFactory wf;
    GameObject characterObj, weaponObj;
    bool[] pAction, pCanCycle;
    int[] pCharIndex;
    PlayerState[] pState;
    StickButton[] pCycle;
    GameObject[] p1Dis, p2Dis, p3Dis, p4Dis;

    void Awake()
    {
        charData = JsonMapper.ToObject(File.ReadAllText("Assets/StreamData/CharacterData.json"));
        wf = new WeaponFactory();
        characterObj = Resources.Load<GameObject>("Prefabs/Characters/Character");
        weaponObj = Resources.Load<GameObject>("Prefabs/Weapons/Weapon");
        pAction = new bool[4] { false, false, false, false };
        pCanCycle = new bool[4] { true, true, true, true };
        pCharIndex = new int[4] { 0, 0, 0, 0 };
        pState = new PlayerState[4] { PlayerState.OUT, PlayerState.OUT, PlayerState.OUT, PlayerState.OUT };
        pCycle = new StickButton[4] { StickButton.ZILCH, StickButton.ZILCH, StickButton.ZILCH, StickButton.ZILCH };
        p1Dis = new GameObject[charData.Count];
        p2Dis = new GameObject[charData.Count];
        p3Dis = new GameObject[charData.Count];
        p4Dis = new GameObject[charData.Count];
        SetCharacters(p1Dis, 1);
        SetCharacters(p2Dis, 2);
        SetCharacters(p3Dis, 3);
        SetCharacters(p4Dis, 4);
    }

    void Update()
    {
        AddNewPlayer();
        LockNewPlayer();
        RemoveNewPlayer();
        UnlockNewPlayer();
        BeginMatch();
        SwitchCharacter();
        ResetAction();
    }

    void SetCharacters(GameObject[] characters, int playerNo)
    {
        for (int i = 0; i < charData.Count; i++)
        {
            characters[i] = Instantiate(characterObj, cPos[playerNo - 1].transform);
            characters[i].SetActive(i == 0 ? true : false);
            characters[i].GetComponent<Character>().SetCharacterDisplay(playerNo, charData[i][2].Count, (string)charData[i][1]);

            for (int j = 0; j < charData[i][2].Count; j++)
            {
                Vector3 weaponPos = Vector3.zero;
                Vector3 weaponRot = Vector3.zero;

                switch ((int)charData[i][3][j])
                {
                    case 0:
                        weaponPos = Vector3.right;
                        weaponRot = new Vector3(0, 0, 90);

                        break;


                    case 1:
                        weaponPos = Vector3.left;
                        weaponRot = new Vector3(0, 0, -90);

                        break;


                    case 2:
                        weaponPos = Vector3.up;
                        weaponRot = new Vector3(180, 0, 0);

                        break;


                    case 3:
                        weaponPos = Vector3.down;
                        weaponRot = new Vector3(0, 0, 0);

                        break;


                    case 4:
                        weaponPos = Vector3.forward;
                        weaponRot = new Vector3(90, 0, 0);

                        break;


                    case 5:
                        weaponPos = Vector3.back;
                        weaponRot = new Vector3(-90, 0, 0);

                        break;
                }

                GameObject newWeaponObj = Instantiate(weaponObj, characters[i].transform);
                newWeaponObj.transform.localPosition = weaponPos * 10;
                newWeaponObj.transform.rotation = Quaternion.Euler(weaponRot);
                Weapon weapon = newWeaponObj.GetComponent<Weapon>();
                weapon = wf.GenerateWeapon((int)charData[i][2][j], weapon);
                newWeaponObj.name = weapon.GetWeaponName();
                characters[i].GetComponent<Character>().BuildWeapon(j, wf.GetMesh(j), wf.GetMaterial(j), newWeaponObj);
            }
        }
    }

    void AddNewPlayer()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButtonDown("P" + (i + 1) + "Submit") && pState[i] == PlayerState.OUT && !pAction[i])
            {
                pIn[i].SetActive(true);
                pOut[i].SetActive(false);
                pState[i] = PlayerState.IN;
                pAction[i] = true;
            }
        }
    }

    void LockNewPlayer()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButtonDown("P" + (i + 1) + "Submit") && pState[i] == PlayerState.IN && !pAction[i])
            {
                lockedText[i].SetActive(true);
                pState[i] = PlayerState.LOCKED;
                pAction[i] = true;
            }
        }
    }

    void RemoveNewPlayer()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButtonDown("P" + (i + 1) + "Cancel") && pState[i] == PlayerState.IN && !pAction[i])
            {
                pIn[i].SetActive(false);
                pOut[i].SetActive(true);
                pState[i] = PlayerState.OUT;
                pAction[i] = true;
            }
        }
    }

    void UnlockNewPlayer()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButtonDown("P" + (i + 1) + "Cancel") && pState[i] == PlayerState.LOCKED && !pAction[i])
            {
                lockedText[i].SetActive(false);
                pState[i] = PlayerState.IN;
                pAction[i] = true;
            }
        }
    }

    void BeginMatch()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetButtonDown("P" + (i + 1) + "Submit") && pState[i] == PlayerState.LOCKED && !pAction[i])
            {
                for (int j = 0; j < 4; j++)
                {
                    if (pState[i] == PlayerState.IN)
                    {
                        return;
                    }
                }

                pAction[i] = true;

                if (pState[0] == PlayerState.LOCKED)
                {
                    MatchManager.GetMM().AddPlayer(p1Dis[pCharIndex[0]].GetComponent<Character>());
                    Debug.Log(p1Dis[pCharIndex[0]].GetComponent<Character>().GetPlayerNo());
                }

                if (pState[1] == PlayerState.LOCKED)
                {
                    MatchManager.GetMM().AddPlayer(p2Dis[pCharIndex[1]].GetComponent<Character>());
                }

                if (pState[2] == PlayerState.LOCKED)
                {
                    MatchManager.GetMM().AddPlayer(p3Dis[pCharIndex[2]].GetComponent<Character>());
                }

                if (pState[3] == PlayerState.LOCKED)
                {
                    MatchManager.GetMM().AddPlayer(p4Dis[pCharIndex[3]].GetComponent<Character>());
                }

                MatchManager.GetMM().StartMatch();
            }
        }
    }

    void SwitchCharacter()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            float xDir = Input.GetAxis("P" + (i + 1) + "XDir");
            StickButton current;
            
            if (xDir > 0)
            {
                current = StickButton.POSITIVE;
            }

            else if (xDir < 0)
            {
                current = StickButton.NEGATIVE;
            }

            else
            {
                current = StickButton.ZILCH;
            }

            bool byPass = false;

            if (pCycle[i] == StickButton.NEGATIVE && current == StickButton.POSITIVE)
            {
                byPass = true;
            }

            else if (pCycle[i] == StickButton.POSITIVE && current == StickButton.NEGATIVE)
            {
                byPass = true;
            }

            if (xDir != 0 && pState[i] == PlayerState.IN && (pCanCycle[i] || byPass))
            {
                switch (i)
                {
                    case 0:
                        p1Dis[pCharIndex[i]].SetActive(false);

                        break;

                    case 1:
                        p2Dis[pCharIndex[i]].SetActive(false);

                        break;

                    case 2:
                        p3Dis[pCharIndex[i]].SetActive(false);

                        break;

                    case 3:
                        p4Dis[pCharIndex[i]].SetActive(false);

                        break;
                }

                if (current == StickButton.POSITIVE)
                {
                    if (pCharIndex[i] == 0)
                    {
                        pCharIndex[i] = charData.Count - 1;
                    }

                    else
                    {
                        pCharIndex[i]--;
                    }
                }

                if (current == StickButton.NEGATIVE)
                {
                    if (pCharIndex[i] == charData.Count - 1)
                    {
                        pCharIndex[i] = 0;
                    }

                    else
                    {
                        pCharIndex[i]++;
                    }
                }

                switch (i)
                {
                    case 0:
                        p1Dis[pCharIndex[i]].SetActive(true);

                        break;

                    case 1:
                        p2Dis[pCharIndex[i]].SetActive(true);

                        break;

                    case 2:
                        p3Dis[pCharIndex[i]].SetActive(true);

                        break;

                    case 3:
                        p4Dis[pCharIndex[i]].SetActive(true);

                        break;
                }
                
                if (byPass)
                {
                    StopCoroutine(cycleTimer);
                }

                pCycle[i] = current;
                pCanCycle[i] = false;
                ResetCycle(i);
            }
        }
    }

    // Limits certain actions to one a frame.
    void ResetAction()
    {
        for (int i = 0; i < 4; i++)
        {
            pAction[i] = false;
        }
    } 

    void ResetCycle(int i)
    {
        cycleTimer = StartCoroutine(CycleTimer(i));
    }

    IEnumerator CycleTimer(int i)
    {
        yield return new WaitForSeconds(0.5f);
        pCanCycle[i] = true;
    }
}