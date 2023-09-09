using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
  protected static T Self;

  public static T self
  {
    get
    {
      if(Self == null)
      {
                Self = (T)FindObjectOfType(typeof(T));

                if (Self == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                       " is needed in the scene, but there is none.");
                }
            }
      return Self;
    }
  }
}