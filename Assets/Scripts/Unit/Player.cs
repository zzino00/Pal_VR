using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;


[Serializable]

public class Player : MonoBehaviour
{
    public List<GameObject> myWeaponList = new List<GameObject>();// ���� ������Ʈ ���� ����Ʈ
    public List<GameObject> monsterGoList = new List<GameObject>();// ���� ������Ʈ�� �����ϴ� ����Ʈ
                        
    public GameObject palBall; // ��ġ�� �޾ƿ������� �Ⱥ� ������Ʈ
    public Transform summonTrs; // ��ȯ�� ��ġ
    public GameObject summonedPal;// ��ȯ�� �ȿ�����Ʈ
    public int Index = 0;// ����Ʈ�� ����ϴ� �ε���
    public PlayerUI playerUI;
    Monster monster;
    Weapon weapon;
    public GameObject equipedWeapon;
    public GameObject RightHand;
    public GameObject LeftHand;


  
    public void SummonPal()
    {
        if (monsterGoList.Count != 0 )
        {
            summonedPal.SetActive(true);//Ȱ��ȭ
            summonedPal.transform.position = summonTrs.position;//��ȯ��ġ�� �Ⱥ��� ��ġ
        }
             
    }

    public void  MoveToNextIndex(List<GameObject> TargetList)// ����Ʈ���� ����ĭ���� �̵��ϴ� �Լ�, �ִ밪�� �����ʰ� ����
    {
        Index++;
        if(Index >= TargetList.Count)
        {
            Index %= TargetList.Count;
        }
    }

    public void MoveToPrevIndex(List<GameObject> TargetList)// ����Ʈ���� ���� ĭ���� �̵��ϴ� �Լ�, �ּҰ��� �����ʰ� ����
    {
        
        Index--;
        if(Index<0)
        {
            Index = TargetList.Count - 1;
        }
    }

    public void ShowWeaponList()// weapon�� ���� �׸�� �����׸��� �����ִ� �Լ�
    {
        int prevIndex = Index - 1;
        int nextIndex = Index + 1;

        if (prevIndex < 0)
        {
            prevIndex = myWeaponList.Count - 1;
        }

        if(nextIndex>= myWeaponList.Count)
        {
            nextIndex %= myWeaponList.Count;
        }

        //ToDo: Weaon�� ������ �÷��̾ ȹ���������� �ƴ϶� weapon�� Start���� �����͸� �ҷ����� �ٲٱ�
        Weapon prevWeapon = myWeaponList[prevIndex].GetComponent<Weapon>();
        Weapon nextWeapon = myWeaponList[nextIndex].GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(prevWeapon.myId, out prevWeapon.myStat);
        DataManager.instance.weaponDict.TryGetValue(nextWeapon.myId, out nextWeapon.myStat);
        playerUI.ModePreviousText.text = prevWeapon.myStat.name;
        playerUI.ModeNextText.text = nextWeapon.myStat.name;
        
       
    }

    public void ShowPalList()// ���� �����׸�� �����׸��� �����ִ� �Լ�
    {


        int prevIndex = Index - 1;
        int nextIndex = Index + 1;

        if (prevIndex < 0)
        {
            prevIndex = monsterGoList.Count - 1;
        }

        if (nextIndex >= monsterGoList.Count)
        {
            nextIndex %= monsterGoList.Count;
        }
        playerUI.ModePreviousText.text = monsterGoList[prevIndex].GetComponent<Monster>().myStat.name;
        playerUI.ModeNextText.text = monsterGoList[nextIndex].GetComponent<Monster>().myStat.name;

     
    }
    public void ChooseWeapon(bool isScrollRight)// ���⸦ ����Ʈ���� ���� �Լ�
    {
        equipedWeapon.SetActive(false);
       
        if(GetComponent<ActionManager>().isScroll)
        {

            if (isScrollRight == true)// ���� ���ý� �����׸����� �ѱ��� �����׸����� �ѱ��� bool������ ����
            {
                MoveToNextIndex(myWeaponList);
            }
            else
            {
                MoveToPrevIndex(myWeaponList);
            }
        }
        
        
        equipedWeapon = myWeaponList[Index];// ������ �ε����� ���⸦ ���繫���
        weapon = equipedWeapon.GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(weapon.myId, out weapon.myStat);// ToDo:���⵵ ���Ⱚ weaopon.cs���� ������������
        if(weapon.weaponType == Weapon.WeaponType.Bow)// Ÿ�Կ����� �����ϴ� ���� �ٲ�
        {
            weapon.targetHandTrs = LeftHand.transform;
        }
        else
        {
            weapon.targetHandTrs = RightHand.transform;
        }

        weapon.weaponState = Weapon.WeaponState.Owned;// �������state�ٲ��ֱ�
        playerUI.ModeText.text = weapon.myStat.name;
        ShowWeaponList();
        equipedWeapon.SetActive(true);

    }
    public void ChoosePal( bool isScrollRight)// ���� ����Ʈ���� ���� ��ȯ�ϴ� �Լ�
    {
        if (GetComponent<ActionManager>().isScroll)
        {
            if (isScrollRight == true)
            {
                MoveToNextIndex(monsterGoList);
            }
            else
            {
                MoveToPrevIndex(monsterGoList);
            }

        }
     
        summonedPal = monsterGoList[Index];// �ε����� ���� ��ȯ�� ���� ����
        monster = summonedPal.GetComponent<Monster>();
        DataManager.instance.palDict.TryGetValue(monster.myId, out monster.myStat);// ��ȯ�� ���� ������ dictionary���� id�� ã�Ƽ� �־���
        monster.palState = Monster.PalState.Catched;// ���͸� ��ȹ���·� ����
        playerUI.ModeText.text = monster.myStat.name;// ui�� �̸�ǥ��
        ShowPalList();
       
    }

    public void UnsummonPal()
    {
        summonedPal.SetActive(false);
     
    }
    void Update()
    {
        summonTrs = palBall.transform;
    }
}
