using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] ActionBasedContinuousMoveProvider moveProvider;
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] XRInteractorLineVisual lineVisual;
    bool isTeleportMode;
    float teleportValue;
    float summonButtonValue;
    public Player player;
    public enum MoveMode// �̵����
    {
        Walk,
        Teleport
    }

    public MoveMode moveMode;
    private void Start()
    {
        isTeleportMode = true;
        moveMode = MoveMode.Teleport;//�̵���尡 �⺻��
    }
    bool isTeleportPressed;
    bool isSummonPressed;
    private void Update()
    {
        teleportValue = inputActionAsset.actionMaps[2].actions[9].ReadValue<float>();// �޼��ܼ� X��ư�� �޾ƿ��� ������������ 1, �ƴϸ� 0
        summonButtonValue = inputActionAsset.actionMaps[5].actions[9].ReadValue<float>();

      

        if (teleportValue == 1 && isTeleportPressed == false) // ��ư�� ������ ������
        {
            isTeleportMode = !isTeleportMode;// ���ٲ��ֱ�
            Debug.Log(moveMode);
            isTeleportPressed = true;
        }
      
        if(teleportValue == 0)// ��ư�� �ȴ����� ������ isPressed�� false
        {
            isTeleportPressed = false;
        }

        if(summonButtonValue==1&& isSummonPressed == false)
        {
            isSummonPressed = true;
            player.SummonPal(0);
        }

        if(summonButtonValue == 0)
        {
            isSummonPressed = false;
        }

        if (isTeleportMode == true)
        {
            moveMode = MoveMode.Teleport;
            moveProvider.enabled = false;// �ڷ���Ʈ����϶��� Walk���ϰ�
            lineVisual.reticle.SetActive(true);
        }
        else
        {
            moveMode = MoveMode.Walk;
            rayInteractor.enabled = false;// Walk����϶��� ray�� �Ⱥ��̰�
            moveProvider.enabled = true;
            lineVisual.reticle.SetActive(false);// telport��忡�� walk���� ��ȯ�� raticle�� ȭ�鿡 ���Ƽ� ��Ȱ��ȭ
        }
           
        
    }
}
