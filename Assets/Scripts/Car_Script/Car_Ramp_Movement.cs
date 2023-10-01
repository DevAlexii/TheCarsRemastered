using UnityEngine;

public class Car_Ramp_Movement : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.AddForce((transform.GetChild(0).forward + Vector3.up) * 20000 * Time.deltaTime, ForceMode.Force);
        rb.excludeLayers = GameManager.self.layer_to_exclude;
        transform.gameObject.layer = 0;
        Destroy(gameObject, 7);
        GameManager.self.UpdateScore(1);
    }
    void Update()
    {
        transform.Translate(transform.GetChild(0).forward * 10 * Time.deltaTime);
    }
}
