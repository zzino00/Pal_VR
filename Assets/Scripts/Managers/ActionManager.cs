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
    [SerializeField] InputActionAsset inputActionAsset;// InputAction�� �޾ƿ������� Ŭ����
    [SerializeField] ActionBasedContinuousMoveProvider moveProvider;// �ȴ� ��� ���� Ŭ����
    [SerializeField] XRRayInteractor rayInteractorL;// ���� �ε������ ���� Ŭ����
    [SerializeField] XRRayInteractor rayInteractorR;// ���� �ε������ ���� Ŭ����
    [SerializeField] XRInteractorLineVisual lineVisual;// ���̸� ��� ȭ�鿡 ǥ������ ���� Ŭ����
    [SerializeField] XRBaseController rBaseController;// ������ �ܼ��� ȸ������ �޾ƿ������� Ŭ����
    bool isTeleportMode; // �ڷ���Ʈ ���
    bool isHoldingBall; // �Ӻ��� ��ȯ�� ����ó���� ���� ����
    bool isShowingMenu;
    public Player player;
    public GameObject palBallGo;
    public PlayerUI playerUI;
    public GameObject MenuCanvas;
    public GameObject ModeMenuCanvas;
    Pal_Ball palBall;
    public ModeSelect modeSelect = 0;
    public enum MoveMode// �̵����
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

    InputAction teleportModeChange;// teleport����ư �Է°��޴� ����
    InputAction palBallSpawn;//�Ⱥ���ȯ�Է°� �޴� ����
    InputAction showMenu;// �޴�â ����
    bool isTeleportModeSelectActive; // �ѹ��� �Լ��� ����ǰ� �ϴ� ����
    bool isPalBallSpawnActive; // �ѹ��� �Լ��� ����ǰ� �ϴ� ����
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
      

        moveMode = MoveMode.Teleport;//�̵���尡 �⺻��
        playerUI.moveModeText.text = "Teleport";
        playerUI.ModeText.text = "Start";
        playerUI.ModeNextText.text = "";
        playerUI.ModePreviousText.text = "";

        moveProvider.enabled = false;// �ڷ���Ʈ����϶��� Walk���ϰ�
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
        MenuPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// ������ ��Ʈ�ѷ� �� �޾ƿ���
        MenuCanvas.transform.rotation = inputActionAsset.actionMaps[4].actions[1].ReadValue<Quaternion>();
        isShowMenuActive = true;
        isShowingMenu = !isShowingMenu;
    }

   
    Vector2  rightThumbValue ;
    Vector3 rightHandPos;
  
    float TriggerValue;// Ʈ���Ű� ������� �ִ���
    float rightHandRotationZ;// ����Ʈ �ѱ涧 ���̴� ��������Ʈ�ѷ�z�� ȸ����

    int palListScrollSpeedCount = 100;// ����Ʈ �Ѿ�� �ӵ� 
    int ScrollSpeed;// ����Ʈ �Ѿ�� �ӵ� ������ ���� ī��Ʈ
  
    bool isRightThumbUp;// ������ ���̽�ƽ ������ �Է°�
    bool isRightThumbDown;// ������ ���̽�ƽ �Ʒ����� �Է°�
    bool isScrollRight;// ����Ʈ�� ���������� �ѱ��� �������� �ѱ��� ���Ѵ� ����
    public bool isScroll;// ����Ʈ���� ���� �׸����� �ѱ��� ������ ���ϴ� ����

    List<GameObject> TargetList = new List<GameObject>();// ��Ȳ������ ���ͳ� ���⸮��Ʈ�� ���

    bool isItemSpawnable = true;
    public void ScrollList(float TriggerValue)
    {
      //ToDo: �����ϸ� �ڵ带 ����ϰ� ����
        switch (modeSelect)// ��忡���� Ÿ�긮��Ʈ�� ����
        {
            case ModeSelect.Weapon:
                TargetList = player.myWeaponList;
              
                break;
            case ModeSelect.Pal:
                TargetList = player.monsterGoList;
              
                break;
        }

      
        if (TriggerValue > 0.5 && TargetList.Count != 0)// Ʈ���Ű� ������ִ� ���¿���
        {
            isScroll = false;
            switch (modeSelect)// ��忡���� ������ �Լ��� ����
            {
                //0�� �ε����� ����,�� ��ȯ
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
            //UI�� ǥ��
            playerUI.ModeNextText.gameObject.SetActive(true);
            playerUI.ModePreviousText.gameObject.SetActive(true);
            playerUI.ModeText.color = Color.green;

            // ����Ʈ �׸� ��� �ѱ��
            if (rightHandRotationZ > 10 && rightHandRotationZ < 160)// ������ �ܼ��� ���������� �����̻� ��������
            {
                isScroll = true;
                isScrollRight = true;
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// ����Ʈ�� �Ѿ�� �ӵ�����
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// �����ֱ�

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
            // ����Ʈ �׸� �·� �ѱ��
            if (rightHandRotationZ <350 && rightHandRotationZ >160)// ������ �ܼ��� �������� �����̻� ��������
            {
                isScroll = true;
                isScrollRight = false;
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// ����Ʈ�� �Ѿ�� �ӵ�����
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// �����ֱ�

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
        else// Ʈ���Ű� ����������
        {
            playerUI.ModeNextText.gameObject.SetActive(false);
            playerUI.ModePreviousText.gameObject.SetActive(false);
            playerUI.ModeText.color = Color.white;
            isItemSpawnable = true;

        }
    }// ����Ʈ���� ������ ��ȯ�ϴ� �Լ�

    int prevMode;
    int nextMode;

    public bool isStartScene = true;



    private void Update()
    {
        rightThumbValue =inputActionAsset.actionMaps[5].actions[10].ReadValue<Vector2>();// ������ ���̽�ƽ ��
        TriggerValue = inputActionAsset.actionMaps[5].actions[11].ReadValue<float>();//������ Ʈ���� ��
        rightHandRotationZ = inputActionAsset.actionMaps[4].actions[1].ReadValue<Quaternion>().eulerAngles.z;// ������ z�� ȸ����

        if (!isStartScene)
        {
            //����� �� ������ ��ȯ
            ScrollList(TriggerValue);

            #region SelectMode // ��� ����

            if (rightThumbValue.y > 0.5) // ������ ���̽�ƽ�� �������� �з��ִ� ���¿���
            {
                isRightThumbUp = true;
                // ���� ���� ��� ǥ��
                playerUI.ModeUpText.gameObject.SetActive(true);// ���ĸ�� �ؽ�Ʈ �����ֱ�
                playerUI.ModeDownText.gameObject.SetActive(true);// ������� �ؽ�Ʈ �����ֱ�
                playerUI.ModeText.color = Color.green;// ���� ������� ���� �ʷϻ����� ����
                nextMode = (int)modeSelect + 1;
                prevMode = (int)modeSelect - 1;

                //����Ʈ������ ����� �ε��� ó��
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
            else if (rightThumbValue.y <= 0.5 && isRightThumbUp == true)// �ٽ� ���ڸ��� ���ƿ���
            {
                playerUI.ModeUpText.gameObject.SetActive(false);// ������� �ؽ�Ʈ ��Ȱ��ȭ
                playerUI.ModeDownText.gameObject.SetActive(false); //���ĸ�� �ؽ�Ʈ ��Ȱ��ȭ
                modeSelect++;// ��� �ϳ� �ѱ��
                if ((int)modeSelect > 3)
                {
                    modeSelect = 0;
                }


                player.Index = 0;
                isRightThumbUp = false;

                if (modeSelect != ModeSelect.Weapon)// ��尡 weapon�� �ƴϸ� �������� ���� ��Ȱ��ȭ
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

            if (rightThumbValue.y < -0.5) // ������ ���̽�ƽ�� �Ʒ������� �з��ִ� ���¿���
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
            else if (rightThumbValue.y >= -0.5 && isRightThumbDown == true)// �ٽ� ���ڸ��� ���ƿ���
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

            #region SelectPalBallMode  //��忡 ���� �Ⱥ��� ��ȹ,��ȯ ���� ����

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

            //ToDo: ���� Ű�� ������ ��ȯ�ϱ⺸�ٴ� ��尡 ���õ����� �ڵ����� �տ� ��ȯ�ǰ� �����Ŀ��� �ڵ����� �տ� �����ǰ��ϴ°� �� �ڿ�������� ����.

        #region Spawing&Releasing PalBall // �Ⱥ��� �տ� ��ȯ
            rightHandPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// ������ ��Ʈ�ѷ� �� �޾ƿ���
            if (modeSelect == ModeSelect.Catch || modeSelect == ModeSelect.Pal)
            {


                if (isHoldingBall)
                {

                    palBallGo.transform.position = rightHandPos;
                    palBallGo.GetComponent<Rigidbody>().isKinematic = true;





                    if (isPalBallSpawnActive&& !isStartScene)
                    {
                        player.UnsummonPal();// ���� ������ �����ö��� ���� ����ȯ ToDo: �� ���ʹ� ��ȯ�� ���·� ��ȹ�� ���� �������� �����ϱ� �����ؾ��Ҽ���
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
      





        #region TeleportModeChange // �̵���� ����

        if (isTeleportModeSelectActive == true)// �ѹ��� ����ǰ� 
        {
            if (isTeleportMode == true)
            {
                moveMode = MoveMode.Teleport;
                playerUI.moveModeText.text = "Teleport";
                moveProvider.enabled = false;// �ڷ���Ʈ����϶��� Walk���ϰ�
                lineVisual.enabled = true;
                lineVisual.reticle.SetActive(true);
                Debug.Log("TeleortMode");
            }
            else
            {
                moveMode = MoveMode.Walk;
                playerUI.moveModeText.text = "Walk";
                rayInteractorL.enabled = false;// Walk����϶��� ray�� �Ⱥ��̰�
                moveProvider.enabled = true;
                lineVisual.reticle.SetActive(false);// telport��忡�� walk���� ��ȯ�� raticle�� ȭ�鿡 ���Ƽ� ��Ȱ��ȭ
                lineVisual.enabled = false;
            }
        }

        isTeleportModeSelectActive = false;
        #endregion


        #region �޴�â ����
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
