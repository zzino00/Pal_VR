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
    public enum MoveMode// 이동모드
    {
        Walk,
        Teleport
    }

    public MoveMode moveMode;
    private void Start()
    {
        isTeleportMode = true;
        moveMode = MoveMode.Teleport;//이동모드가 기본값
    }
    bool isTeleportPressed;
    bool isSummonPressed;
    bool isHoldingBall;
    Vector2  rightThumbValue ;
    Vector3 rightHandPos;
    bool isRightThumb;
    private void Update()
    {
        teleportValue = inputActionAsset.actionMaps[2].actions[9].ReadValue<float>();// 왼손콘솔 X버튼값 받아오기 누르고있으면 1, 아니면 0
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

        if (teleportValue == 1 && isTeleportPressed == false) // 버튼이 눌려진 순간만
        {
            isTeleportMode = !isTeleportMode;// 모드바꿔주기
            isTeleportPressed = true;
        }

        if (teleportValue == 0)// 버튼을 안누르고 있으면 isPressed는 false
        {
            isTeleportPressed = false;
        }

        if (isTeleportMode == true)
        {
            moveMode = MoveMode.Teleport;
            playerUI.moveModeText.text = "Teleport";
            moveProvider.enabled = false;// 텔레포트모드일때는 Walk못하게
            lineVisual.reticle.SetActive(true);
        }
        else
        {
            moveMode = MoveMode.Walk;
            playerUI.moveModeText.text = "Walk";
            rayInteractor.enabled = false;// Walk모드일때는 ray가 안보이게
            moveProvider.enabled = true;
            lineVisual.reticle.SetActive(false);// telport모드에서 walk모드로 번환시 raticle이 화면에 남아서 비활성화
        }

        #endregion
    }
}
