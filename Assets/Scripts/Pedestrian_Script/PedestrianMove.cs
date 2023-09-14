using UnityEngine;

public class PedestrianMove : MonoBehaviour
{
    Rigidbody rb;

    [Header("Explosion Variables")]
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float afterCollisionDeadTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        PedestriansMoves();
    }

    void PedestriansMoves()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Destroy(this, afterCollisionDeadTimer);
        }
    }

}
