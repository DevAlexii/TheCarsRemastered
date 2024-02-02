using UnityEngine;
using TMPro;

public class ShowStats : MonoBehaviour
{
    [SerializeField] TMP_Text Fps;

    void Update()
    {
        int fps = (int)(1.0f / Time.deltaTime);
        Fps.text = "FPS : " + fps.ToString();
    }
}
