using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMemePicker : MonoBehaviour
{
    public List<GameObject> memeObj;
    AudioType currentMeme;
    GameObject activeMeme;

    private void Start()
    {
        ChooseRandomMeme();
    }

    void ChooseRandomMeme()
    {
        if (activeMeme != null)
        {
            activeMeme.SetActive(false);
        }

        int index = Random.Range(0, memeObj.Count);

        for (int i = 0; i < memeObj.Count; i++)
        {
            memeObj[i].SetActive(i == index);
        }

        currentMeme = AudioType.StartMemeEnum + 1 + index;
        activeMeme = memeObj[index];
        activeMeme.SetActive(true);
    }
}