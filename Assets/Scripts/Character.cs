using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Transform modelTransform;

    [SerializeField]
    private Transform faceTransform;
    [SerializeField]
    private Vector3 faceLocalPosition;
    [SerializeField]
    private bool start = false;
    [SerializeField]
    private Vector3 startLocalPosition;
    [SerializeField]
    private Vector3 endLocalPosition;

    [SerializeField]
    private Transform bodyTransform;
    [SerializeField]
    private Vector3 startScale = Vector3.one;
    [SerializeField]
    private Vector3 endScale;

    [Header("Properties")]
    [SerializeField]
    private float jumpPower = 4;
    [SerializeField]
    private float lookSpeed = 2;
    [SerializeField]
    private float rotateSpeed = 2;

    private Vector3 dir;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody.AddTorque(Random.insideUnitSphere * 3, ForceMode.Impulse);
    }

    private Quaternion rotateDirection;
    private Quaternion prevQuaternion;
    private bool look = false;
    void Update()
    {
        faceLocalPosition = faceTransform.localPosition;

        if (start)
            faceTransform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (bodyTransform.localScale.x - startScale.x) / (endScale.x - startScale.x));

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftCommand))
        {
            rigidbody.AddForce(transform.forward * jumpPower, ForceMode.Impulse);
        }

        Vector3 f = Vector3.zero;

        float hor = Input.GetAxis("Horizontal");

        float ver = Input.GetAxis("Vertical");

        f += Vector3.up * ver;
        f += Vector3.right * hor;

        f.Normalize();

        if (f.sqrMagnitude > 0)
        {
            dir = f;
            // transform.LookAt(transform.position + dir);
            if (!look)
            {
                rotateDirection = Quaternion.FromToRotation(prevQuaternion.eulerAngles, Quaternion.LookRotation(dir).eulerAngles);
                print(rotateDirection);
                prevQuaternion = transform.rotation;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * lookSpeed);
            rigidbody.angularVelocity = Vector3.zero;
            look = true;
        }
        else
        {
            // if (look)
            // {
            //     // Vector3 t = new Vector3(-dir.y, dir.x);
            //     Vector3 t = rotateDirection.eulerAngles; //new Vector3(rotateDirection.x, rotateDirection.y, rotateDirection.z);
            //     t.Normalize();
            //     rigidbody.AddTorque(t * rotateSpeed);
            //     look = false;
            // }
        }

    }
}
