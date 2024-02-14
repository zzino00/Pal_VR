using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    Player player;
    public Weapon_Stat myStat;// ���� ����
    public WeaponState weaponState = WeaponState.Unowned;
    public int myId;
    public Transform targetHandTrs;
   public enum WeaponType// ToDo: ����Ÿ���� ������ json���� ����Ǿ������ϱ� �׳� ���Ÿ� �ٰŸ��� ������
    {
        Sword,
        Bow
    }
    public enum WeaponState// ���������� �ʵ忡 �ִ���
    {
        Unowned,
        Owned
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (weaponState == WeaponState.Unowned && other.gameObject.tag == "RightHand")
        {
            player.myWeaponList.Add(gameObject);// �÷��̾� weapon����Ʈ�� �߰�
            gameObject.SetActive(false);
        }
    }
    public void Update()
    {
        if(weaponState==WeaponState.Owned)// �÷��̾� �տ� ����
        {
            gameObject.transform.position = targetHandTrs.position;
        }
    }
}
