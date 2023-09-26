using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Life")]
    [SerializeField] private Int32 n_life;
    public void UpdateLife(Int32 amount) { n_life += amount; }
    [Header("Score")]
    private Int32 score;
    public Int32 Score => score;
    public void UpdateScore(Int32 amount)
    {
        score += amount;
        if (score % 50 == 0)
        {
            PowerUpManager.self.SpawnHealth();
        }
    }
    [Header("Difficulty")]
    public int difficulty = 50;
    [Header("HookRef")]
    [SerializeField] private Hook hook_component;


    public void E_OnCarCrash()
    {
        if (n_life > 0)
        {
            n_life--;
            Invoke(nameof(EnableHook), 0.3f);
        }
        else
        {
            Invoke(nameof(E_GameOver), 0.3f);
        }
    }
    private void EnableHook()
    {
        if (!hook_component.enabled)
            hook_component.enabled = true;
    }
    public void E_GameOver()
    {
        //ToDo
        CustomLibrary.SetGlobalTimeDilation(1);


        //ForDebugCall
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
