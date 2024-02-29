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
    public enum ItemState// ���������� �ʵ忡 �ִ���
    {
        Unowned,
        Owned

    }
    private void OnTriggerEnter(Collider other)
    {
        if (itemState==ItemState.Unowned&&other.gameObject.tag == "RightHand")
        {
            if (player.myItemList.Count==0)// ����Ʈ�� ��������� ������ �������� ����Ʈ�� �߰�
            {
                player.myItemList.Add(gameObject);
                ItemCount++;
            }
            else//�װ� �ƴϸ�
            {
                for (int i = 0; i < player.myItemList.Count; i++) //  �÷��̾��� ����Ʈ�� ��ȸ�ϸ� ���� id�� �������� ������ ������ī��Ʈ�� ++
                {
                    if (myId == player.myItemList[i].GetComponent<Item>().myId)
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount++;
                        break;
                    }
                    else// ����Ʈ�� ���� id�� �������� ������ �������� ����Ʈ�� �߰��ϰ� ī��Ʈ++, ī��Ʈ�� 0���� �����ϱ� ������
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
            itemState = ItemState.Owned;// �������� state�� ������ ���������� ����
            gameObject.SetActive(false);// ȹ�������� ������ ��Ȱ��ȭ ���ֱ�
            if (player.actionManager.modeSelect == ActionManager.ModeSelect.Inven && player.Index<player.myItemList.Count && player.myItemList[player.Index].GetComponent<Item>() != null)
            {//ȹ���� ������ ǥ�ô� Inven����϶���
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }

        }
    }

 
    private void OnTriggerExit(Collider other)// �������� �տ��� ���� ��ó��
    {
        if (other.gameObject.tag == "RightHand")
        {
            Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
            rigid.isKinematic = false;// �߷��� ������ �ٽ� �޵��� ����
            itemState = ItemState.Unowned;// �����? ���������� ó��
            for (int i = 0; i < player.myItemList.Count; i++)
            {
                if (myId == player.myItemList[i].GetComponent<Item>().myId)
                {
                    if (player.myItemList[i].GetComponent<Item>().ItemCount<=1)// �������� ������ 0���� ������ �����۸���Ʈ���� ������
                    {
                        player.myItemList.Remove(player.myItemList[i]);
                        ItemCount = 0;
                    }
                    else
                    {
                        player.myItemList[i].GetComponent<Item>().ItemCount--;// �������� �տ��� ������ ������ �ϳ� ����
                        break;
                    }
                }
            }// ToDo: ����� �������� ȹ���ϸ� ���� ������� �ش�������� ������ ������ ǥ�õ�, ��尡 Inven�϶��� �۵��ϰ� �����Ұ�
            if (player.Index < player.myItemList.Count&& player.myItemList[player.Index].GetComponent<Item>() != null)
            {// Index�� List�� ���������� ���������� ���� �˻� �״����� �ش� �ε����� null�� �ƴ���Ȯ��
                player.playerUI.ModeText.text = player.myItemList[player.Index].GetComponent<Item>().myStat.name + "x" + player.myItemList[player.Index].GetComponent<Item>().ItemCount;
            }
            if(player.myItemList.Count <=1 ) // �����۸���Ʈ�� �������� �ϳ��ۿ� �������� ����,���ľ����۵� �������� ǥ�����ֱ�
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
