using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    Player player;
    public Weapon_Stat myStat;
    public WeaponState weaponState = WeaponState.Unowned;
    public int myId;
    public Transform targetHandTrs;
   public enum WeaponType
    {
        Sword,
        Bow
    }

    public enum WeaponState
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
            player.myWeaponList.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if(weaponState==WeaponState.Owned)
        {
            gameObject.transform.position = targetHandTrs.position;
        }
    }
}
