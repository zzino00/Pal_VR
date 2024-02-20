using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    Player player;
    public Weapon_Stat myStat;// 무기 스텟
    public WeaponState weaponState = WeaponState.Unowned;
    public int myId;
    public Transform targetHandTrs;
   public enum WeaponType// ToDo: 무기타입은 어차피 json으로 저장되어있으니까 그냥 원거리 근거리로 나눌것
    {
        Sword,
        Bow,
        Ammo
    }
    public enum WeaponState// 소유중인지 필드에 있는지
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
            player.myWeaponList.Add(gameObject);// 플레이어 weapon리스트에 추가
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
        if (weaponType != WeaponType.Ammo&&weaponState == WeaponState.Owned)// 플레이어 손에 고정
        {
            gameObject.transform.position = targetHandTrs.position;
        }
    }
}
