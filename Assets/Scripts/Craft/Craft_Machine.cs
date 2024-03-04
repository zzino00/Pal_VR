using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Craft_Machine : MonoBehaviour
{
    public GameObject panel;//�����г�

    public void ShowCraftableWeaponList()// ���۰����� �������� �гο� ���� �Լ�
    {
        for (int i = 1; i <= DataManager.instance.weaponDict.Count; i++)
        {
            GameObject newButton = Resources.Load("Prefabs/UI/TargetObject") as GameObject; //���������� ��ư�ҷ�����
            GameObject newText = Resources.Load("Prefabs/UI/CraftMethod") as GameObject;// ���������� �ý�Ʈ�ҷ�����
            TMP_Text objectName = newButton.GetComponentInChildren<TMP_Text>();
            TMP_Text methodText = newText.GetComponentInChildren<TMP_Text>();
            Weapon_Stat tempWeaponStat;
            DataManager.instance.weaponDict.TryGetValue(i, out tempWeaponStat);// dictionary ���� ���� �ҷ�����
            objectName.text = tempWeaponStat.name;
            methodText.text = tempWeaponStat.craftMethodText;

            newButton = Instantiate(newButton, panel.transform);//��ư����
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectMethod(tempWeaponStat); });//onClick �̺�Ʈ�� ����� �Լ� ����


            newText = Instantiate(newText, panel.transform);// �ؽ�Ʈ ����
            newButton.transform.localPosition = new Vector3(5, -2 + -i * 5, 0);
            newText.transform.localPosition = new Vector3(25, (float)-3.3 + -i * 5, 0);

        }
    }

    public List<Craft_InputObj> InputList;// ���������� ���Թ޴� ������Ʈ
    Weapon_Stat tempStat;
    public int CheckCount = 0;
    public void SelectMethod(Weapon_Stat stat)
    {
        Debug.Log("Button Clicked");
        tempStat = stat;
        int addPos = 0;
        for (int i = 0; i < stat.craftMethod.Length; i++)// ���ս� ����Ʈ�� ���鼭 
        {
            if (stat.craftMethod[i] != 0)// ������ᰡ �����Ҷ���
            {

              
                addPos++;
                GameObject inputObject = Resources.Load("Prefabs/InputObject") as GameObject;//�����տ��� ���Ա��ҷ�����
                Item_Stat tempItemStat;
                DataManager.instance.itemDict.TryGetValue(i, out tempItemStat);// �ε����� ���̵�� dictionary���� item���� �ҷ�����
                inputObject = Instantiate(inputObject, gameObject.transform);
                Craft_InputObj input = inputObject.GetComponent<Craft_InputObj>();// ���Ա� ����
                input.CheckMaterialFilled += CreateWeapon;// CheckMaterialFilled �̺�Ʈ ����� ����� �Լ� �������ֱ�
                input.Name.text = tempItemStat.name;
                input.countNum = stat.craftMethod[i];
                input.Count.text = "x"+input.countNum.ToString();
                InputList.Add(input);// ���Ա��� ��Ằ�� 1���� �����Ǳ⿡ ������ ���Ա� ����Ʈ�� �־��ֱ�
                inputObject.transform.localPosition = new Vector3(-35 + addPos * 35, 10, -10);

            }
        }
    }
    //CheckMaterialFilled�̺�Ʈ ����� ����� �Լ�, ���սĿ� ���� ���⸦ �����Ѵ�
    public void CreateWeapon()
    {
        Debug.Log("Weapon Created");
        CheckCount = 0;// ī��Ʈ �ʱ�ȭ
        Weapon_Stat newWeaponStat;
        DataManager.instance.weaponDict.TryGetValue(tempStat.Id, out newWeaponStat);// �������� dictionry���� �ҷ�����
        GameObject newWeaponObj = Resources.Load($"Prefabs/Weapon/{newWeaponStat.Id.ToString()}") as GameObject;// �����տ��� ���������Ʈ �ҷ�����
        Instantiate(newWeaponObj, gameObject.transform.position + new Vector3(-1, 1, 0), gameObject.transform.rotation);// ���� ����
    }
}
