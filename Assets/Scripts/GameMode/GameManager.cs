using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //Life
    private Int32 n_life;
    public void UpdateLife(Int32 amount) { n_life += amount; }
    //Score
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
    //Difficulty
    public int difficulty = 50;
}
