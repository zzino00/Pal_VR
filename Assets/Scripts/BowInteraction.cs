using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class BowInteraction : XRBaseInteractable// XRBaseInteractable�� ���
{

    public static event Action<float> PullActionReleased;// Ȱ���� ��� ���� ���������� ����Ǵ� action

    public Transform start, end;// Ȱ���� ������� ��ġ��, �ִ�� ����� ��ġ
    public GameObject notch; // ȭ���� �����Ǵ� ��ġ

    public float pullAmount = 0.0f;// ���� ����

    private LineRenderer lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;// ���� ��ȣ�ۿ��ϰ� �ִ� ��ü, IXRSelectInteractable�̶� �򰥸��� ����
                                                         // IXRSelectInteractable�̰� ��ȣ�ۿ��� ������ ��ü


    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetpullInteractor(SelectEnterEventArgs args)// ��ȣ�ۿ��� ������Ʈ ����, XRBaseInteractable�� �Լ�
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
        base.ProcessInteractable(updatePhase);//�θ�Ŭ������ �Լ� ȣ��
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
        Vector3 pullDirection = pullPosition - start.position;// ���� ��ġ - ������ġ = ���� ����
        Vector3 targetDirection = end.position - start.position;// Ȱ�� �ִ� ��ġ - ������ġ = Ȱ���� ��ܾ� �ϴ� ����
        float maxLength = targetDirection.magnitude;// targetDirecton�� ���̸� ����
        targetDirection.Normalize();// targetDirection�� ����ȭ, ���⺤�ͷ� ����, ũ��� 1
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;// Dot�Լ��� Ȱ���� ���� ���� ���Ѵ�
        return Mathf.Clamp(pullValue, 0, 1);//Clamp�� pullValue�� 0,1���̷� ������

    }

    private void UpdateString()// Ȱ�� ��ġ ������Ʈ
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);// Lerp�� Ȱ���ؼ� ���� ����ŭ ���� �����̰�
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z+.2f);// ȭ���� ������ �ɷ��� ������ ���� ��ġ�� �������� 
                                                                                                                                          // ȭ���� �ɸ��� ��ġ�� ����
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
