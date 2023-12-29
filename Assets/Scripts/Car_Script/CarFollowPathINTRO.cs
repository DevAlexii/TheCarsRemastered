using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CarFollowPathINTRO : MonoBehaviour
{
    [Header("CarComponent")]
    [SerializeField] private List<Transform> front_wheels;
    [SerializeField] private Transform shell;

    [Header("Movement")]
    private float rotation_speed;
    private float max_move_speed;
    private float max_brake_speed;
    private float move_speed;
    private float target_max_move_speed;

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

    [Header("ColorOutline")]
    public Color color;
    private Outline outlineScript;

    #region Initialize
    public void InitilizedPath(Path newPath, Car_Core Owner, CarInfo data)
    {
        owner = Owner;//SetReference
       
        MovementVarSetup(data);//SetupMovement
        path = newPath; //SetUpPath
       
        On_CarMove = MoveCarToNode;
       
        outlineScript = GetComponentInChildren<Outline>();
    }

    private void MovementVarSetup(CarInfo data)
    {
        max_move_speed = data.MaxSpeed;
        max_brake_speed = data.MaxBreakForce;
        rotation_speed = data.MaxRotationSpeed;
        target_max_move_speed = max_move_speed;
    }
    #endregion
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
        if (relativeVector == Vector3.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(relativeVector, Vector3.up);
        targetRotation.eulerAngles = targetRotation.eulerAngles.y * Vector3.up;
        foreach (Transform wheel in front_wheels)
        {
            wheel.rotation = Quaternion.Slerp(wheel.rotation, targetRotation, Time.deltaTime * rotation_speed * 2);
        }
        shell.rotation = Quaternion.Slerp(shell.rotation, targetRotation, Time.deltaTime * rotation_speed);
    }
    private void CheckNodeDistance()
    {
      

        float distance = Vector3.Distance(GetNodePosition(), transform.position);
        if (distance <= node_reachable_distance)
        {
            if (path.Nodes[node_index].name.StartsWith("Stop"))
            {
                //ToogleCollision(true);
                stop_car = true;
                owner.ShowDirectionalArrow();
                //can_be_touched = true;
            }
            else if (path.Nodes[node_index].name.StartsWith("StartBrake"))
            {
                target_max_move_speed = max_brake_speed;
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
   
}
