using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
   public int myId = 1;
   public Pal_Stat myStat;
   public PalState palState = PalState.Wild;
    public float catchProb;

    public  MonsterUI monsterUI;
  
    public enum PalState
    {
        Wild,
        Catched
    }
    void Start()
    {
        DataManager.instance.palDict.TryGetValue(myId,out myStat);
        monsterUI = gameObject.GetComponentInChildren<MonsterUI>();
        monsterUI.monsterName.text = myStat.name;   
        monsterUI.catchProb.text = catchProb.ToString();
        monsterUI.slider.value = myStat.curHp / myStat.maxHp;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        monsterUI.catchProb.text = catchProb.ToString();
        monsterUI.slider.value = myStat.curHp / myStat.maxHp;
        if (myStat.curHp > 0)
        {
            catchProb = (myStat.maxHp / myStat.curHp) / 10;
        }

        if (palState == PalState.Wild && other.gameObject.tag == "Weapon")
        {
            Debug.Log("Hit");

            myStat.curHp -= other.gameObject.GetComponent<Weapon>().myStat.damage;
            if (myStat.curHp < 0)
            {
                myStat.curHp = 0;
                this.gameObject.SetActive(false);
            }

        }
    }
}
