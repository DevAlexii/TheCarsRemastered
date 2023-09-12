using UnityEngine;

public class Car_Ramp_Movement : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = GetComponent<Collider>().isTrigger = false;
        rb.AddForce((transform.forward + Vector3.up) * 500, ForceMode.Force);
        transform.gameObject.layer = 0;
        Destroy(transform.parent.gameObject,5);
    }
    void Update()
    {
        transform.parent.Translate(transform.forward * 10 * Time.deltaTime);
    }
}
