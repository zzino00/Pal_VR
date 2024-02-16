using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;


[Serializable]

public class Player : MonoBehaviour
{
    public List<GameObject> myWeaponList = new List<GameObject>();// 무기 오브젝트 저장 리스트
    public List<GameObject> monsterGoList = new List<GameObject>();// 몬스터 오브젝트를 저장하는 리스트
                        
    public GameObject palBall; // 위치를 받아오기위한 팔볼 오브젝트
    public Transform summonTrs; // 소환할 위치
    public GameObject summonedPal;// 소환된 팔오브젝트
    public int Index = 0;// 리스트에 사용하는 인덱스
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
            summonedPal.SetActive(true);//활성화
            summonedPal.transform.position = summonTrs.position;//소환위치는 팔볼의 위치
        }
             
    }

    public void  MoveToNextIndex(List<GameObject> TargetList)// 리스트에서 다음칸으로 이동하는 함수, 최대값을 넘지않게 설정
    {
        Index++;
        if(Index >= TargetList.Count)
        {
            Index %= TargetList.Count;
        }
    }

    public void MoveToPrevIndex(List<GameObject> TargetList)// 리스트에서 이전 칸으로 이동하는 함수, 최소값을 넘지않게 설정
    {
        
        Index--;
        if(Index<0)
        {
            Index = TargetList.Count - 1;
        }
    }

    public void ShowWeaponList()// weapon의 이전 항목과 이후항목을 보여주는 함수
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

        //ToDo: Weaon도 스텟을 플레이어가 획득했을때가 아니라 weapon의 Start에서 데이터를 불러오게 바꾸기
        Weapon prevWeapon = myWeaponList[prevIndex].GetComponent<Weapon>();
        Weapon nextWeapon = myWeaponList[nextIndex].GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(prevWeapon.myId, out prevWeapon.myStat);
        DataManager.instance.weaponDict.TryGetValue(nextWeapon.myId, out nextWeapon.myStat);
        playerUI.ModePreviousText.text = prevWeapon.myStat.name;
        playerUI.ModeNextText.text = nextWeapon.myStat.name;
        
       
    }

    public void ShowPalList()// 팰의 이전항목과 이후항목을 보여주는 함수
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
    public void ChooseWeapon(bool isScrollRight)// 무기를 리스트에서 고르는 함수
    {
        equipedWeapon.SetActive(false);
       
        if(GetComponent<ActionManager>().isScroll)
        {

            if (isScrollRight == true)// 무기 선택시 이전항목으로 넘길지 이후항목으로 넘길지 bool변수로 선택
            {
                MoveToNextIndex(myWeaponList);
            }
            else
            {
                MoveToPrevIndex(myWeaponList);
            }
        }
        
        
        equipedWeapon = myWeaponList[Index];// 정해진 인덱스의 무기를 현재무기로
        weapon = equipedWeapon.GetComponent<Weapon>();
        DataManager.instance.weaponDict.TryGetValue(weapon.myId, out weapon.myStat);// ToDo:여기도 무기값 weaopon.cs에서 가져오게하자
        if(weapon.weaponType == Weapon.WeaponType.Bow)// 타입에따라 장착하는 손이 바뀜
        {
            weapon.targetHandTrs = LeftHand.transform;
        }
        else
        {
            weapon.targetHandTrs = RightHand.transform;
        }

        weapon.weaponState = Weapon.WeaponState.Owned;// 무기소유state바꿔주기
        playerUI.ModeText.text = weapon.myStat.name;
        ShowWeaponList();
        equipedWeapon.SetActive(true);

    }
    public void ChoosePal( bool isScrollRight)// 팰을 리스트에서 고르고 소환하는 함수
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
        playerUI.ModeText.text = monster.myStat.name;// ui에 이름표시
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
