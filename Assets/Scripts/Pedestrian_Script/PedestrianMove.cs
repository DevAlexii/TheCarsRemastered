using System.Collections.Generic;
using UnityEngine;

public class PedestrianMove : MonoBehaviour
{
    Rigidbody rb;

    [Header("Explosion Variables")]
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float afterCollisionDeadTimer;

    [Header("Path")]
    [SerializeField] private Transform arrow;
    private Path path;
    private List<Path> possible_cross_path;
    private int path_index = 0;
    [SerializeField] private float walk_speed;
    private float reachable_point_distance;
    private float rotation_speed;
    private bool has_crossed;
    private int cross_index;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rotation_speed = Random.Range(6, 11);
        reachable_point_distance = Random.Range(0.5f, 1.1f);
    }
    public void Initilized(PathInfo pathInfo)
    {
        this.path = pathInfo.path;
        possible_cross_path = pathInfo.cross_paths;
    }

    void Update()
    {
        PedestriansMoves();
    }
    void PedestriansMoves()
    {
        if (path == null) return;
        Vector3 relativeVector = transform.InverseTransformPoint(path.Nodes[path_index].position);
        Quaternion targetRotation = Quaternion.LookRotation(relativeVector, Vector3.up);
        arrow.rotation = Quaternion.Slerp(arrow.rotation, targetRotation, Time.deltaTime * rotation_speed);
        CheckNode();
        transform.Translate(arrow.forward * walk_speed * Time.deltaTime);
    }
    void CheckNode()
    {
        Vector3 distance = path.Nodes[path_index].position - transform.position;

        if (distance.magnitude <= reachable_point_distance)
        {
            if (path_index + 1 == path.Nodes.Count)
            {
                PedestrianSpawn.self.pedestrians_spawned.Remove(gameObject);
                PedestrianSpawn.self.currentPedestrians--;
                Destroy(gameObject);
                return;
            }
            else if (path.Nodes[path_index].transform.name.StartsWith("Cross"))
            {
                if (!has_crossed)
                {
                    if (CustomLibrary.RandomBoolInPercentage(GameManager.self.difficulty))
                    {
                        path = possible_cross_path[cross_index];
                        has_crossed = true;
                    }
                }
                cross_index++;
            }
            path_index++;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            PedestrianSpawn.self.currentPedestrians--;
            rb.isKinematic = false;
            rb.AddForce(collider.gameObject.transform.forward * 300f + Vector3.up * 4);
            GetComponent<Collider>().isTrigger = false;
            Destroy(this);
            Destroy(gameObject, afterCollisionDeadTimer);
        }
    }
}
