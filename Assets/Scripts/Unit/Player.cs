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

    public void MoveToPrevIndex(List<GameObject> TargetList)
    {
        Index--;
        if(Index<0)
        {
            Index = TargetList.Count - 1;
        }
    }

    public void ShowWeaponList()
    {
        int prevIndex = Index - 1;
        int nextIndex = Index + 1;

        Debug.Log("ShowWeaponList");

        for(int i=0; i<myWeaponList.Count; i++)
        {
            Debug.Log(myWeaponList[i].GetComponent<Weapon>().myStat.name);
        }
        if (prevIndex < 0)
        {
            prevIndex = myWeaponList.Count - 1;
        }

        if(nextIndex>=myWeaponList.Count)
        {
            nextIndex %= myWeaponList.Count;
        }

        Weapon prevWeapon = myWeaponList[prevIndex].GetComponent<Weapon>();
        Weapon nextWeapon = myWeaponList[nextIndex].GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(prevWeapon.myId, out prevWeapon.myStat);
        DataManager.instance.weaponDict.TryGetValue(nextWeapon.myId, out nextWeapon.myStat);
        playerUI.ModePreviousText.text = prevWeapon.myStat.name;
        playerUI.ModeNextText.text = nextWeapon.myStat.name;
        
       
    }

    public void ShowPalList()
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

        //if (Index - 1 >= 0)
        //{
        //    playerUI.ModePreviousText.text = monsterGoList[Index - 1].GetComponent<Monster>().myStat.name;
        //}
        //else
        //{
        //    playerUI.ModePreviousText.text = monsterGoList[monsterGoList.Count - 1].GetComponent<Monster>().myStat.name;
        //}

        //if (Index + 1 < monsterGoList.Count)
        //{
        //    playerUI.ModeNextText.text = monsterGoList[Index + 1].GetComponent<Monster>().myStat.name;
        //}
        //else
        //{
        //    playerUI.ModeNextText.text = monsterGoList[(Index + 1) % monsterGoList.Count].GetComponent<Monster>().myStat.name;
        //}
    }

    //ToDo: ó��  Weapon, Pal���� ��带 ���������� �ش����� 0���ε����� ���õǰ� ����
    public void ChooseWeapon(bool isScrollRight)
    {
        equipedWeapon.SetActive(false);
       
        if(GetComponent<ActionManager>().isScroll)
        {

            if (isScrollRight == true)
            {
                MoveToNextIndex(myWeaponList);
            }
            else
            {
                MoveToPrevIndex(myWeaponList);
            }
        }
        
        
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


        ShowWeaponList();
    

         equipedWeapon.SetActive(true);

    }
    public void ChoosePal( bool isScrollRight)
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
        playerUI.ModeText.text = monster.myStat.name;
        ShowPalList();
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
