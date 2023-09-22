using UnityEngine;
using TMPro;

public class ShowStats : MonoBehaviour
{
    [SerializeField] TMP_Text Ram;
    [SerializeField] TMP_Text Fps;
    private float deltaTime;

    private void Start()
    {
        Ram.text = SystemInfo.systemMemorySize.ToString();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = (int)(1.0f / deltaTime);
        Fps.text = fps.ToString();
    }
}
