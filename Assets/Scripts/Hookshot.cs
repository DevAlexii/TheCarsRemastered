using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hookshot : MonoBehaviour
{
    [Header("Hookshot")]
    public GameObject prefabToInstantiate;
    public float pullingSpeedDown;
    public float pullingSpeedUp;
    private bool canStartGrap = false;
    private bool carCrashed;

    public bool CarCrashedSelection { get { return carCrashed; } set { carCrashed = value; if (carCrashed) StartCoroutine(ResetCarCrashed()); } }
    private List<GameObject> hookedCar = new List<GameObject>();
    public bool grapActive;
    private GameObject selectedCar;
    public int numSelection;
    private float timer;


    public IEnumerator ResetCarCrashed()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.deltaTime * 0.1f;
        while (timer <= 10)
        {
            timer += Time.unscaledDeltaTime;

            CheckCanGrap();

            yield return null;
        }

        timer = 0;
        numSelection = 0;
        carCrashed = false;

        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.deltaTime;

        GameOver();
        yield return null;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void SelectCar()
    {
        numSelection++;
    }

    public void CheckCanGrap()
    {

        if (numSelection == hookedCar.Count)
        {
            hookCars = true;
            StartCoroutine(GrapAnimation());

            timer = 0;
            numSelection = 0;
            carCrashed = false;



            StopCoroutine(ResetCarCrashed());
        }


    }

    IEnumerator GrapAnimation()
    {
        grapActive = true;
        hookCars = true;

        if (hookedCar.Count > 0)
        {
            bool AscendGrap = false;
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < hookedCar.Count; i++)
            {
                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, hookedCar[i].transform.position + Vector3.up * 100, Quaternion.identity, hookedCar[i].transform);
                list.Add(instantiatedPrefab);
            }
            while (!AscendGrap)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].transform.position = Vector3.Lerp(list[i].transform.position, hookedCar[i].transform.position, Time.unscaledDeltaTime * pullingSpeedDown);
                    if (Vector3.Distance(list[i].transform.position, hookedCar[i].transform.position) < 1)
                    {
                        AscendGrap = true;
                    }
                }
                yield return null;
            }

            while (AscendGrap)
            {
                for (int i = 0; i < hookedCar.Count; i++)
                {
                    hookedCar[i].transform.position = Vector3.Lerp(hookedCar[i].transform.position, hookedCar[i].transform.position + Vector3.up * targetHeights, Time.unscaledDeltaTime * pullingSpeedUp);
                    if (hookedCar[i].transform.position.y >= targetHeights)
                    {
                        //hookedCar[i].GetComponent<CarEngine>().OnDestroy();
                        hookedCar.RemoveAt(i);
                    }
                }

                if (hookedCar.Count <= 0)
                {
                    AscendGrap = false;
                    hookCars = false;
                }
                yield return null;
            }


        }
        else
        {
            hookCars = false;
        }
        grapActive = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.deltaTime;
        yield break;
    }

    public bool hookCars;
    private float targetHeights = 10;

    public void HookCar()
    {
        if (hookedCar.Count > 0)
        {
            foreach (var car in hookedCar)
            {
                car.GetComponent<Rigidbody>().isKinematic = true;
            }
            hookCars = true;
        }
    }

    public void AddCar(GameObject car)
    {
        if (hookedCar.Count == 0)
        {
            hookedCar.Add(car);
        }
        else
        {
            foreach (var cars in hookedCar)
            {
                if (car == cars)
                {
                    return;
                }
            }
            hookedCar.Add(car);
        }
    }
}
