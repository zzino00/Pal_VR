using JetBrains.Annotations;
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
    public  int[] craftMethod;
    public string craftMethodText;
}

[Serializable]
public class Item_Stat
{
    public int Id;
    public string name;
    public string discription;
}

[Serializable]
public class Weapon_Data
{
    public List<Weapon_Stat> weapon_Datas = new List<Weapon_Stat>();

}

[Serializable]
public class Item_Data
{
    public List<Item_Stat> item_Datas = new List<Item_Stat>();
}

[Serializable]
public class Item_Go
{
    public List<GameObject> item_Objects = new List<GameObject>();
}

[Serializable]
public class Player_Data // 플레이어가 현재 데이터
{
    public List<int> playerWeaponDatas = new List<int>();
    public List<int> playerPalDatas = new List<int>();
}




public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public Player_Data playerData;
    public Item_Go item_Go;
    public Player player;

    public Dictionary<int, Pal_Stat> palDict = new Dictionary<int, Pal_Stat>();
    public Dictionary<int, Weapon_Stat> weaponDict = new Dictionary<int, Weapon_Stat>();
    public Dictionary<int, Item_Stat> itemDict = new Dictionary<int, Item_Stat>();

       
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


        TextAsset itemText = Resources.Load<TextAsset>("Json/Item_Data");
        Item_Data itemData = JsonUtility.FromJson<Item_Data>(itemText.text);
        foreach(Item_Stat item_Stat in itemData.item_Datas)
        {
            itemDict.Add(item_Stat.Id, item_Stat);
        }
    }

  

    [ContextMenu("To Json Data")]// 테스트용 코드 , 컴포넌트 메뉴에 아래함수를 호출하는 명령어가 생김
    public void SavePlayerData()
    {
        foreach (GameObject myWeapon in player.myWeaponList)// Player.cs에 있는 리스트에서 무기항목들을 가져와서 playerWeaponDatas에 저장
        {
            Weapon playerWeapon = myWeapon.GetComponent<Weapon>();
            playerData.playerWeaponDatas.Add(playerWeapon.myId);
        }

        foreach (GameObject myPal in player.monsterGoList)// Player.cs에 있는 리스트에서 팰항목들을 가져와서 playerPalDatas에 저장
        {
            Monster monster = myPal.GetComponent<Monster>();
            playerData.playerPalDatas.Add(monster.myId);
        }
        Debug.Log("Saved");
        string jsonData = JsonUtility.ToJson(playerData, true);//플레이어 데이터를 json형식 string으로 변환

        string path = Path.Combine(Application.dataPath, "Resources/Json/Player_Data.json");//경로지정
        File.WriteAllText(path, jsonData);//지정된 경로에 파일 생성
    }

    [ContextMenu("Load Json Data")]// 테스트용 코드 , 컴포넌트 메뉴에 아래함수를 호출하는 명령어가 생김
    public void LoadPlayerData()
    {
        TextAsset playerDataText = Resources.Load<TextAsset>("Json/Player_Data");// 지정된 경로에서 데이터 찾기
        playerData = JsonUtility.FromJson<Player_Data>(playerDataText.text);// Json양식에서 읽을수 있게 변환
        foreach (int playerWeaponId in playerData.playerWeaponDatas)//playerWeaponDatas를 돌면서 
        {
            GameObject newWeapon;
            newWeapon = Resources.Load($"Prefabs/Weapon/{playerWeaponId.ToString()}") as GameObject; //리스트안에 있는 무기의 아이디로 프리팹을 찾아서 생성
            if (newWeapon == null)
            {
                Debug.Log("Load Failed");
            }
            Debug.Log("Sucess");

            GameObject loadedWeaon = Instantiate(newWeapon);
            player.myWeaponList.Add(loadedWeaon);// 생성된 무기를 플레이어 무기리스트에 추가
            player.equipedWeapon = player.myWeaponList[0];
            loadedWeaon.SetActive(false);

        }

        foreach (int playerPalId in playerData.playerPalDatas)
        {
            GameObject newPal;
            newPal = Resources.Load($"Prefabs/Pal/{playerPalId.ToString()}") as GameObject;
            if (newPal == null)
            {
                Debug.Log("Load Failed");
            }

            GameObject loadedPal = Instantiate(newPal);
            player.monsterGoList.Add(loadedPal);
            player.summonedPal = player.monsterGoList[0];
            loadedPal.SetActive(false);
        }


    }
}
