using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class BowInteraction : XRBaseInteractable// XRBaseInteractable을 상속
{

    public static event Action<float> PullActionReleased;// 활줄을 당긴 손이 놓아졌을때 실행되는 action

    public Transform start, end;// 활줄이 당겨지는 위치와, 최대로 당겨질 위치
    public GameObject notch; // 화살이 생성되는 위치

    public float pullAmount = 0.0f;// 당기는 정도

    private LineRenderer lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;// 현재 상호작용하고 있는 물체, IXRSelectInteractable이랑 헷갈리지 말것
                                                         // IXRSelectInteractable이건 상호작용이 가능한 물체


    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetpullInteractor(SelectEnterEventArgs args)// 상호작용할 오브젝트 선택, XRBaseInteractable을 함수
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
        base.ProcessInteractable(updatePhase);//부모클래스의 함수 호출
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
        Vector3 pullDirection = pullPosition - start.position;// 손의 위치 - 시작위치 = 당기는 방향
        Vector3 targetDirection = end.position - start.position;// 활줄 최대 위치 - 시작위치 = 활줄을 당겨야 하는 방향
        float maxLength = targetDirection.magnitude;// targetDirecton의 길이를 저장
        targetDirection.Normalize();// targetDirection의 정규화, 방향벡터로 만듦, 크기는 1
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;// Dot함수를 활용해 당기는 힘을 구한다
        return Mathf.Clamp(pullValue, 0, 1);//Clamp로 pullValue를 0,1사이로 제한함

    }

    private void UpdateString()// 활줄 위치 업데이트
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);// Lerp를 활용해서 당기는 힘만큼 줄이 움직이게
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z+.2f);// 화살이 시위에 걸려야 함으로 줄의 위치를 기준으로 
                                                                                                                                          // 화살이 걸리는 위치를 정함
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
