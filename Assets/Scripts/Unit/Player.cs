using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

  //  public List<int> myPalList = new List<int>();// ToDo: ���;��̵� �����ϴ� ����Ʈ�� ������ �ߴµ� ���ٺ��� �׳� ���ӿ�����Ʈ���� ã�Ƽ� ���°�찡 �� ���Ƽ� ������ ����
    public List<GameObject> myWeaponList = new List<GameObject>();
    public List<GameObject> monsterGoList = new List<GameObject>();// ���� ������Ʈ�� �����ϴ� ����Ʈ
    public GameObject palBall;
    public Transform summonTrs;
    public GameObject summonedPal;
    public int Index = 0;
    public PlayerUI playerUI;
    Monster monster;
    Weapon weapon;
    bool isPalChoosed= false;
    public GameObject equipedWeapon;
    public GameObject RightHand;
    public GameObject LeftHand;
    void Start()
    {
    //    summonedPal = Instantiate(summonedPal, summonTrs.position, Quaternion.identity);// ������ ���͸� �����س��� ����
     //   summonedPal.SetActive(false);//��Ȱ��ȭ
     //   equipedWeapon = Instantiate(equipedWeapon);
     //   equipedWeapon.SetActive(false);

    }
    public void SummonPal()
    {
        if (monsterGoList.Count != 0 && isPalChoosed == true)
        {
            summonedPal.SetActive(true);//Ȱ��ȭ
            summonedPal.transform.position = summonTrs.position;//��ȯ��ġ�� �Ⱥ��� ��ġ
        }
             
           //   isMyPalSummoned = true;

    }

    public void  MoveToNextIndex(List<GameObject> TargetList)
    {
        Index++;
        if(Index >= TargetList.Count)
        {
            Index %= TargetList.Count;
        }
    }

    public void ChooseWeapon()
    {
        equipedWeapon.SetActive(false);
        MoveToNextIndex(myWeaponList);
        equipedWeapon = myWeaponList[Index];
        weapon = equipedWeapon.GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(weapon.myId, out weapon.myStat);
        if(weapon.weaponType == Weapon.WeaponType.Bow)
        {
            weapon.targetHandTrs = LeftHand.transform;
        }
        else
        {
            weapon.targetHandTrs = RightHand.transform;
        }
        weapon.weaponState = Weapon.WeaponState.Owned;
        playerUI.ModeText.text = weapon.myStat.name;
        equipedWeapon.SetActive(true);

    }
    public void ChoosePal()
    {
        MoveToNextIndex(monsterGoList);
     
        summonedPal = monsterGoList[Index];// �ε����� ���� ��ȯ�� ���� ����
        monster = summonedPal.GetComponent<Monster>();
        DataManager.instance.palDict.TryGetValue(monster.myId, out monster.myStat);// ��ȯ�� ���� ������ dictionary���� id�� ã�Ƽ� �־���
        monster.palState = Monster.PalState.Catched;// ���͸� ��ȹ���·� ����
        playerUI.ModeText.text = monster.myStat.name;
        isPalChoosed = true;
    }

    public void UnsummonPal()
    {
        summonedPal.SetActive(false);
     
    }

    // Update is called once per frame
    void Update()
    {
        summonTrs = palBall.transform;
    }
}
