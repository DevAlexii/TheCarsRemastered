using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public int scoreToSpawnObject = 50;
    public Sprite[] numberSprites;

    public Transform[] digitTransforms;
    public SpriteRenderer scoreRenderer;

    private bool scoreOverNine = false;
    //Life
    private Int32 n_life;
    [Header("Life")]
    [SerializeField] private Int32 n_life;
    public void UpdateLife(Int32 amount) { n_life += amount; }
    [Header("Score")]
    private Int32 score;
    public Int32 Score => score;

    private void Start()
    {
        for (int i = 0; i < digitTransforms.Length; i++)
        {
            SpriteRenderer digitRenderer = digitTransforms[i].GetComponent<SpriteRenderer>();
            if (digitRenderer != null)
            {
                digitRenderer.enabled = (i == 0);
            }
        }

        UpdateScore(0);
    }
    public void UpdateScore(Int32 amount)
    {
        score += amount;


        if (score % 50 == 0)
        {
            PowerUpManager.self.SpawnHealth();
        }
        UpdateNumberSprite();
    }
    [Header("Difficulty")]
    public int difficulty = 50;

    private void UpdateNumberSprite()
    {
        string scoreString = score.ToString(); 

        Vector3[] digitPositions;

        switch (scoreString.Length)//non ti arrabiare ale
        {
            case 1:
                digitPositions = new Vector3[] { new Vector3(-6.8f, 0.6f, 6.7f) };
                break;
            case 2:
                digitPositions = new Vector3[]
                {
                new Vector3(-6.7f, 0.6f, 7.2f),
                new Vector3(-7.4f, 0.6f, 6.6f)
                };
                break;
            case 3:
                digitPositions = new Vector3[]
                {
                new Vector3(-6.3f,0.6f,7.6f),
                new Vector3(-6.9f, 0.6f, 7f),
                new Vector3(-7.5f, 0.6f, 6.4f)
                };
                break;
            case 4:
                digitPositions = new Vector3[]
                {
                new Vector3(-5.9f,0.6f,7.9f),
                new Vector3(-6.5f,0.6f,7.3f),
                new Vector3(-7.2f,0.6f,6.6f),
                new Vector3(-7.9f,0.6f,5.9f)
                };
                break;
            default:
                digitPositions = new Vector3[] { Vector3.zero };
                break;
        }

        for (int i = 0; i < digitTransforms.Length; i++)
        {
            SpriteRenderer digitRenderer = digitTransforms[i].GetComponent<SpriteRenderer>();

            if (digitRenderer != null)
            {
                if (i < scoreString.Length)
                {
                    int digit = int.Parse(scoreString[i].ToString()); 
                    digitRenderer.sprite = numberSprites[digit]; 
                    digitRenderer.enabled = true; 

                    digitTransforms[i].localPosition = digitPositions[i];
                }
                else
                {
                    digitRenderer.enabled = false;
                }
            }
        }
    }
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




