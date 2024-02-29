using System.Collections;
using System.Collections.Generic;
using System.Net;
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
            if (player.myItemList.Count==0)// 리스트가 비어있으면 무조건 아이템을 리스트에 추가
            {
                player.myItemList.Add(gameObject);
                ItemCount++;
            }
            else//그게 아니면
            {
                for (int i = 0; i < player.myItemList.Count; i++) //  플레이어의 리스트를 순회하며 같은 id의 아이템이 있으면 아이템카운트를 ++
                {
                    if (myId == player.myItemList[i].GetComponent<Item>().myId)
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount++;
                        break;
                    }
                    else// 리스트에 같은 id의 아이템이 없으면 아이템을 리스트에 추가하고 카운트++, 카운트는 0부터 시작하기 때문에
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
            itemState = ItemState.Owned;// 아이템의 state를 소유된 아이템으로 변경
            gameObject.SetActive(false);// 획득했으면 아이템 비활성화 해주기
            if (player.actionManager.modeSelect == ActionManager.ModeSelect.Inven && player.Index<player.myItemList.Count && player.myItemList[player.Index].GetComponent<Item>() != null)
            {//획득한 아이템 표시는 Inven모드일때만
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }

        }
    }

 
    private void OnTriggerExit(Collider other)// 아이템을 손에서 놨을 때처리
    {
        if (other.gameObject.tag == "RightHand")
        {
            Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
            rigid.isKinematic = false;// 중력의 영향을 다시 받도록 설정
            itemState = ItemState.Unowned;// 비소유? 아이템으로 처리
            for (int i = 0; i < player.myItemList.Count; i++)
            {
                if (myId == player.myItemList[i].GetComponent<Item>().myId)
                {
                    if (player.myItemList[i].GetComponent<Item>().ItemCount<=1)// 아이템의 개수가 0보다 작을때 아이템리스트에서 빼버림
                    {
                        player.myItemList.Remove(player.myItemList[i]);
                        ItemCount = 0;
                    }
                    else
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount--;// 아이템을 손에서 놨을때 개수을 하나 빼줌
                        break;
                    }
                }
            }// ToDo: 현재는 아이템을 획득하면 모드와 관계없이 해당아이템의 종류와 개수가 표시됨, 모드가 Inven일때만 작동하게 설정할것
            if (player.Index < player.myItemList.Count&& player.myItemList[player.Index].GetComponent<Item>() != null)
            {// Index가 List의 범위밖으로 나가는지를 먼저 검사 그다음에 해당 인덱스가 null이 아닌지확인
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }
            if(player.myItemList.Count <=1 ) // 아이템리스트에 아이템이 하나밖에 없을때는 이전,이후아이템도 없음으로 표시해주기
            {
                player.playerUI.ModeNextText.text = "";
                player.playerUI.ModePreviousText.text = "";
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
