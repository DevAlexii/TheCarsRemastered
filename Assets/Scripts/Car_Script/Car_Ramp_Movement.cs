using UnityEngine;

public class Car_Ramp_Movement : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce((transform.GetChild(0).forward + Vector3.up) * 24500 * Time.deltaTime, ForceMode.Force);
        transform.gameObject.layer = 0;
        Destroy(transform.gameObject, 7);
        GameManager.self.UpdateScore(1);
    }
    void Update()
    {
        transform.Translate(transform.GetChild(0).forward * 3 * Time.deltaTime);
    }
}
