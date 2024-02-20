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
    public void ItemDrop()
    {
        int randDropItem = UnityEngine.Random.Range(0, DataManager.instance.item_Go.item_Objects.Count);
        int randDropNum = UnityEngine.Random.Range(1, 4);

        while(randDropNum-- >0)
        {
           GameObject DropItem = Instantiate(DataManager.instance.item_Go.item_Objects[randDropItem], transform.position+new Vector3(0,1,0),transform.rotation);
            Item item = DropItem.GetComponent<Item>();
            DataManager.instance.itemDict.TryGetValue(item.myId, out item.myStat);

        }
     
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
                ItemDrop();
                this.gameObject.SetActive(false);
            }

        }
    }
}
