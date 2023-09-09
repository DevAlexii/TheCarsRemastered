using UnityEngine;

public class Touch : MonoBehaviour
{
    void Update()
    {
#if UNITY_ANDROID
    if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.TryGetComponent(out Car_Interface clickedCar))
                    {
                        clickedCar.OnCarClicked();
                    }
                }
            }
        }
#endif
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,100f))
            {
                if (hit.transform.TryGetComponent(out Car_Interface clickedCar))
                {
                    clickedCar.OnCarClicked();
                }
            }
        }
    }
}
