using UnityEngine;
using TMPro;

public class ShowStats : MonoBehaviour
{
    [SerializeField] TMP_Text Fps;
   
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = (int)(1.0f / deltaTime);
        Fps.text = "FPS : " + fps.ToString();
    }
}
