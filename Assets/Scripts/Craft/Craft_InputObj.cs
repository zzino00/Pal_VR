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
    public int countNum;// 재료 카운트
    Craft_Machine machine;
    void Start()
    {
        machine = gameObject.transform.parent.GetComponent<Craft_Machine>();
    }
    public event Action CheckMaterialFilled;// 재료가 전부 채워졌을때 실행되는 action
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Material"&& Name.text == other.gameObject.GetComponent<Item>().myStat.name.ToString())// tag가 재료아이템일때만
        {
            if(countNum>0)
            {
                other. gameObject.SetActive(false);// 투입한 재료아이템 비활성화
                countNum--;
                Count.text = "x" + countNum.ToString();// 남은 재료아이템 개수 표시

                for(int i=0; i< machine.InputList.Count; i++)// 조합기의 리스트를 돌면서
                {
                    
                    if (machine.InputList[i].countNum==0)// 해당 리스트의 필요재료개수가 0이면(재료가 다 채워졌으면)
                    {
                        Debug.Log("Material Filled");


                        machine.InputList.RemoveAt(i); // 리스트에서 해당재료를 받은 투입구를 빼줌
                        if(machine.InputList.Count==0)// 투입구리스트의 카운트가 0이되면 재료를 다 받았다는 이야기니까
                        {
                          
                            CheckMaterialFilled.Invoke();// 이벤트로 신호보내기
                           
                        }
                        Destroy(gameObject);// 투입구는 없애기
                    }
                }
            }
               
        }
    }
}
