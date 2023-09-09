using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarFollowPath : MonoBehaviour
{
    private Path path;
    [SerializeField] private List<Transform> front_wheels;
    [SerializeField] private Transform shell;
    [SerializeField] private float node_reachable_distance;
    [SerializeField] private float move_speed;
    private float max_move_speed;
    private float target_max_move_speed;
    [SerializeField] private float rotation_speed;
    private int node_index;
    public bool stopCar;
    Action moveCar;

    private void Start()
    {
        max_move_speed = move_speed;
        target_max_move_speed = max_move_speed;
        move_speed = 0;
    }
    public void InitilizedPath(Path newPath)
    {
        path = newPath;
        moveCar = MoveCarToNode;
    }
    void Update()
    {
        moveCar?.Invoke();
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
    private void FinalNodeCheck()
    {
        if (node_index >= path.Nodes.Count)
        {
            if (path.repeat)
            {
                node_index = 0;
                stopCar = false;
            }
            else stopCar = true;
        }
    }
    private void CheckNodeDistance()
    {
        float distance = Vector3.Distance(GetNodePosition(), transform.position);
        if (distance <= node_reachable_distance)
        {
            if ( path.Nodes[node_index].name.StartsWith("Stop"))
            {
                stopCar = true;
            }
            else if (path.Nodes[node_index].name.StartsWith("StartBrake"))
            {
                target_max_move_speed = 4;
            }
            else if (path.Nodes[node_index].name.StartsWith("EndBrake"))
            {
                target_max_move_speed = max_move_speed;
            }
            else if (path.Nodes[node_index].name.StartsWith("End"))
            {
                Car_Spawn_Manager.self.RemoveCar(this.gameObject);
                Destroy(this.gameObject);
            }
            node_index++;
        }
    }
    void MoveCarToNode()
    {
        if (path != null)
        {
            FinalNodeCheck();
            RotateWheelsAndCar();
            if (stopCar)
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
    public void ToogleShouldMove()
    {
        if (moveCar != null)
        {
            stopCar = stopCar? false : true;
        }
    }
}
