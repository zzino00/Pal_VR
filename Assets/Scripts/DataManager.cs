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


public class DataManager : MonoBehaviour
{
    public static DataManager instance;


    public Dictionary<int, Pal_Stat> dict = new Dictionary<int, Pal_Stat>();


    private void Awake()
    {
        instance = this;
        TextAsset textAsset = Resources.Load<TextAsset>("Json/Pal_Data");
        Pal_Data data = JsonUtility.FromJson<Pal_Data>(textAsset.text);// 우선 리스트에 저장후
        foreach (Pal_Stat pal_Stat in data.pal_Datas)//
        {
            dict.Add(pal_Stat.Id, pal_Stat);// Dictionary에 넣어준다.
        }
    }
}
