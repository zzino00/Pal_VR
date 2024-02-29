using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Craft_InputObj : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Count;
    public int countNum;// ��� ī��Ʈ
    Craft_Machine machine;
    void Start()
    {
        machine = gameObject.transform.parent.GetComponent<Craft_Machine>();
    }
    public event Action CheckMaterialFilled;// ��ᰡ ���� ä�������� ����Ǵ� action
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Material"&& Name.text == other.gameObject.GetComponent<Item>().myStat.name.ToString())// tag�� ���������϶���
        {
            if(countNum>0)
            {
                other. gameObject.SetActive(false);// ������ �������� ��Ȱ��ȭ
                countNum--;
                Count.text = countNum.ToString();// ���� �������� ���� ǥ��

                for(int i=0; i< machine.InputList.Count; i++)// ���ձ��� ����Ʈ�� ���鼭
                {
                    
                    if (machine.InputList[i].countNum==0)// �ش� ����Ʈ�� �ʿ���ᰳ���� 0�̸�(��ᰡ �� ä��������)
                    {
                        Debug.Log("Material Filled");
                        machine.CheckCount++;// �� ��� ī��Ʈ�� �÷���
                        if(machine.CheckCount == machine.InputList.Count)// �� ���ī��Ʈ�� ���Ա��� ������ ���ٸ�
                        {
                            CheckMaterialFilled.Invoke();// �̺�Ʈ�� ��ȣ������
                            Destroy(gameObject);// ���Ա��� ���ֱ�
                        }
                    }
                }
            }
               
        }
    }
}