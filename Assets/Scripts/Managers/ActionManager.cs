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
        if (TriggerValue > 0.5 && TargetList.Count != 0)// Ʈ���Ű� ������ִ� ���¿���
        {
            if (rightHandRotationZ > 10 && rightHandRotationZ < 160)// ������ �ܼ��� ���������� �����̻� ��������
            {
                ScrollSpeed++;
                if (ScrollSpeed >= palListScrollSpeedCount)// ����Ʈ�� �Ѿ�� �ӵ�����
                {
                    rBaseController.SendHapticImpulse(0.7f, 0.3f);// �����ֱ�

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
        teleportValue = inputActionAsset.actionMaps[2].actions[9].ReadValue<float>();// �޼��ܼ� X��ư�� �޾ƿ��� ������������ 1, �ƴϸ� 0
        summonButtonValue = inputActionAsset.actionMaps[5].actions[9].ReadValue<float>();// ������ priamrybutton��
        rightThumbValue =inputActionAsset.actionMaps[5].actions[10].ReadValue<Vector2>();// ������ ���̽�ƽ ��
        TriggerValue = inputActionAsset.actionMaps[5].actions[11].ReadValue<float>();//������ Ʈ���� ��
        rightHandRotationZ = inputActionAsset.actionMaps[5].actions[12].ReadValue<Quaternion>().eulerAngles.z;// ������ z�� ȸ����


        #region SelectMonsterTo Spawn
        ScrollList(TriggerValue);

        //if (TriggerValue >0.5&&player.monsterGoList.Count !=0)// Ʈ���Ű� ������ִ� ���¿���
        //{
        //  if( rightHandRotationZ > 10 && rightHandRotationZ <160)// ������ �ܼ��� ���������� �����̻� ��������
        //    {
        //        ScrollSpeed++;
        //        if(ScrollSpeed>=palListScrollSpeedCount)// ����Ʈ�� �Ѿ�� �ӵ�����
        //        {

        //            rBaseController.SendHapticImpulse(0.7f, 0.3f);// �����ֱ�
        //            player.ChoosePal();// ����Ʈ �ѱ�� �Լ�
        //            ScrollSpeed = 0;
        //        }

        //    }


        //}
        #endregion



        #region SelectMode // ��� ����

        if (rightThumbValue.y > 0.5) // ������ ���̽�ƽ�� �������� �з��ִ� ���¿���
        {
            isRightThumbUp = true;
        }
        else if (rightThumbValue.y <= 0.5 && isRightThumbUp == true)// �ٽ� ���ڸ��� ���ƿ���
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

        if (rightThumbValue.y < -0.5) // ������ ���̽�ƽ�� �������� �з��ִ� ���¿���
        {
            isRightThumbDown = true;
        }
        else if (rightThumbValue.y >= -0.5 && isRightThumbDown == true)// �ٽ� ���ڸ��� ���ƿ���
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
        //if (rightThumbValue.y>0.5) // ������ ���̽�ƽ�� �������� �з��ִ� ���¿���
        //{
        //    isRightThumb = true;
        //}
        //else if(rightThumbValue.y<=0.5 && isRightThumb == true)// �ٽ� ���ڸ��� ���ƿ���
        //{
        //    palBall = palBallGo.GetComponent<Pal_Ball>();
        //    if (palBall.ballState == Pal_Ball.BallState.Catch)// �Ⱥ��� ���°� �ٲ�
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
            if (summonButtonValue == 1 && isSummonPressed == false)// ��ư�� ������ �ִ� ���� float������ ������
                                                                   // ������ ������Ʈ���� 1���� ����ǰ� �Ϸ��� �̷��� ����� ����Ѵ�.
            {
                isHoldingBall = !isHoldingBall; // ���� ��ȯ�� ����
                isSummonPressed = true;
            }
            if (summonButtonValue == 0)
            {
                isSummonPressed = false;
            }

            if (isHoldingBall)
            {
                rightHandPos = this.transform.position + inputActionAsset.actionMaps[4].actions[0].ReadValue<Vector3>();// ������ ��Ʈ�ѷ� �� �޾ƿ���
                palBallGo.transform.localPosition = rightHandPos;
                player.UnsummonPal();// ���� ������ �����ö��� ���� ����ȯ ToDo: �� ���ʹ� ��ȯ�� ���·� ��ȹ�� ���� �������� �����ϱ� �����ؾ��Ҽ���
                palBallGo.SetActive(true);
            }
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
