using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] TeleportationProvider teleportProvider;
    [SerializeField] ActionManager actionManager;
    [SerializeField] ActionBasedContinuousMoveProvider actionBasedContinuousMoveProvider;
    private InputAction _thumstick;
    private bool _isActive;
    private void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
    private void Start()
    {
        rayInteractor.enableARRaycasting = false;
        var activate = inputActionAsset.actionMaps[3].actions[1];//InputAction들을 변수로 저장해서
        activate.Enable();// 사용할수 있게 설정
        activate.performed += OnTeleportActivate;// 실행됐을때 이벤트 실행
        var cancel = inputActionAsset.actionMaps[3].actions[2];
        cancel.Enable();
        cancel.performed += OnTeleportCancel;
        _thumstick = inputActionAsset.actionMaps[3].actions[5];
        _thumstick.Enable();
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;// 입력이 들어오고 있을때만 ray가 표시되게 설정
        _isActive = true;// 입력중에만 텔레포트를 작동시킬수 있게
    }
    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
    void Update()
    {
        if (actionManager.moveMode != ActionManager.MoveMode.Teleport)// 이동모드가 Teleport가 아니면 이동못하게
            return;

        if(!_isActive)
         return;


        if (_thumstick.IsPressed())// 누르고있는 중에는 이동못하게
            return;

      

     if(! rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))// 텔레포트할려고 레이케스트 했는데 어디에도 부딪히지 않았을때
        {
            rayInteractor.enabled = false; 
            _isActive = false;
            return;
        }

       // Debug.Log(_thumstick.triggered);

        TeleportRequest request = new TeleportRequest();

        request.destinationPosition = hit.point;// 이동할장소를 ray가 부딪힌 장소로 설정

        Debug.Log("Teleport");

        teleportProvider.QueueTeleportRequest(request);// 이동시키기
        _isActive = false;// 이동시킨후에는 꺼주기
        rayInteractor.enabled = false;
    }


    
}
