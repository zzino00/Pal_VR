using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;
    private XRGrabInteractable bow;
    private bool isArrowNotched = false;
    private GameObject currentArrow = null;
    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        BowInteraction.PullActionReleased += NotchEmpty;
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
        if(bow.isSelected && isArrowNotched == false)// Ȱ�� ���õ� ���¿��� ȭ���� �ɷ����� �������� ȭ�� ����
        {
            isArrowNotched = true;
            StartCoroutine("DelaySpawn");
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
    }

}