using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int myId;
    public Item_Stat myStat;
    Player player;
    public int ItemCount = 0;
    public ItemState itemState=ItemState.Unowned;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
      
        
    }
    public enum ItemState// 소유중인지 필드에 있는지
    {
        Unowned,
        Owned

    }
    private void OnTriggerEnter(Collider other)
    {
        if (itemState==ItemState.Unowned&&other.gameObject.tag == "RightHand")
        {

            Debug.Log("Get Item");

            if (player.myItemList.Count==0)
            {
                player.myItemList.Add(gameObject);
                ItemCount++;
              
            }
            else
            {
                Debug.Log("ItemCount+");
                for (int i = 0; i < player.myItemList.Count; i++)
                {
                    if (myId == player.myItemList[i].GetComponent<Item>().myId)
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount++;
                      

                        break;
                    }
                    else
                    {
                        if (i == player.myItemList.Count - 1)
                        {
                            player.myItemList.Add(gameObject);
                            player.myItemList[i+1].GetComponent<Item>().ItemCount++;
                       
                            break;
                        }
                      
                        continue;
                    }

                }
            }

            itemState = ItemState.Owned;
            gameObject.SetActive(false);
            if(player.myItemList[player.Index].GetComponent<Item>() != null)
            {
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }
           
        }
    }

 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RightHand")
        {
            Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            itemState = ItemState.Unowned;
            Debug.Log("Item Spawn");
            for (int i = 0; i < player.myItemList.Count; i++)
            {
                if (myId == player.myItemList[i].GetComponent<Item>().myId)
                {
                    if (player.myItemList[i].GetComponent<Item>().ItemCount<=0)
                    {
                        player.myItemList.Remove(player.myItemList[i]);
                        Debug.Log("Remove from List");
                    }
                    else
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount--;
                        Debug.Log("-ItemCount");
                        break;
                     
                    }
                   
                }
                
            }// ToDo: 현재는 아이템을 획득하면 모드와 관계없이 해당아이템의 종류와 개수가 표시됨, 모드가 Inven일때만 작동하게 설정할것
            if (player.myItemList[player.Index].GetComponent<Item>() != null)
            {
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
