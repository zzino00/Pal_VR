using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<int> myPalList = new List<int>();// ToDo: 몬스터아이디를 저장하는 리스트를 만들기는 했는데 쓰다보니 그냥 게임오브젝트에서 찾아서 쓰는경우가 더 많아서 뺄수도 있음
    public List<GameObject> monsterGoList = new List<GameObject>();// 몬스터 오브젝트를 저장하는 리스트
    public GameObject palBall;
    public Transform summonTrs;
  //  public bool isMyPalSummoned;
    public GameObject summonedPal;
    int palIndex = 0;
    public PlayerUI playerUI;
    Monster monster;
    bool isPalChoosed= false;
    void Start()
    {
        summonedPal = Instantiate(summonedPal, summonTrs.position, Quaternion.identity);// 기존에 몬스터를 생성해놓고 시작
        summonedPal.SetActive(false);//비활성화
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

    public void ChoosePal()
    {
        palIndex++;// 인덱스를 증가시킴
        if (palIndex>=myPalList.Count)
        {
            palIndex %= myPalList.Count;// 리스트크기로 나눠서 인덱스가 리스트밖으로 안나가게
        }
        summonedPal = monsterGoList[palIndex];// 인덱스에 따라 소환할 몬스터 설정
        monster = summonedPal.GetComponent<Monster>();
        DataManager.instance.dict.TryGetValue(monster.myId, out monster.myStat);// 소환할 몬스터 스텟은 dictionary에서 id로 찾아서 넣어줌
        monster.palState = Monster.PalState.Catched;// 몬스터를 포획상태로 설정
        playerUI.BallModeText.text = monster.myStat.name;
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
