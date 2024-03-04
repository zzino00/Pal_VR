using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Craft_Machine : MonoBehaviour
{
    public GameObject panel;//조합패널

    public void ShowCraftableWeaponList()// 제작가능한 무기목록을 패널에 띄우는 함수
    {
        for (int i = 1; i <= DataManager.instance.weaponDict.Count; i++)
        {
            GameObject newButton = Resources.Load("Prefabs/UI/TargetObject") as GameObject; //프리팹으로 버튼불러오기
            GameObject newText = Resources.Load("Prefabs/UI/CraftMethod") as GameObject;// 프리팹으로 택스트불러오기
            TMP_Text objectName = newButton.GetComponentInChildren<TMP_Text>();
            TMP_Text methodText = newText.GetComponentInChildren<TMP_Text>();
            Weapon_Stat tempWeaponStat;
            DataManager.instance.weaponDict.TryGetValue(i, out tempWeaponStat);// dictionary 에서 정보 불러오기
            objectName.text = tempWeaponStat.name;
            methodText.text = tempWeaponStat.craftMethodText;

            newButton = Instantiate(newButton, panel.transform);//버튼생성
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectMethod(tempWeaponStat); });//onClick 이벤트시 실행될 함수 설정


            newText = Instantiate(newText, panel.transform);// 텍스트 생성
            newButton.transform.localPosition = new Vector3(5, -2 + -i * 5, 0);
            newText.transform.localPosition = new Vector3(25, (float)-3.3 + -i * 5, 0);

        }
    }

    public List<Craft_InputObj> InputList;// 재료아이템을 투입받는 오브젝트
    Weapon_Stat tempStat;
    public int CheckCount = 0;
    public void SelectMethod(Weapon_Stat stat)
    {
        Debug.Log("Button Clicked");
        tempStat = stat;
        int addPos = 0;
        for (int i = 0; i < stat.craftMethod.Length; i++)// 조합식 리스트를 돌면서 
        {
            if (stat.craftMethod[i] != 0)// 조합재료가 존재할때만
            {

              
                addPos++;
                GameObject inputObject = Resources.Load("Prefabs/InputObject") as GameObject;//프리팹에서 투입구불러오기
                Item_Stat tempItemStat;
                DataManager.instance.itemDict.TryGetValue(i, out tempItemStat);// 인덱스를 아이디로 dictionary에서 item정보 불러오기
                inputObject = Instantiate(inputObject, gameObject.transform);
                Craft_InputObj input = inputObject.GetComponent<Craft_InputObj>();// 투입구 생성
                input.CheckMaterialFilled += CreateWeapon;// CheckMaterialFilled 이벤트 실행시 실행될 함수 구독해주기
                input.Name.text = tempItemStat.name;
                input.countNum = stat.craftMethod[i];
                input.Count.text = "x"+input.countNum.ToString();
                InputList.Add(input);// 투입구는 재료별로 1개씩 생성되기에 생성된 투입구 리스트에 넣어주기
                inputObject.transform.localPosition = new Vector3(-35 + addPos * 35, 10, -10);

            }
        }
    }
    //CheckMaterialFilled이벤트 실행시 실행될 함수, 조합식에 따라 무기를 생성한다
    public void CreateWeapon()
    {
        Debug.Log("Weapon Created");
        CheckCount = 0;// 카운트 초기화
        Weapon_Stat newWeaponStat;
        DataManager.instance.weaponDict.TryGetValue(tempStat.Id, out newWeaponStat);// 무기정보 dictionry에서 불러오기
        GameObject newWeaponObj = Resources.Load($"Prefabs/Weapon/{newWeaponStat.Id.ToString()}") as GameObject;// 프리팹에서 무기오브젝트 불러오기
        Instantiate(newWeaponObj, gameObject.transform.position + new Vector3(-1, 1, 0), gameObject.transform.rotation);// 무기 생성
    }
}
