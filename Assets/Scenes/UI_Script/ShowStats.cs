using UnityEngine;
using TMPro;

public class ShowStats : MonoBehaviour
{
    [SerializeField] TMP_Text Ram;
    [SerializeField] TMP_Text Fps;
    [SerializeField] TMP_Text deviceType;
    [SerializeField] TMP_Text deviceName;
    [SerializeField] TMP_Text deviceModel;
    private float deltaTime;

    private void Start()
    {
        Ram.text = "Device Ram : " + SystemInfo.systemMemorySize.ToString();
        deviceType.text = "Device Type : " + SystemInfo.deviceType.ToString();
        deviceName.text = "Device Name : " + SystemInfo.deviceName.ToString();
        deviceModel.text = "Device Model : " + SystemInfo.deviceModel.ToString();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = (int)(1.0f / deltaTime);
        Fps.text = "FPS : " + fps.ToString();
    }
}
