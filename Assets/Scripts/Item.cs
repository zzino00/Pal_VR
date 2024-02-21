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
    public ItemState itemState;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        itemState = ItemState.Unowned;
        
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
                Debug.Log(player.myItemList[0].GetComponent<Item>().myStat.name + player.myItemList[0].GetComponent<Item>().ItemCount);
            }
            else
            {
                for (int i = 0; i < player.myItemList.Count; i++)
                {
                    if (myId == player.myItemList[i].GetComponent<Item>().myId)
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount++;
                        Debug.Log(player.myItemList[i].GetComponent<Item>().myStat.name + player.myItemList[i].GetComponent<Item>().ItemCount);
                        break;
                    }
                    else
                    {
                        if (i == player.myItemList.Count - 1)
                        {
                            player.myItemList.Add(gameObject);
                            player.myItemList[i+1].GetComponent<Item>().ItemCount++;
                            Debug.Log(player.myItemList[i+1].GetComponent<Item>().myStat.name + player.myItemList[i+1].GetComponent<Item>().ItemCount);
                            break;
                        }
                      
                        continue;
                    }

                }
            }

            itemState = ItemState.Owned;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RightHand")
        {
            Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            itemState = ItemState.Unowned;
            for (int i = 0; i < player.myItemList.Count; i++)
            {
                if (myId == player.myItemList[i].GetComponent<Item>().myId)
                {
                    if (player.myItemList[i].GetComponent<Item>().ItemCount<=0)
                    {
                        player.myItemList.Remove(player.myItemList[i]);
                    }
                    else
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount--;
                    }
                  
                }
                
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
