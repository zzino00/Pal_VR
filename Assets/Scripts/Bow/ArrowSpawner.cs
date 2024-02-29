using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;
    private XRGrabInteractable bow;
    private bool isArrowNotched = false;
    private GameObject currentArrow = null;
    public Player player;
    public TMP_Text ArrowCount;
    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        BowInteraction.PullActionReleased += NotchEmpty;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnDestroy()
    {
        BowInteraction.PullActionReleased -= NotchEmpty;
    }
    private void NotchEmpty(float value)
    {
        isArrowNotched = false;
        currentArrow = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.ammo>0&&bow.isSelected && isArrowNotched == false)// 활이 선택된 상태에서 화살이 걸려있지 않을때만 화살 생성
        {
            isArrowNotched = true;
            StartCoroutine("DelaySpawn");
          
        }
        if(bow.isSelected)
        {
            ArrowCount.text = "Arrow:" + player.ammo;
        }
        if(!bow.isSelected && currentArrow!=null)// 활이 생성되어있지않고 현재 화살이 존재한다면 화살을 파괴
        {
            Destroy(currentArrow);
            NotchEmpty(1f);// PullActionRelease 액션이 매개변수로 float을 받기 때문에 어쩔수없이 아무값이나 넣음
        }
    }
    IEnumerator DelaySpawn()// 화살생성 딜레이
    {
        yield return new WaitForSeconds(1);
        currentArrow = Instantiate(arrow, notch.transform);
        currentArrow.GetComponent<Weapon>().weaponState = Weapon.WeaponState.Owned;
        player.ammo--;
    }

}
