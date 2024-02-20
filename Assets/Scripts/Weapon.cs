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
        Bow,
        Ammo
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
            player.equipedWeapon = player.myWeaponList[0];
            gameObject.SetActive(false);
        }

        if(weaponState == WeaponState.Unowned && weaponType == WeaponType.Ammo&& other.gameObject.tag == "RightHand")
        {
            player.ammo += 10;
        }
    }
    public void Update()
    {
        if (weaponType != WeaponType.Ammo&&weaponState == WeaponState.Owned)// �÷��̾� �տ� ����
        {
            gameObject.transform.position = targetHandTrs.position;
        }
    }
}
