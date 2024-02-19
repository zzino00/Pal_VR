using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;

    private Rigidbody rigidBody;
    private bool isInAir = false;
    Vector3 lastPosition = Vector3.zero;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        BowInteraction.PullActionReleased += Release;

        Stop();
    }
    private void Stop()
    {
        isInAir = false;
        SetPhysics(false);
    }
    private void SetPhysics(bool usePhyics)// true�̸� �߷��� �����ϰ� isKinematic�� ����
    {
        rigidBody.useGravity = usePhyics;
        rigidBody.isKinematic = !usePhyics;
    }

    private void OnDestroy()
    {
        BowInteraction.PullActionReleased -= Release;// �ı��ɶ� �׼� �������
    }
    private void Release(float value)
    {
        BowInteraction.PullActionReleased -= Release;// ȭ���� �ִµ��� ��� ����Ǵ°��� �������ؼ� �������
        gameObject.transform.parent = null;
        isInAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward* value*speed; // ȭ���� ���󰥶� PullActionRelease �׼ǿ��� ���� PullAmount�� ��ŭ ����
        rigidBody.AddForce(force,ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
    }

    private IEnumerator RotateWithVelocity()// �׻� ���󰡴� �������� ȭ������ ���ϰ� ���ִ� �Լ�
    {
        yield return new WaitForFixedUpdate();// FixedUpdate�� ������ �Ʒ����� ����
        while(isInAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }

    }
    private void OnCollisionEnter(Collision collision)// ȭ���� ��ü�� ������ ���ִ� �Լ�
    {
        if(collision.gameObject.tag != "Weapon" &&  collision.gameObject.tag != "Player" && collision.gameObject.tag != "RightHand")
        {
           transform.parent = collision.transform;
            Debug.Log(collision.gameObject);
            Stop();
            Destroy(this.gameObject, 2.0f);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
