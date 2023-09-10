using System.Collections.Generic;
using UnityEngine;

public class Car_Core : MonoBehaviour, Car_Interface
{
    [SerializeField] private CarFollowPath carFollowPathRef;
    [SerializeField] private List<GameObject> directional_arrwos;
    private int directional_arrow_index_to_play;

    #region Initialized
        public void OnInitializedCar(Path newPath, int arrow_index)
        {
            carFollowPathRef.InitilizedPath(newPath, this);
            directional_arrow_index_to_play = arrow_index;
        }
        #endregion

    #region DirectionalArrow
    public void ShowDirectionalArrow()
    {
        if (directional_arrow_index_to_play >= 0)
        {
            GameObject directional_arrow = directional_arrwos[directional_arrow_index_to_play];
            directional_arrow.SetActive(true);
            directional_arrow.AddComponent<Directional_Arrow_Animation>();
        }
    }
    public void HideDirectionalArrow()
    {
        if (directional_arrwos.Count <= 0) return;
        foreach (var gameObject in directional_arrwos)
        {
            Destroy(gameObject);
        }
        directional_arrwos.Clear();
    }
    #endregion


    public void OnCarClicked()
    {
        carFollowPathRef.ToogleShouldMove();
    }
    public void EnableInvisiblity()
    {

    }
}
