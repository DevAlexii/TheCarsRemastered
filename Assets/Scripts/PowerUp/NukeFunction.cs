using UnityEngine;

public class NukeFunction : MonoBehaviour
{
    [SerializeField] private float force_explosion;
    [SerializeField] private float radius_explosion;
    private void OnCollisionEnter(Collision collision)
    {
        Car_Manager.self.toggleNuke(force_explosion, radius_explosion);
        Destroy(gameObject);
    }
}
