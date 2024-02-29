using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;// InputAction을 받아오기위한 클래스
    [SerializeField] ActionBasedContinuousMoveProvider moveProvider;// 걷는 모드 관련 클래스
    [SerializeField] XRRayInteractor rayInteractorL;// 레이 인디게이터 관련 클래스
    [SerializeField] XRRayInteractor rayInteractorR;// 레이 인디게이터 관련 클래스
    [SerializeField] XRInteractorLineVisual lineVisual;// 레이를 어떻게 화면에 표시할지 관련 클래스
    [SerializeField] XRBaseController rBaseController;// 오른손 콘솔의 회전값을 받아오기위한 클래스
    bool isTeleportMode; // 텔레포트 모드
    bool isHoldingBall; // 팰볼이 소환된 상태처리를 위한 변수
    bool isShowingMenu;
    public Player player;
    public GameObject palBallGo;
    public PlayerUI playerUI;
    public GameObject MenuCanvas;
    public GameObject ModeMenuCanvas;
    Pal_Ball palBall;
    public ModeSelect modeSelect = 0;
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

    InputAction teleportModeChange;// teleport모드버튼 입력값받는 변수
    InputAction palBallSpawn;//팔볼소환입력값 받는 변수
    InputAction showMenu;// 메뉴창 열기
    bool isTeleportModeSelectActive; // 한번만 함수가 실행되게 하는 변수
    bool isPalBallSpawnActive; // 한번만 함수가 실행되게 하는 변수
    bool isShowMenuActive;
    public MoveMode moveMode;
    private void Start()
    {
        isTeleportModeSelectActive = false;
        isPalBallSpawnActive = false;
        isShowMenuActive = false;
        isTeleportMode = true;
    
        teleportModeChange = inputActionAsset.actionMaps[2].actions[9];
        palBallSpawn = inputActionAsset.actionMaps[5].actions[9];
        showMenu = inputActionAsset.actionMaps[5].actions[13];
        teleportModeChange.performed += TeleportModeSelect;
        palBallSpawn.performed += PalBallSpawn;
        showMenu.performed += ShowMenu;

        modeSelect = ModeSelect.Catch;
        playerUI.ModeDownText.gameObject.SetActive(false);
        playerUI.ModeUpText.gameObject.SetActive(false);
        playerUI.ModeText.text = modeSelect.ToString();
      

        moveMode = MoveMode.Teleport;//이동모드가 기본값
        playerUI.moveModeText.text = "Teleport";
        playerUI.ModeText.text = "Start";
        playerUI.ModeNextText.text = "";
        playerUI.ModePreviousText.text = "";

        moveProvider.enabled = false;// 텔레포트모드일때는 Walk못하게
        lineVisual.enabled = true;
        lineVisual.reticle.SetActive(true);
    }

    private void TeleportModeSelect(InputAction.CallbackContext context)
    {
        isTeleportModeSelectActive = true;
        isTeleportMode = !isTeleportMode;
     
    }

    private void PalBallSpawn(InputAction.CallbackContext context)
    {
        isPalBallSpawnActive = true;
        isHoldingBall = !isHoldingBall;

    }

    Vector3 MenuPos;
    private void ShowMenu(InputAction.CallbackContext context)
    {
        MenuPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// 오른손 컨트롤러 값 받아오기
        MenuCanvas.transform.rotation = inputActionAsset.actionMaps[4].actions[1].ReadValue<Quaternion>();
        isShowMenuActive = true;
        isShowingMenu = !isShowingMenu;
    }

   
    Vector2  rightThumbValue ;
    Vector3 rightHandPos;
  
    float TriggerValue;// 트리거가 당겨지고 있는지
    float rightHandRotationZ;// 리스트 넘길때 쓰이는 오른손컨트롤러z축 회전값

    int palListScrollSpeedCount = 100;// 리스트 넘어가는 속도 
    int ScrollSpeed;// 리스트 넘어가는 속도 조절을 위한 카운트
  
    bool isRightThumbUp;// 오른손 조이스틱 윗방향 입력값
    bool isRightThumbDown;// 오른손 조이스틱 아랫방향 입력값
    bool isScrollRight;// 리스트를 오른쪽으로 넘길지 왼쪽으로 넘길지 정한는 변수
    public bool isScroll;// 리스트에서 다음 항목으로 넘길지 말지를 정하는 변수

    List<GameObject> TargetList = new List<GameObject>();// 상황에따라 몬스터나 무기리스트로 사용

    bool isItemSpawnable = true;
    public void ScrollList(float TriggerValue)
    {
      //ToDo: 가능하면 코드를 깔끔하게 수정
        switch (modeSelect)// 모드에따라 타깃리스트를 설정
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
            isScroll = false;
            switch (modeSelect)// 모드에따라 지정된 함수를 실행
            {
                //0번 인덱스의 무기,팔 소환
                case ModeSelect.Weapon:
                    player.ChooseWeapon(isScrollRight);

                    break;
                case ModeSelect.Pal:
                    player.ChoosePal(isScrollRight);

                    break;

                case ModeSelect.Inven:
                    if(isItemSpawnable)
                    {
                        player.ChooseItem(isScrollRight);
                        isItemSpawnable = false;
                    }
                
                    break;
            }
            //UI에 표시
            playerUI.ModeNextText.gameObject.SetActive(true);
            playerUI.ModePreviousText.gameObject.SetActive(true);
            playerUI.ModeText.color = Color.green;

            // 리스트 항목 우로 넘기기
            if (rightHandRotationZ > 10 && rightHandRotationZ < 160)// 오른손 콘솔이 오른쪽으로 일정이상 기울어지면
            {
                isScroll = true;
                isScrollRight = true;
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// 리스트가 넘어가는 속도조절
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// 진동주기

                    switch(modeSelect)
                    {
                        case ModeSelect.Weapon:
                            player.ChooseWeapon(isScrollRight);
                           
                            break;
                        case ModeSelect.Pal:
                            player.ChoosePal(isScrollRight);
                            break;

                        case ModeSelect.Inven:
                            player.ChooseItem(isScrollRight);
                            break;
                    }
                    ScrollSpeed = 0;
                }
            }
            // 리스트 항목 좌로 넘기기
            if (rightHandRotationZ <350 && rightHandRotationZ >160)// 오른손 콘솔이 왼쪽으로 일정이상 기울어지면
            {
                isScroll = true;
                isScrollRight = false;
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// 리스트가 넘어가는 속도조절
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// 진동주기

                    switch (modeSelect)
                    {
                        case ModeSelect.Weapon:
                            player.ChooseWeapon(isScrollRight);
                            break;
                        case ModeSelect.Pal:
                            player.ChoosePal(isScrollRight);
                            break;
                        case ModeSelect.Inven:
                            player.ChooseItem(isScrollRight);
                            break;
                    }
                    ScrollSpeed = 0;
                }
            }
        }
        else// 트리거가 놓아졌을때
        {
            playerUI.ModeNextText.gameObject.SetActive(false);
            playerUI.ModePreviousText.gameObject.SetActive(false);
            playerUI.ModeText.color = Color.white;
            isItemSpawnable = true;

        }
    }// 리스트에서 선택후 소환하는 함수

    int prevMode;
    int nextMode;

    public bool isStartScene = true;



    private void Update()
    {
        rightThumbValue =inputActionAsset.actionMaps[5].actions[10].ReadValue<Vector2>();// 오른손 조이스틱 값
        TriggerValue = inputActionAsset.actionMaps[5].actions[11].ReadValue<float>();//오른손 트리거 값
        rightHandRotationZ = inputActionAsset.actionMaps[4].actions[1].ReadValue<Quaternion>().eulerAngles.z;// 오른손 z축 회전값

        if (!isStartScene)
        {
            //무기및 팰 선택후 소환
            ScrollList(TriggerValue);

            #region SelectMode // 모드 선택

            if (rightThumbValue.y > 0.5) // 오른손 조이스틱이 위쪽으로 밀려있는 상태에서
            {
                isRightThumbUp = true;
                // 이전 이후 모드 표시
                playerUI.ModeUpText.gameObject.SetActive(true);// 이후모드 텍스트 보여주기
                playerUI.ModeDownText.gameObject.SetActive(true);// 이전모드 텍스트 보여주기
                playerUI.ModeText.color = Color.green;// 현재 사용중인 모드색 초록색으로 변경
                nextMode = (int)modeSelect + 1;
                prevMode = (int)modeSelect - 1;

                //리스트밖으로 벗어나는 인덱스 처리
                if ((int)nextMode > 3)
                {
                    nextMode = 0;
                }
                if ((int)prevMode < 0)
                {
                    prevMode = 3;
                }
                playerUI.ModeUpText.text = ((ModeSelect)nextMode).ToString();
                playerUI.ModeDownText.text = ((ModeSelect)prevMode).ToString();
                //
            }
            else if (rightThumbValue.y <= 0.5 && isRightThumbUp == true)// 다시 제자리로 돌아오면
            {
                playerUI.ModeUpText.gameObject.SetActive(false);// 이전모드 텍스트 비활성화
                playerUI.ModeDownText.gameObject.SetActive(false); //이후모드 텍스트 비활성화
                modeSelect++;// 모드 하나 넘기기
                if ((int)modeSelect > 3)
                {
                    modeSelect = 0;
                }


                player.Index = 0;
                isRightThumbUp = false;

                if (modeSelect != ModeSelect.Weapon)// 모드가 weapon이 아니면 착용중인 무기 비활성화
                {
                    player.equipedWeapon.SetActive(false);
                    rayInteractorR.enabled = true;
                    playerUI.moveModeText.gameObject.SetActive(true);
                }
                else
                {
                    rayInteractorR.enabled = false;
                    if (player.equipedWeapon.GetComponent<Weapon>().weaponType == Weapon.WeaponType.Bow)
                    {
                        playerUI.moveModeText.gameObject.SetActive(false);
                    }
                    else
                    {
                        playerUI.moveModeText.gameObject.SetActive(true);
                    }

                }


                playerUI.ModeText.text = modeSelect.ToString();
                playerUI.ModeText.color = Color.white;
            }

            if (rightThumbValue.y < -0.5) // 오른손 조이스틱이 아래로으로 밀려있는 상태에서
            {
                isRightThumbDown = true;
                playerUI.ModeUpText.gameObject.SetActive(true);
                playerUI.ModeDownText.gameObject.SetActive(true);
                nextMode = (int)modeSelect + 1;
                prevMode = (int)modeSelect - 1;
                if ((int)nextMode > 3)
                {
                    nextMode = 0;
                }
                if ((int)prevMode < 0)
                {
                    prevMode = 3;
                }
                playerUI.ModeUpText.text = ((ModeSelect)nextMode).ToString();
                playerUI.ModeDownText.text = ((ModeSelect)prevMode).ToString();
                playerUI.ModeText.color = Color.green;
            }
            else if (rightThumbValue.y >= -0.5 && isRightThumbDown == true)// 다시 제자리로 돌아오면
            {
                playerUI.ModeUpText.gameObject.SetActive(false);
                playerUI.ModeDownText.gameObject.SetActive(false);
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
                    rayInteractorR.enabled = true;
                    playerUI.moveModeText.gameObject.SetActive(true);

                }
                else
                {
                    rayInteractorR.enabled = false;
                    if (player.equipedWeapon.GetComponent<Weapon>().weaponType == Weapon.WeaponType.Bow)
                    {
                        playerUI.moveModeText.gameObject.SetActive(false);
                    }
                    else
                    {
                        playerUI.moveModeText.gameObject.SetActive(true);
                    }
                }
                playerUI.ModeText.text = modeSelect.ToString();
                playerUI.ModeText.color = Color.white;
            }


            #endregion

            #region SelectPalBallMode  //모드에 따라 팔볼의 포획,소환 여부 선택

            if (modeSelect == ModeSelect.Catch)
            {
                palBall = palBallGo.GetComponent<Pal_Ball>();
                palBall.ballState = Pal_Ball.BallState.Catch;

            }

            if (modeSelect == ModeSelect.Pal)
            {
                palBall = palBallGo.GetComponent<Pal_Ball>();
                palBall.ballState = Pal_Ball.BallState.Summon;
            }

            #endregion
        }

            //ToDo: 따로 키를 눌러서 소환하기보다는 모드가 선택됐을때 자동으로 손에 소환되고 던진후에도 자동으로 손에 스폰되게하는게 더 자연스러울거 같다.

        #region Spawing&Releasing PalBall // 팔볼을 손에 소환
            rightHandPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// 오른손 컨트롤러 값 받아오기
            if (modeSelect == ModeSelect.Catch || modeSelect == ModeSelect.Pal)
            {


                if (isHoldingBall)
                {

                    palBallGo.transform.position = rightHandPos;
                    palBallGo.GetComponent<Rigidbody>().isKinematic = true;





                    if (isPalBallSpawnActive&& !isStartScene)
                    {
                        player.UnsummonPal();// 공을 손으로 가져올때는 몬스터 역소환 ToDo: 내 몬스터는 소환한 상태로 포획용 볼을 꺼낼수도 있으니까 수정해야할수도
                        palBallGo.SetActive(true);
                        isPalBallSpawnActive = false;
                    }


                }
                else
                {
                    palBallGo.GetComponent<Rigidbody>().isKinematic = false;
                }
            }


            #endregion
      





        #region TeleportModeChange // 이동모드 선택

        if (isTeleportModeSelectActive == true)// 한번만 실행되게 
        {
            if (isTeleportMode == true)
            {
                moveMode = MoveMode.Teleport;
                playerUI.moveModeText.text = "Teleport";
                moveProvider.enabled = false;// 텔레포트모드일때는 Walk못하게
                lineVisual.enabled = true;
                lineVisual.reticle.SetActive(true);
                Debug.Log("TeleortMode");
            }
            else
            {
                moveMode = MoveMode.Walk;
                playerUI.moveModeText.text = "Walk";
                rayInteractorL.enabled = false;// Walk모드일때는 ray가 안보이게
                moveProvider.enabled = true;
                lineVisual.reticle.SetActive(false);// telport모드에서 walk모드로 번환시 raticle이 화면에 남아서 비활성화
                lineVisual.enabled = false;
            }
        }

        isTeleportModeSelectActive = false;
        #endregion


        #region 메뉴창 열기
        if (isShowMenuActive == true)
        {
            if(isShowingMenu)
            {
                MenuCanvas.transform.position = MenuPos;
                MenuCanvas.SetActive(true);
                ModeMenuCanvas.SetActive(false);
                
            }
            else
            {
                MenuCanvas.SetActive(false);
                ModeMenuCanvas.SetActive(true);
            }
        }
        #endregion




       

 
    }




    private void LateUpdate()
    {
      
    }
}
