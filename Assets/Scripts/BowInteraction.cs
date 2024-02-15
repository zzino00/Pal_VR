using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class BowInteraction : XRBaseInteractable
{

    public static event Action<float> PullActionReleased;

    public Transform start, end;
    public GameObject notch;

    public float pullAmount = 0.0f;

    private LineRenderer lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;// ���� ��ȣ�ۿ��ϰ� �ִ� ��ü, IXRSelectInteractable�̶� �򰥸��� ����
                                                         // IXRSelectInteractable�̰� ��ȣ�ۿ��� ������ ��ü


    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetpullInteractor(SelectEnterEventArgs args)// ��ȣ�ۿ��� ������Ʈ ����
    {
        pullingInteractor = args.interactorObject;
      
        Debug.Log(args.interactableObject);
    }
  
    public void Release()// Ȱ�� �������� �ʱ�ȭ
    {
        PullActionReleased?.Invoke(pullAmount);// ?�� PullAcitonReleased�� null�� �ƴҶ��� Invoke��
        pullingInteractor = null;
        pullAmount = 0.0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)// ��ȣ�ۿ��߿��� �۵��Ǵ� �Լ�
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)// enum �� �����Ӹ��� ȣ��Ǵ°� Dynamic
        {
            if(isSelected)
            {
                Vector3 pullPosition = pullingInteractor.transform.position;
             //   Debug.Log(pullPosition);
                pullAmount = CalculatePull(pullPosition);
               UpdateString();
            }
        }
    }

    private float CalculatePull(Vector3 pullPosition)// Ȱ�� ��ġ ���
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;
      //  Debug.Log("pullDirection:"+pullDirection);
      //  Debug.Log("targetDirection:"+targetDirection);
      //  Debug.Log("maxLength:"+maxLength);
        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
       // Debug.Log(pullValue);
        return Mathf.Clamp(pullValue, 0, 1);

    }

    private void UpdateString()// Ȱ�� ��ġ ������Ʈ
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z+.2f);
        lineRenderer.SetPosition(1,linePosition);
       // Debug.Log(pullAmount);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
