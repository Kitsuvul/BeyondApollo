using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// DEPRECATED
/// </summary>
public class BackgroundScript : MonoBehaviour
{
    #region Variables
    private Vector2 currentBackgroundPos = new Vector2(0f, 0f);
    private Vector2 spawnPositionStart = new Vector2(-4096f, -4096f);
    private GameObject rocketShipObj;
    private HelperScript helperScript;
    [SerializeField] public GameObject backgroundPrefab;
    private List<GameObject> spawnedObjs;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        helperScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<HelperScript>();
        spawnedObjs = new List<GameObject>();
        GameObject currBackground = GameObject.FindGameObjectWithTag("Background");
        spawnedObjs.Add(currBackground);
    }

    // Add function to check if player is within square

    void Update()
    {
        if (backgroundPrefab != null && rocketShipObj != null)
        {
            SpawnNewBackground();
            UpdateCurrentBackground();
            ClearPreviousBackgrounds();
        }
    }
    #endregion

    #region Private Functions
    private void SpawnNewBackground()
    {
        if (!CheckIfWithinBox(rocketShipObj, currentBackgroundPos, new Vector3(3500f, 3500f)))
        {
            Vector2 nextPos = spawnPositionStart + currentBackgroundPos;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i == 1 && j == 1)
                    {
                        continue;
                    }

                    if (CheckIfWithinBox(rocketShipObj, nextPos, new Vector3(6000f, 6000f)))
                    {
                        bool spawn = true;
                        foreach (GameObject gameObject in spawnedObjs)
                        {
                            if (helperScript.GetVec2FromPositionHelper(gameObject.transform.position) == nextPos)
                            {
                                spawn = false;
                            }
                        }

                        if (spawn)
                        {
                            spawnedObjs.Add(Instantiate(backgroundPrefab, nextPos, Quaternion.identity));
                        }
                    }
                    nextPos.x += 4096;
                }
                nextPos.x = spawnPositionStart.x + currentBackgroundPos.x;
                nextPos.y += 4096;
            }
        }
    }

    private void UpdateCurrentBackground()
    {
        foreach (GameObject gameObject in spawnedObjs)
        {
            if (CheckIfWithinBox(rocketShipObj, gameObject.transform.position, new Vector3(4096f, 4096f)))
            {
                currentBackgroundPos = helperScript.GetVec2FromPositionHelper(gameObject.transform.position);
                break;
            }
        }
    }

    private void ClearPreviousBackgrounds()
    {
        foreach (GameObject gameObject in spawnedObjs)
        {
            if (!CheckIfWithinBox(rocketShipObj, gameObject.transform.position, new Vector3(10000f, 10000f)))
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    private bool CheckIfWithinBox(GameObject gameObject, Vector2 currentBackground, Vector3 size)
    {
        Bounds bound = new Bounds(currentBackground, size);

        if (gameObject != null)
        {
            if(bound.Contains(gameObject.transform.position)) { return true; }
        }
        return false;
    }
    #endregion
}
