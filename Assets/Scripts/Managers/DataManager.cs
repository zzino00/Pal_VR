using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Pal_Stat
{
    public int Id;
    public string name;
    public int level;
    public float maxHp;
    public float curHp;
    public float attackPower;
    public float defensePower;
    public float moveSpeed;
}

[Serializable]
public class Pal_Data
{
    public List<Pal_Stat> pal_Datas = new List<Pal_Stat>();

}


[Serializable]
public class Weapon_Stat
{
    public int Id;
    public string name;
    public int damage;
    public int durability;
}

[Serializable]
public class Weapon_Data
{
    public List<Weapon_Stat> weapon_Datas = new List<Weapon_Stat>();

}

[Serializable]
public class Player_Data
{
    public List<int> playerWeaponDatas = new List<int>();
    public List<int> playerPalDatas = new List<int>();
}


public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public Player_Data playerData;
    public Player player;

    public Dictionary<int, Pal_Stat> palDict = new Dictionary<int, Pal_Stat>();
    public Dictionary<int, Weapon_Stat> weaponDict = new Dictionary<int, Weapon_Stat>();


    [ContextMenu("To Json Data")]// 테스트용 코드 , 컴포넌트 메뉴에 아래함수를 호출하는 명령어가 생김
    void SavePlayerData()
    {
        foreach (GameObject myWeapon in player.myWeaponList)
        {
            Weapon playerWeapon = myWeapon.GetComponent<Weapon>();
           playerData.playerWeaponDatas.Add(playerWeapon.myId);
        }

        foreach (GameObject myPal in player.monsterGoList)
        {
            Monster monster = myPal.GetComponent<Monster>();
            playerData.playerPalDatas.Add(monster.myId);
        }

        string jsonData = JsonUtility.ToJson(playerData,true);

        string path = Path.Combine(Application.dataPath, "Resources/Json/Player_Data.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load Json Data")]// 테스트용 코드 , 컴포넌트 메뉴에 아래함수를 호출하는 명령어가 생김
    void LoadPlayerData()
    {
        TextAsset playerDataText = Resources.Load<TextAsset>("Json/Player_Data");
        playerData = JsonUtility.FromJson<Player_Data>(playerDataText.text);
        foreach (int playerWeaponId in playerData.playerWeaponDatas)
        {
            GameObject newWeapon;
            newWeapon = Resources.Load($"Prefabs/Weapon/{playerWeaponId.ToString()}") as GameObject;
            if (newWeapon == null)
            {
                Debug.Log("Load Failed");
            }
            Debug.Log("Sucess");

            player.myWeaponList.Add(newWeapon);
            Instantiate(newWeapon);
        }

        foreach (int playerPalId in playerData.playerPalDatas)
        {
            GameObject newPal;
            newPal = Resources.Load($"Prefabs/Pal/{playerPalId.ToString()}") as GameObject;
            if(newPal == null)
            {
                Debug.Log("Load Failed");
            }
            player.monsterGoList.Add(newPal);
            Instantiate(newPal);
           
        }

      
    }
       
    private void Awake()
    {
        instance = this;
        TextAsset palText = Resources.Load<TextAsset>("Json/Pal_Data");
        Pal_Data palData = JsonUtility.FromJson<Pal_Data>(palText.text);// 우선 리스트에 저장후

        foreach (Pal_Stat pal_Stat in palData.pal_Datas)//
        {
            palDict.Add(pal_Stat.Id, pal_Stat);// Dictionary에 넣어준다.
        }

        TextAsset weaponText = Resources.Load<TextAsset>("Json/Weapon_Data");
        Weapon_Data weaponData = JsonUtility.FromJson<Weapon_Data>(weaponText.text);// 우선 리스트에 저장후

        foreach (Weapon_Stat weaon_Stat in weaponData.weapon_Datas)//
        {
            weaponDict.Add(weaon_Stat.Id, weaon_Stat);// Dictionary에 넣어준다.
        }

      

    }
}
