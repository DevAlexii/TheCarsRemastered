using UnityEngine;

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
}
