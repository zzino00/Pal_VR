using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<int> myPalList = new List<int>();// ToDo: ���;��̵� �����ϴ� ����Ʈ�� ������ �ߴµ� ���ٺ��� �׳� ���ӿ�����Ʈ���� ã�Ƽ� ���°�찡 �� ���Ƽ� ������ ����
    public List<GameObject> monsterGoList = new List<GameObject>();// ���� ������Ʈ�� �����ϴ� ����Ʈ
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
        summonedPal = Instantiate(summonedPal, summonTrs.position, Quaternion.identity);// ������ ���͸� �����س��� ����
        summonedPal.SetActive(false);//��Ȱ��ȭ
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

    public void ChoosePal()
    {
        palIndex++;// �ε����� ������Ŵ
        if (palIndex>=myPalList.Count)
        {
            palIndex %= myPalList.Count;// ����Ʈũ��� ������ �ε����� ����Ʈ������ �ȳ�����
        }
        summonedPal = monsterGoList[palIndex];// �ε����� ���� ��ȯ�� ���� ����
        monster = summonedPal.GetComponent<Monster>();
        DataManager.instance.dict.TryGetValue(monster.myId, out monster.myStat);// ��ȯ�� ���� ������ dictionary���� id�� ã�Ƽ� �־���
        monster.palState = Monster.PalState.Catched;// ���͸� ��ȹ���·� ����
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
