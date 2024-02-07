using System;
using System.Collections;
using System.Collections.Generic;
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


public class DataManager : MonoBehaviour
{
    public static DataManager instance;


    public Dictionary<int, Pal_Stat> palDict = new Dictionary<int, Pal_Stat>();
    public Dictionary<int, Weapon_Stat> weaponDict = new Dictionary<int, Weapon_Stat>();

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
