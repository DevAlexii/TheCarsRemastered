using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CarFollowPath : MonoBehaviour
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

    [Header("Wait Time")]
    [SerializeField] private float wait_time;
    [SerializeField] private AnimationCurve angle_curve;
    private float start_yaw;
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
    private float queque_distance;
    private Ray queque_ray;
    private RaycastHit queque_hit;
    private bool pickCombo = true;

    [Header("VFX")]
    [SerializeField] private GameObject smoke_effect;

    #region Initialize
    public void InitilizedPath(Path newPath, Car_Core Owner, bool Kamikaze, CarInfo data)
    {
        owner = Owner;//SetReference
        queque_ray = new Ray();//GenerateQuequeRay
        queque_distance = data.QuequeRange;//maxDistanceForQuequeRay
        MovementVarSetup(data);//SetupMovement
        path = newPath; //SetUpPath
        is_kamikaze = Kamikaze; //IsKamikaze?
        if (!is_kamikaze) { ToogleCollision(false); }
        On_CarMove = MoveCarToNode;
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
        if (node_index <= 1) CheckQueque();

        float distance = Vector3.Distance(GetNodePosition(), transform.position);
        if (distance <= node_reachable_distance)
        {
            if (path.Nodes[node_index].name.StartsWith("Stop") && !is_kamikaze)
            {
                ToogleCollision(true);
                stop_car = true;
                owner.ShowDirectionalArrow();
                start_yaw = shell.eulerAngles.y;
                ToogleSmokeEffectOnWait(true);
                On_Waiting = Waiting;
                can_be_touched = true;
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
            else if (path.Nodes[node_index].name.StartsWith("GivePoint"))
            {
                ToogleCollision();
                GameManager.self.UpdateScore(1);
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
    private bool ToogleShouldMove()
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
    public bool ClickedCar()
    {
        if (ComboCounter())
        {
            return true;
        }
        return false;
    }
    public void OnCrash()
    {
        ToogleSmokeEffectOnWait();
        Destroy(this);
    }
    private void Waiting()
    {
        wait_timer += Time.deltaTime;
        smoke_effect.GetComponent<VisualEffect>().SetFloat("BlendColor", wait_timer / wait_time);
        shell.rotation = Quaternion.Euler(new Vector3(0, start_yaw + angle_curve.Evaluate(wait_timer), 0));
        if (wait_timer >= wait_time)
        {
            wait_timer = 0;
            ToogleShouldMove();
            ToogleSmokeEffectOnWait();
            pickCombo = false;
            Car_Manager.self.comboCount = 0;
            On_Waiting = null;
        }
    }

    public bool ComboCounter()
    {
        if (ToogleShouldMove())
        {
            ToogleSmokeEffectOnWait();
            if (pickCombo)
            {
                Car_Manager.self.comboCount++;
                pickCombo = false;
            }
            return true;
        }

        return false;
    }

    private void CheckQueque()
    {
        queque_ray.origin = transform.position + shell.forward;
        queque_ray.direction = shell.forward;
        if (Physics.Raycast(queque_ray.origin, queque_ray.direction, out queque_hit, queque_distance))
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
    private void ToogleSmokeEffectOnWait(bool active = false)
    {
        smoke_effect.SetActive(active);
    }
    private void ToogleCollision(bool hasCollision = false)
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (!hasCollision)
        {
            rb.excludeLayers = GameManager.self.layer_to_exclude;
        }
        else
        {
            rb.excludeLayers = GameManager.self.layer_to_exclude_default;
        }
    }
}
