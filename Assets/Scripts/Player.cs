using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<int> myPalList = new List<int>();
    public List<GameObject> monsterGoList = new List<GameObject>();
    public GameObject palBall;
    public Transform summonTrs;
    bool isMyPalSummoned;
    GameObject summonedPal;
    void Start()
    {
        summonedPal = Instantiate(monsterGoList[0], summonTrs.position, Quaternion.identity);
        summonedPal.SetActive(false);
    }
    public void SummonPal(int summonNum)
    {
      
        if (isMyPalSummoned==false)
        {
              summonedPal = monsterGoList[summonNum];
              Monster monster = summonedPal.GetComponent<Monster>();
              DataManager.instance.dict.TryGetValue(monster.myId, out monster.myStat);
              monster.palState = Monster.PalState.Catched;
              summonedPal.SetActive(true);
              summonedPal.transform.position = summonTrs.position;
              isMyPalSummoned = true;

        }
        else
        {
            summonedPal.SetActive(false);
            isMyPalSummoned = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        summonTrs = palBall.transform;
    }
}
