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
    private float rotatePower = 2;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.AddTorque(Random.insideUnitSphere * 3, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        faceLocalPosition = faceTransform.localPosition;

        if (start)
            faceTransform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (bodyTransform.localScale.x - startScale.x) / (endScale.x - startScale.x));

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }


        Vector3 f = Vector3.zero;

        float hor = Input.GetAxis("Horizontal");
        f += Vector3.up * -hor * rotatePower;

        float ver = Input.GetAxis("Vertical");
        f += Vector3.right * ver * rotatePower;

        rigidbody.AddTorque(f);
    }
}
