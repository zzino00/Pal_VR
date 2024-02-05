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
    public GameObject palBallGo;
    public PlayerUI playerUI;
    Pal_Ball palBall;
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
    bool isHoldingBall;
    Vector2  rightThumbValue ;
    Vector3 rightHandPos;
    bool isRightThumb;
    private void Update()
    {
        teleportValue = inputActionAsset.actionMaps[2].actions[9].ReadValue<float>();// �޼��ܼ� X��ư�� �޾ƿ��� ������������ 1, �ƴϸ� 0
        summonButtonValue = inputActionAsset.actionMaps[5].actions[9].ReadValue<float>();
        rightThumbValue =inputActionAsset.actionMaps[5].actions[10].ReadValue<Vector2>();
       
      

        //Debug.Log(rightThumbValue);

        if(rightThumbValue.y>0.5)
        {
            isRightThumb = true;
        }
        else if(rightThumbValue.y<=0.5 && isRightThumb == true)
        {
            palBall = palBallGo.GetComponent<Pal_Ball>();
            if (palBall.ballState == Pal_Ball.BallState.Catch)
            {
                palBall.ballState = Pal_Ball.BallState.Summon;
                playerUI.BallModeText.text = "Summon";
            }
            else
            {
                palBall.ballState = Pal_Ball.BallState.Catch;
                playerUI.BallModeText.text = "Catch";
            }

            isRightThumb = false;
        }


        #region Spawing&Releasing PalBall

        if (summonButtonValue==1&& isSummonPressed == false)
        {
            isHoldingBall = !isHoldingBall;
            isSummonPressed = true;
        }
        if(summonButtonValue == 0)
        {
            isSummonPressed = false;
        }

        if (isHoldingBall)
        {
            rightHandPos = this.transform.position+inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();
            palBallGo.transform.localPosition = rightHandPos;
            player.UnsummonPal();
            palBallGo.SetActive(true);
        }

        #endregion


        #region TeleportModeChange

        if (teleportValue == 1 && isTeleportPressed == false) // ��ư�� ������ ������
        {
            isTeleportMode = !isTeleportMode;// ���ٲ��ֱ�
            isTeleportPressed = true;
        }

        if (teleportValue == 0)// ��ư�� �ȴ����� ������ isPressed�� false
        {
            isTeleportPressed = false;
        }

        if (isTeleportMode == true)
        {
            moveMode = MoveMode.Teleport;
            playerUI.moveModeText.text = "Teleport";
            moveProvider.enabled = false;// �ڷ���Ʈ����϶��� Walk���ϰ�
            lineVisual.reticle.SetActive(true);
        }
        else
        {
            moveMode = MoveMode.Walk;
            playerUI.moveModeText.text = "Walk";
            rayInteractor.enabled = false;// Walk����϶��� ray�� �Ⱥ��̰�
            moveProvider.enabled = true;
            lineVisual.reticle.SetActive(false);// telport��忡�� walk���� ��ȯ�� raticle�� ȭ�鿡 ���Ƽ� ��Ȱ��ȭ
        }

        #endregion
    }
}
