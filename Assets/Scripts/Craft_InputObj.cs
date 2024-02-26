using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Craft_InputObj : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Count;
    public int countNum;
    Craft_Machine machine;
    void Start()
    {
        machine = gameObject.transform.parent.GetComponent<Craft_Machine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    public event Action CheckMaterialFilled;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Material"&& Name.text == other.gameObject.GetComponent<Item>().myStat.name)
        {
            if(countNum>0)
            {
                other. gameObject.SetActive(false);
                countNum--;
                Count.text = countNum.ToString();
                for(int i=0; i< machine.InputList.Count; i++)
                {
                    if (machine.InputList[i].countNum==0)// 여기서부터
                    {
                        Debug.Log("Material Filled");
                        machine.CheckCount++;
                        if(machine.CheckCount == machine.InputList[i].countNum)
                        {
                            CheckMaterialFilled.Invoke();
                        }
                    }
                }
            }
               
        }
    }
}
