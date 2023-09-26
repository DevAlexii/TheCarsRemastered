using System;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Singleton<Hook>
{
    private List<GameObject> crashed_car;
    [HideInInspector] public int seleceted_car;
    private Action binded_event;
    private float timer;
    [SerializeField] private float max_time;
    [SerializeField] private GameObject hook_prefab;

    private void OnEnable()
    {
        binded_event = CountDown;
        crashed_car = new List<GameObject>();
        crashed_car = Car_Manager.self.car_crashed;
        seleceted_car = 0;
        CustomLibrary.SetGlobalTimeDilation(0.01f);
    }
    private void OnDisable()
    {
        binded_event = null;
        if (crashed_car.Count > 0)
        {
            crashed_car.Clear();
        }
        hooks.Clear();
        Car_Manager.self.car_crashed.Clear();
        CustomLibrary.SetGlobalTimeDilation(1);
    }
    private void Update()
    {
        binded_event?.Invoke();
    }
    private void CountDown()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > max_time)
        {
            timer = 0;
            enabled = false;
            binded_event = null;
            GameManager.self.E_GameOver();
        }
        CheckCarSelection();
    }
    List<GameObject> hooks = new List<GameObject>();
    private void CheckCarSelection()
    {
        if (seleceted_car == crashed_car.Count)
        {
            for (int i = 0; i < crashed_car.Count; i++)
            {
                GameObject hook = Instantiate(hook_prefab, crashed_car[i].transform.position + Vector3.up * 10, Quaternion.identity, transform);
                hooks.Add(hook);
            }
            binded_event -= CountDown;
            binded_event = HookGrabDownAnimation;
        }
    }
    private void HookGrabDownAnimation()
    {
        for (int i = 0; i < hooks.Count; i++)
        {
            hooks[i].transform.position = Vector3.Lerp(hooks[i].transform.position, crashed_car[i].transform.GetChild(0).transform.position, Time.unscaledDeltaTime * 3f);
            if (Vector3.Distance(hooks[i].transform.position, crashed_car[i].transform.GetChild(0).transform.position) <= .5f)
            {
                for (int j = 0; j < hooks.Count; j++)
                {
                    crashed_car[j].transform.parent = hooks[j].transform;
                }
                binded_event -= HookGrabDownAnimation;
                binded_event = HookGrabUpAnimation;
            }
        }
    }
    private void HookGrabUpAnimation()
    {
        for (int i = 0; i < hooks.Count; i++)
        {
            hooks[i].transform.position = Vector3.Lerp(hooks[i].transform.position, new Vector3(hooks[i].transform.position.x, 15, hooks[i].transform.position.z), Time.unscaledDeltaTime * 5f);
            if (hooks[i].transform.position.y >= 10)
            {
                foreach (GameObject hook in hooks)
                {
                    Destroy(hook);
                }
                enabled = false;
            }
        }
    }
}
