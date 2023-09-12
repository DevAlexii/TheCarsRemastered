using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarFollowPath : MonoBehaviour
{
    [Header("CarComponent")]
    [SerializeField] private List<Transform> front_wheels;
    [SerializeField] private Transform shell;

    [Header("Movement")]
    [SerializeField] private float rotation_speed;
    [SerializeField] private float move_speed;
    private float max_move_speed;
    private float target_max_move_speed;

    [Header("Wait Time")]
    [SerializeField] private float wait_time;
    private float wait_timer;

    [Header("PathInfo")]
    [SerializeField] private float node_reachable_distance;
    private Path path;
    private int node_index;

    [Header("Internal Var")]
    private Car_Core owner;
    private Action On_CarMove;
    private Action On_Waiting;
    private bool stop_car;
    private bool can_be_touched;
    private bool is_kamikaze;

    [Header("Queque")]
    [SerializeField][Range(1.5f,2.3f)] float max_distance;
    [SerializeField][Range(1f,1.5f)] float min_distance;
    private float distance;
    private Ray queque_ray;
    private RaycastHit queque_hit;

    private void Start()
    {
        queque_ray = new Ray();
        distance = Random.Range(min_distance, max_distance);
        max_move_speed = move_speed;
        target_max_move_speed = max_move_speed;
        move_speed = 0;
    }
    public void InitilizedPath(Path newPath, Car_Core Owner,bool Kamikaze)
    {
        path = newPath;
        On_CarMove = MoveCarToNode;
        owner = Owner;
        is_kamikaze = Kamikaze;
    }
    void Update()
    {
        On_Waiting?.Invoke();
        On_CarMove?.Invoke();
    }
    private Vector3 GetNodePosition()
    {
        if (path != null)
        {
            if (node_index <= path.Nodes.Count - 1)
            {
                return path.Nodes[node_index].position;
            }
        }
        return Vector3.zero;
    }
    private void RotateWheelsAndCar()
    {
        if (node_index == path.Nodes.Count) return;
        Vector3 relativeVector = transform.InverseTransformPoint(GetNodePosition());
        Quaternion targetRotation = Quaternion.LookRotation(relativeVector, Vector3.up);
        foreach (Transform wheel in front_wheels)
        {
            wheel.rotation = Quaternion.Slerp(wheel.rotation, targetRotation, Time.deltaTime * rotation_speed * 2);
        }
        shell.rotation = Quaternion.Slerp(shell.rotation, targetRotation, Time.deltaTime * rotation_speed);
    }
    private void CheckNodeDistance()
    {
        if (node_index <= 1) CheckQueque();

        float distance = Vector3.Distance(GetNodePosition(), transform.position);
        if (distance <= node_reachable_distance)
        {
            if (path.Nodes[node_index].name.StartsWith("Stop") && !is_kamikaze)
            {
                stop_car = true;
                owner.ShowDirectionalArrow();
                On_Waiting = Waiting;
                can_be_touched = true;
            }
            else if (path.Nodes[node_index].name.StartsWith("StartBrake"))
            {
                target_max_move_speed = 4;
            }
            else if (path.Nodes[node_index].name.StartsWith("EndBrake"))
            {
                target_max_move_speed = max_move_speed;
                owner.HideDirectionalArrow();
            }
            else if (path.Nodes[node_index].name.StartsWith("End"))
            {
                Car_Manager.self.RemoveCar(this.gameObject);
                Destroy(this.gameObject);
            }
            node_index++;
        }
    }
    void MoveCarToNode()
    {
        if (path != null)
        {
            RotateWheelsAndCar();
            if (stop_car)
            {
                if (move_speed > 0.05f)
                {
                    move_speed = Mathf.Lerp(move_speed, 0, Time.deltaTime * 5);
                }
                else
                {
                    move_speed = 0;
                }
            }
            else
            {
                if (Mathf.Abs(move_speed - target_max_move_speed) > 0.05f)
                {
                    move_speed = Mathf.Lerp(move_speed, target_max_move_speed, Time.deltaTime * 2);
                }
                else
                {
                    move_speed = target_max_move_speed;
                }
            }
            CheckNodeDistance();
            transform.Translate(front_wheels[0].forward * move_speed * Time.deltaTime);
        }
    }
    public bool ToogleShouldMove()
    {
        if (!can_be_touched) return false;

        if (On_CarMove != null)
        {
            stop_car = stop_car ? false : true;
            if (On_Waiting != null)
            {
                On_Waiting = null;
            }
        }
        return true;
    }
    public void OnCrash()
    {
        Destroy(this);
    }
    private void Waiting()
    {
        wait_timer += Time.deltaTime;
        if (wait_timer >= wait_time)
        {
            wait_timer = 0;
            ToogleShouldMove();
            On_Waiting = null;
        }
    }
    private void CheckQueque()
    {
        queque_ray.origin = transform.position + shell.forward + Vector3.up * 0.5f;
        queque_ray.direction = shell.forward;

        if (Physics.Raycast(queque_ray.origin, queque_ray.direction, out queque_hit, distance))
        {
            if (queque_hit.transform.gameObject.layer == 3 || queque_hit.transform.gameObject.layer == 6)
            {
                stop_car = true;
            }
        }
        else
        {
            stop_car = false;
        }
    }
}
