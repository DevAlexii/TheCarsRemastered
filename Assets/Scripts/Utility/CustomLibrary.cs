using UnityEngine;
using UnityEngine.EventSystems;

public static class CustomLibrary
{
    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 1;
    }
    public static bool RandomBoolInPercentage(int percentage)
    {
        return Random.Range(0, 101) <= percentage;
    }
    static float fixedAppoggio;
    public static void SetGlobalTimeDilation(float timeDilation)
    {
        if (timeDilation == 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = fixedAppoggio;
        }
        else
        {
            fixedAppoggio = Time.fixedDeltaTime;
            Time.timeScale = timeDilation;
            Time.fixedDeltaTime = Time.timeScale * Time.deltaTime;
        }
    }
}
