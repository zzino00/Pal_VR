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
    [SerializeField] XRBaseController rBaseController;
    bool isTeleportMode;
    float teleportValue;
    float summonButtonValue;
    public Player player;
    public GameObject palBallGo;
    public PlayerUI playerUI;
    Pal_Ball palBall;
    ModeSelect modeSelect = 0;
    public enum MoveMode// 이동모드
    {
        Walk,
        Teleport
    }

    public enum ModeSelect
    {
        Weapon,
        Pal,
        Catch,
        Inven
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
  
    float TriggerValue;
    float rightHandRotationZ;

    int palListScrollSpeedCount = 100;
    int ScrollSpeed;
    int palIndex;
    bool isRightThumbUp;
    bool isRightThumbDown;

    List<GameObject> TargetList = new List<GameObject>();
    public void ScrollList(float TriggerValue)
    {
      
        switch (modeSelect)
        {
            case ModeSelect.Weapon:
                TargetList = player.myWeaponList;
                break;
            case ModeSelect.Pal:
                TargetList = player.monsterGoList;
                break;
        }
        if (TriggerValue > 0.5 && TargetList.Count != 0)// 트리거가 당겨져있는 상태에서
        {
            if (rightHandRotationZ > 10 && rightHandRotationZ < 160)// 오른손 콘솔이 오른쪽으로 일정이상 기울어지면
            {
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// 리스트가 넘어가는 속도조절
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// 진동주기

                    switch(modeSelect)
                    {
                        case ModeSelect.Weapon:
                            player.ChooseWeapon();
                            break;
                        case ModeSelect.Pal:
                            player.ChoosePal();
                            break;
                    }

                    ScrollSpeed = 0;
                }

            }


        }
    }
    private void Update()
    {
        teleportValue = inputActionAsset.actionMaps[2].actions[9].ReadValue<float>();// 왼손콘솔 X버튼값 받아오기 누르고있으면 1, 아니면 0
        summonButtonValue = inputActionAsset.actionMaps[5].actions[9].ReadValue<float>();// 오른손 priamrybutton값
        rightThumbValue =inputActionAsset.actionMaps[5].actions[10].ReadValue<Vector2>();// 오른손 조이스틱 값
        TriggerValue = inputActionAsset.actionMaps[5].actions[11].ReadValue<float>();//오른손 트리거 값
        rightHandRotationZ = inputActionAsset.actionMaps[5].actions[12].ReadValue<Quaternion>().eulerAngles.z;// 오른손 z축 회전값


        #region SelectMonsterTo Spawn
        ScrollList(TriggerValue);

        //if (TriggerValue >0.5&&player.monsterGoList.Count !=0)// 트리거가 당겨져있는 상태에서
        //{
        //  if( rightHandRotationZ > 10 && rightHandRotationZ <160)// 오른손 콘솔이 오른쪽으로 일정이상 기울어지면
        //    {
        //        ScrollSpeed++;
        //        if(ScrollSpeed>=palListScrollSpeedCount)// 리스트가 넘어가는 속도조절
        //        {

        //            rBaseController.SendHapticImpulse(0.7f, 0.3f);// 진동주기
        //            player.ChoosePal();// 리스트 넘기는 함수
        //            ScrollSpeed = 0;
        //        }

        //    }


        //}
        #endregion



        #region SelectMode // 모드 선택

        if (rightThumbValue.y > 0.5) // 오른손 조이스틱이 위쪽으로 밀려있는 상태에서
        {
            isRightThumbUp = true;
        }
        else if (rightThumbValue.y <= 0.5 && isRightThumbUp == true)// 다시 제자리로 돌아오면
        {
            modeSelect++;
            if((int)modeSelect >3)
            {
                modeSelect = 0;
            }

            Debug.Log(modeSelect);
            player.Index = 0;
            isRightThumbUp = false;

            if (modeSelect != ModeSelect.Weapon)
            {
                player.equipedWeapon.SetActive(false);
            }
            playerUI.ModeText.text = modeSelect.ToString();
        }

        if (rightThumbValue.y < -0.5) // 오른손 조이스틱이 위쪽으로 밀려있는 상태에서
        {
            isRightThumbDown = true;
        }
        else if (rightThumbValue.y >= -0.5 && isRightThumbDown == true)// 다시 제자리로 돌아오면
        {
            modeSelect--;
            if ((int)modeSelect < 0)
            {
                modeSelect = (ModeSelect)3;
            }

            Debug.Log(modeSelect);
            player.Index = 0;
            isRightThumbDown = false;
            if (modeSelect != ModeSelect.Weapon)
            {
                player.equipedWeapon.SetActive(false);
            }
            playerUI.ModeText.text = modeSelect.ToString();
        }

    
        #endregion

        #region SelectPalBallMode

        if(modeSelect == ModeSelect.Catch)
        {
            palBall = palBallGo.GetComponent<Pal_Ball>();
            palBall.ballState = Pal_Ball.BallState.Catch;
            
        }

        if (modeSelect == ModeSelect.Pal)
        {
            palBall = palBallGo.GetComponent<Pal_Ball>();
            palBall.ballState = Pal_Ball.BallState.Summon;
        }
        //if (rightThumbValue.y>0.5) // 오른손 조이스틱이 위쪽으로 밀려있는 상태에서
        //{
        //    isRightThumb = true;
        //}
        //else if(rightThumbValue.y<=0.5 && isRightThumb == true)// 다시 제자리로 돌아오면
        //{
        //    palBall = palBallGo.GetComponent<Pal_Ball>();
        //    if (palBall.ballState == Pal_Ball.BallState.Catch)// 팔볼의 상태가 바뀜
        //    {
        //        palBall.ballState = Pal_Ball.BallState.Summon;
        //        playerUI.BallModeText.text = "Summon";
        //    }
        //    else
        //    {
        //        palBall.ballState = Pal_Ball.BallState.Catch;
        //        playerUI.BallModeText.text = "Catch";
        //    }

        //    isRightThumb = false;
        //}
        #endregion


        #region Spawing&Releasing PalBall

        if(modeSelect == ModeSelect.Catch || modeSelect == ModeSelect.Pal)
        {
            if (summonButtonValue == 1 && isSummonPressed == false)// 버튼을 누르고 있는 값이 float값으로 들어오기
                                                                   // 때문에 업데이트에서 1번만 실행되게 하려면 이러한 방법을 써야한다.
            {
                isHoldingBall = !isHoldingBall; // 공을 소환한 상태
                isSummonPressed = true;
            }
            if (summonButtonValue == 0)
            {
                isSummonPressed = false;
            }

            if (isHoldingBall)
            {
                rightHandPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// 오른손 컨트롤러 값 받아오기
                palBallGo.transform.localPosition = rightHandPos;
                player.UnsummonPal();// 공을 손으로 가져올때는 몬스터 역소환 ToDo: 내 몬스터는 소환한 상태로 포획용 볼을 꺼낼수도 있으니까 수정해야할수도
                palBallGo.SetActive(true);
            }
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
