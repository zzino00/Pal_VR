using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Craft_Machine : MonoBehaviour
{
    public GameObject panel;

    
    public void ShowCraftableWeaponList()
    {
        for(int i=1; i<= DataManager.instance.weaponDict.Count; i++)
        {
            GameObject newButton = Resources.Load("Prefabs/UI/TargetObject") as GameObject; //리스트안에 있는 무기의 아이디로 프리팹을 찾아서 생성
            GameObject newText = Resources.Load("Prefabs/UI/CraftMethod") as GameObject;
            TMP_Text objectName = newButton.GetComponentInChildren<TMP_Text>();
            TMP_Text methodText = newText.GetComponentInChildren<TMP_Text>();
            Weapon_Stat tempWeaponStat; 
            DataManager.instance.weaponDict.TryGetValue(i, out tempWeaponStat);
            objectName.text = tempWeaponStat.name;
            methodText.text = tempWeaponStat.craftMethodText;

            

            newButton = Instantiate(newButton, panel.transform);
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectMethod(tempWeaponStat); });


            newText = Instantiate(newText, panel.transform);
            newButton.transform.localPosition = new Vector3(5, -2+-i*5, 0);
            newText.transform.localPosition = new Vector3(25, (float)-3.3 + -i * 5, 0);
          
        }
    }

    public List<Craft_InputObj> InputList;
    Weapon_Stat tempStat;
    public int CheckCount = 0;
    public void SelectMethod(Weapon_Stat stat)
    {
        Debug.Log("Button Clicked");
        tempStat = stat;
        for (int i=0; i<stat.craftMethod.Length; i++)
        {
            if (stat.craftMethod[i]!=0)
            {
                GameObject inputObject = Resources.Load("Prefabs/InputObject") as GameObject;
                Craft_InputObj input = inputObject.GetComponent<Craft_InputObj>();
                input.CheckMaterialFilled += CreateWeapon;
                InputList.Add(input);
                Item_Stat tempItemStat;
                DataManager.instance.itemDict.TryGetValue(i,out tempItemStat);
                input.Name.text = tempItemStat.name;
                input.countNum = stat.craftMethod[i];
                input.Count.text = input.countNum.ToString();
                inputObject= Instantiate(inputObject,gameObject.transform);
                inputObject.transform.localPosition = new Vector3(-30+i*30,10,-10);

            }
        }
    }

    void Start()
    {
      
    }

    public void CreateWeapon()
    {
        Debug.Log("Weapon Created");
        CheckCount = 0;
        Weapon_Stat newWeaponStat;
        DataManager.instance.weaponDict.TryGetValue(tempStat.Id, out newWeaponStat);
        GameObject newWeaponObj = Resources.Load($"Prefabs/Weapon/{newWeaponStat.Id.ToString()}") as GameObject;
        Instantiate(newWeaponObj,gameObject.transform.position+new Vector3(0,10,0),gameObject.transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
