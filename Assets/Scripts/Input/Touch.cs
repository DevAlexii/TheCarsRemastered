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
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.transform.TryGetComponent(out I_Interface clickedCar))
                    {
                        clickedCar.OnClicked();
                    }
                }
            }
        }
#endif
        //#if UNITY_EDITOR_WIN
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            RaycastHit hit;
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            if (Physics.Raycast(ray, out hit, 100f))
        //            {
        //                if (hit.transform.TryGetComponent(out I_Interface clickedCar))
        //                {
        //                    clickedCar.OnClicked();
        //                }
        //            }
        //        }
        //#endif
    }
}
