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
        if(player.ammo>0&&bow.isSelected && isArrowNotched == false)// Ȱ�� ���õ� ���¿��� ȭ���� �ɷ����� �������� ȭ�� ����
        {
            isArrowNotched = true;
            StartCoroutine("DelaySpawn");
          
        }
        if(bow.isSelected)
        {
            ArrowCount.text = "Arrow:" + player.ammo;
        }
        if(!bow.isSelected && currentArrow!=null)// Ȱ�� �����Ǿ������ʰ� ���� ȭ���� �����Ѵٸ� ȭ���� �ı�
        {
            Destroy(currentArrow);
            NotchEmpty(1f);// PullActionRelease �׼��� �Ű������� float�� �ޱ� ������ ��¿������ �ƹ����̳� ����
        }
    }
    IEnumerator DelaySpawn()// ȭ����� ������
    {
        yield return new WaitForSeconds(1);
        currentArrow = Instantiate(arrow, notch.transform);
        currentArrow.GetComponent<Weapon>().weaponState = Weapon.WeaponState.Owned;
        player.ammo--;
    }

}
