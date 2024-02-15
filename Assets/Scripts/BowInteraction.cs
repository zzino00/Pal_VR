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
    private IXRSelectInteractor pullingInteractor = null;// 현재 상호작용하고 있는 물체, IXRSelectInteractable이랑 헷갈리지 말것
                                                         // IXRSelectInteractable이건 상호작용이 가능한 물체


    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetpullInteractor(SelectEnterEventArgs args)// 상호작용할 오브젝트 선택
    {
        pullingInteractor = args.interactorObject;
      
        Debug.Log(args.interactableObject);
    }
  
    public void Release()// 활줄 놓았을때 초기화
    {
        PullActionReleased?.Invoke(pullAmount);// ?는 PullAcitonReleased가 null이 아닐때만 Invoke함
        pullingInteractor = null;
        pullAmount = 0.0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)// 상호작용중에만 작동되는 함수
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)// enum 매 프래임마다 호출되는게 Dynamic
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

    private float CalculatePull(Vector3 pullPosition)// 활줄 위치 계산
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

    private void UpdateString()// 활줄 위치 업데이트
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
