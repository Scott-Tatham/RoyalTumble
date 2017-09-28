using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loadout : MonoBehaviour
{
    static Loadout instance;

    int charIndex;
    JsonData charData;
    GameObject selectedChar;
    List<GameObject> playerChars;

    public static Loadout GetLO() { return instance; }

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
    }

    void Start()
    {
        charData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamData/CharacterData.json"));
        //charData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamData/CharData.json"));
        playerChars = new List<GameObject>();

        for (int i = 0; i < charData.Count; i++)
        {
            //GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Characters/" + charData[i][0].ToString()), Vector3.zero, Quaternion.identity);
            //// Insert Weapon Data.
            //Destroy(go.GetComponent<Character>());
            //go.SetActive(false);
            //go.GetComponent<Rigidbody>().useGravity = false;
            //
            //for (int j = 0; j < go.transform.childCount; j++)
            //{
            //    go.transform.GetChild(j).GetComponent<Rigidbody>().useGravity = false;
            //}
            //
            //playerChars.Add(go);
        }
    }

    void Update()
    {
        PlaceWeapon();
    }

    public void LoadoutOpen()
    {
        playerChars[charIndex].SetActive(true);
    }

    public void LoadoutClose()
    {
        playerChars[charIndex].SetActive(false);
    }

    public void LeftChar()
    {
        playerChars[charIndex].SetActive(false);
        charIndex = charIndex == 0 ? 0 : charIndex - 1;
        playerChars[charIndex].SetActive(true);
    }

    public void RightChar()
    {
        playerChars[charIndex].SetActive(false);
        charIndex = charIndex == playerChars.Count - 1 ? playerChars.Count - 1 : charIndex + 1;
        playerChars[charIndex].SetActive(true);
    }

    void PlaceWeapon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject)
                {

                }
            }
        }
    }
}