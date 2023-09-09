using System;
using System.Collections.Generic;
using UnityEngine;

public class TestCarFollowPath : MonoBehaviour
{
    [SerializeField] private Path path;
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
    [SerializeField]  LayerInfoNodes layerInfo;

    private void Start()
    {
        moveCar = MoveCarToNode;
        max_move_speed = move_speed;
        target_max_move_speed = max_move_speed;
        move_speed = 0;
    }
    void Update()
    {
        RotateWheelsAndCar();
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
            if (node_index == 1)
            {
                target_max_move_speed = 4;
            }
            else if (node_index == 5)
            {
                target_max_move_speed = max_move_speed;
            }
            node_index++;
        }
    }

    void MoveCarToNode()
    {
        if (path != null)
        {
            FinalNodeCheck();
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
            LayerMask.NameToLayer("ciao");
        }
    }
}

[Serializable]
struct LayerInfoNodes
{
    [SerializeField] LayerMask startPoint;
    [SerializeField] LayerMask startBreak;
    [SerializeField] LayerMask endBreak;

    public int StartPoint => startPoint;
    public int StartBreak => startBreak;
    public int EndBreak => endBreak;
}