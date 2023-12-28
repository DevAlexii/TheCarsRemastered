using System;
using System.Collections;
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

    [Header("Scale Multiplier")]
    private Vector3 originalScale;

    [Header("ColorOutline")]
    public Color color;
    private Outline outlineScript;

    #region Initialize
    public void InitilizedPath(Path newPath, Car_Core Owner, bool Kamikaze, CarInfo data)
    {
        owner = Owner;//SetReference
        queque_ray = new Ray();//GenerateQuequeRay
        queque_distance = data.QuequeRange;//maxDistanceForQuequeRay
        MovementVarSetup(data);//SetupMovement
        path = newPath; //SetUpPath
        is_kamikaze = Kamikaze; //IsKamikaze?
        if (!is_kamikaze) { can_be_touched = true; }
        On_CarMove = MoveCarToNode;
        originalScale = transform.localScale;
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
        if (node_index <= 1) CheckQueque();

        float distance = Vector3.Distance(GetNodePosition(), transform.position);
        if (distance <= node_reachable_distance)
        {
            if (path.Nodes[node_index].name.StartsWith("Stop") && !is_kamikaze)
            {
                //ToogleCollision(true);
                stop_car = true;
                owner.ShowDirectionalArrow();
                On_Waiting = Waiting;
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
                StopCoroutine(ToogleWaitSize());
                StopCoroutine(ResetScale(0f));
                startWaitSize = false;
                Outline outlineScript = GetComponentInChildren<Outline>();
                outlineScript.OutlineColor = GameManager.self.Get_Start_Outline_Color;
                outlineScript.OutlineWidth = 1.2f;
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
        Destroy(this);
    }
    bool startWaitSize;
    private void Waiting()
    {
        wait_timer += Time.deltaTime;

        if (wait_timer > wait_time - 2f)
        {
            if (!startWaitSize)
            {
                StartCoroutine(ToogleWaitSize());
            }
            float transitionProgress = Mathf.InverseLerp(wait_time - 2f, wait_time, wait_timer);

            float widthTransition = Mathf.SmoothStep(1.2f, 3f, transitionProgress);

            Outline outlineScript = GetComponentInChildren<Outline>();

            outlineScript.OutlineColor = Color.Lerp(GameManager.self.Get_Start_Outline_Color, GameManager.self.GetWaitingColor, transitionProgress);
            outlineScript.OutlineWidth = widthTransition;
        }
        if (wait_timer >= wait_time)
        {
            wait_timer = 0;
            ToogleShouldMove();
            pickCombo = false;
            Car_Manager.self.comboCount = 0;
            On_Waiting = null;
            transform.localScale = originalScale;
            Outline outlineScript = GetComponentInChildren<Outline>();
            outlineScript.OutlineColor = GameManager.self.Get_Start_Outline_Color;
            outlineScript.OutlineWidth = 1.2f;
            startWaitSize = false;
        }
    }
    IEnumerator ToogleWaitSize()
    {
        startWaitSize = true;
        float startScale = GetComponentInChildren<Car_Core>().shrink_on ? 0.5f : 1;

        float maxScale = startScale * 1.1f;
        float minScale = startScale * 0.9f;
        float targetScale = maxScale;
        float scaleValue = startScale;

        while (startWaitSize)
        {
            scaleValue = Mathf.Lerp(scaleValue, targetScale, Time.deltaTime * 10);
            if (scaleValue > maxScale - 0.01f && targetScale == maxScale)
            {
                scaleValue = maxScale;
                targetScale = minScale;
            }
            else if (scaleValue < minScale + 0.01f && targetScale == minScale)
            {
                scaleValue = minScale;
                targetScale = maxScale;
            }
            transform.localScale = new Vector3(originalScale.x * scaleValue, originalScale.y, originalScale.z * scaleValue);
            yield return null;
        }
        StopCoroutine(ToogleWaitSize());
        StartCoroutine(ResetScale(scaleValue));
        yield return null;
    }
    IEnumerator ResetScale(float lastScale)
    {
        float startScale = GetComponentInChildren<Car_Core>().shrink_on ? 0.5f : 1;
        float newscaleValue = lastScale;
        while (newscaleValue > startScale + 0.01f)
        {
            newscaleValue = Mathf.Lerp(newscaleValue, startScale, Time.deltaTime * 10);
            transform.localScale = new Vector3(originalScale.x * newscaleValue, originalScale.y, originalScale.z * newscaleValue);
            yield return null;
        }
        transform.localScale = originalScale;
        StopCoroutine(ResetScale(0f));
        yield break;
    }
    public bool ComboCounter()
    {
        if (ToogleShouldMove())
        {
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
        queque_ray.origin = transform.position + shell.forward + Vector3.up * 0.5f;
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
