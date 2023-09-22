using UnityEngine;

[ExecuteInEditMode]
public class DayNightTest01 : MonoBehaviour
{
    public bool enable_time;

    [Header("Day_Varaiables")]
    [Range(0f,1f)] public float time;
    public float dayLenght;
    public float startday;
    public Vector3 noon;
    private float timeRate;

    [Header("Sun")]
    [SerializeField] Light sun;
    [SerializeField] AnimationCurve sunIntensity;
    public Gradient sunGradient;

    [Header("Moon")]
    [SerializeField] Light moon;
    [SerializeField] AnimationCurve moonintesnity;
    public Gradient moonGradient;

    [SerializeField] AnimationCurve intensity_multiplier;
    [SerializeField] AnimationCurve reflexion_multiplier;

    void Start()
    {
        timeRate = 1f / dayLenght;

        time = startday;
    }

    void Update()
    {
        if (enable_time)
        {
            time += timeRate * Time.deltaTime;
            if (time >= 1f)
            {
                time = 0f;
            }
        }
        SetLighting();
        SetSunMoonintensity();
        SetSunMoonActive();
    }
    void SetLighting()
    {
        RenderSettings.ambientIntensity = intensity_multiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflexion_multiplier.Evaluate(time);
    }
    void SetSunMoonintensity()
    {
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonintesnity.Evaluate(time);

        sun.color = sunGradient.Evaluate(time);
        moon.color = moonGradient.Evaluate(time);
    }
    void SetSunMoonActive()
    {
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);

        }
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);

        }
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);

        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }
    }
}
