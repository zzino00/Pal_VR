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
                Count.text = countNum.ToString();// 남은 재료아이템 개수 표시

                for(int i=0; i< machine.InputList.Count; i++)// 조합기의 리스트를 돌면서
                {
                    
                    if (machine.InputList[i].countNum==0)// 해당 리스트의 필요재료개수가 0이면(재료가 다 채워졌으면)
                    {
                        Debug.Log("Material Filled");
                        machine.CheckCount++;// 총 재료 카운트를 올려줌
                        if(machine.CheckCount == machine.InputList.Count)// 총 재료카운트가 투입구의 개수와 같다면
                        {
                            CheckMaterialFilled.Invoke();// 이벤트로 신호보내기
                            Destroy(gameObject);// 투입구는 없애기
                        }
                    }
                }
            }
               
        }
    }
}
