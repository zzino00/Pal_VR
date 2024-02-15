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

    private void SetPhysics(bool usePhyics)
    {
        rigidBody.useGravity = usePhyics;
        rigidBody.isKinematic = !usePhyics;
    }

    private void OnDestroy()
    {
        BowInteraction.PullActionReleased -= Release;
    }
    private void Release(float value)
    {
        BowInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        isInAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward* value*speed;
        rigidBody.AddForce(force,ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
        lastPosition = tip.position;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();// FixedUpdate가 끝나면 아랫구문 시작
        while(isInAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }

    }

    private void FixedUpdate()
    {
        if (isInAir)
        {
            CheckCollision();
            lastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
