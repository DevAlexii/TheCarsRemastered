using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
   
    public int score = 0;
    public GameObject heartToSpawn;
    public int scoreToSpawnObject = 50;
    public Sprite[] numberSprites; 

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (score == 50)
        {
            SpawnObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            score++;
            UpdateNumberSprite(); 
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateNumberSprite()
    {
        if (score >= 0 && score < numberSprites.Length)
        {
            spriteRenderer.sprite = numberSprites[score];

            
            if (score % 50 == 0)
            {
                
                SpawnObject();
            }
        }
    }

    private void SpawnObject()
    {
        Instantiate(heartToSpawn, transform.position, Quaternion.identity);
    }
}