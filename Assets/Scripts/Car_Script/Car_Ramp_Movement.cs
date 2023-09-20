using UnityEngine;

public class Car_Ramp_Movement : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = GetComponent<Collider>().isTrigger = false;
        rb.AddForce((transform.forward + Vector3.up) * 11000 * Time.deltaTime, ForceMode.Force);
        transform.gameObject.layer = 0;
        Destroy(transform.parent.gameObject,5);
        GameManager.self.UpdateScore(1);
    }
    void Update()
    {
        transform.parent.Translate(transform.forward * 3 * Time.deltaTime);
    }
}
