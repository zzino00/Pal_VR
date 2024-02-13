using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

  //  public List<int> myPalList = new List<int>();// ToDo: 몬스터아이디를 저장하는 리스트를 만들기는 했는데 쓰다보니 그냥 게임오브젝트에서 찾아서 쓰는경우가 더 많아서 뺄수도 있음
    public List<GameObject> myWeaponList = new List<GameObject>();
    public List<GameObject> monsterGoList = new List<GameObject>();// 몬스터 오브젝트를 저장하는 리스트
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
    //    summonedPal = Instantiate(summonedPal, summonTrs.position, Quaternion.identity);// 기존에 몬스터를 생성해놓고 시작
     //   summonedPal.SetActive(false);//비활성화
     //   equipedWeapon = Instantiate(equipedWeapon);
     //   equipedWeapon.SetActive(false);

    }
    public void SummonPal()
    {
        if (monsterGoList.Count != 0 && isPalChoosed == true)
        {
            summonedPal.SetActive(true);//활성화
            summonedPal.transform.position = summonTrs.position;//소환위치는 팔볼의 위치
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

    //ToDo: 처음  Weapon, Pal같은 모드를 선택했을때 해당모드의 0번인덱스가 선택되게 설정
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
     
        summonedPal = monsterGoList[Index];// 인덱스에 따라 소환할 몬스터 설정
        monster = summonedPal.GetComponent<Monster>();
        DataManager.instance.palDict.TryGetValue(monster.myId, out monster.myStat);// 소환할 몬스터 스텟은 dictionary에서 id로 찾아서 넣어줌
        monster.palState = Monster.PalState.Catched;// 몬스터를 포획상태로 설정
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
