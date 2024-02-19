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
    private void SetPhysics(bool usePhyics)// true이면 중력을 적용하고 isKinematic을 꺼줌
    {
        rigidBody.useGravity = usePhyics;
        rigidBody.isKinematic = !usePhyics;
    }

    private void OnDestroy()
    {
        BowInteraction.PullActionReleased -= Release;// 파괴될때 액션 구독취소
    }
    private void Release(float value)
    {
        BowInteraction.PullActionReleased -= Release;// 화살이 있는데도 계속 실행되는것을 막기위해서 구독취소
        gameObject.transform.parent = null;
        isInAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward* value*speed; // 화살이 날라갈때 PullActionRelease 액션에서 받은 PullAmount값 만큼 날라감
        rigidBody.AddForce(force,ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
    }

    private IEnumerator RotateWithVelocity()// 항상 날라가는 방향으로 화살촉이 향하게 해주는 함수
    {
        yield return new WaitForFixedUpdate();// FixedUpdate가 끝나면 아랫구문 시작
        while(isInAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }

    }
    private void OnCollisionEnter(Collision collision)// 화살이 물체에 꽂히게 해주는 함수
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
