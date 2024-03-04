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
        var activate = inputActionAsset.actionMaps[3].actions[1];//InputAction���� ������ �����ؼ�
        activate.Enable();// ����Ҽ� �ְ� ����
        activate.performed += OnTeleportActivate;// ��������� �̺�Ʈ ����
        var cancel = inputActionAsset.actionMaps[3].actions[2];
        cancel.Enable();
        cancel.performed += OnTeleportCancel;
        _thumstick = inputActionAsset.actionMaps[3].actions[5];
        _thumstick.Enable();
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;// �Է��� ������ �������� ray�� ǥ�õǰ� ����
        _isActive = true;// �Է��߿��� �ڷ���Ʈ�� �۵���ų�� �ְ�
    }
    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
    void Update()
    {
        if (actionManager.moveMode != ActionManager.MoveMode.Teleport)// �̵���尡 Teleport�� �ƴϸ� �̵����ϰ�
            return;

        if(!_isActive)
         return;


        if (_thumstick.IsPressed())// �������ִ� �߿��� �̵����ϰ�
            return;

      

     if(! rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))// �ڷ���Ʈ�ҷ��� �����ɽ�Ʈ �ߴµ� ��𿡵� �ε����� �ʾ�����
        {
            rayInteractor.enabled = false; 
            _isActive = false;
            return;
        }

       // Debug.Log(_thumstick.triggered);

        TeleportRequest request = new TeleportRequest();

        request.destinationPosition = hit.point;// �̵�����Ҹ� ray�� �ε��� ��ҷ� ����

        Debug.Log("Teleport");

        teleportProvider.QueueTeleportRequest(request);// �̵���Ű��
        _isActive = false;// �̵���Ų�Ŀ��� ���ֱ�
        rayInteractor.enabled = false;
    }


    
}
