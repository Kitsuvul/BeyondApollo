using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public LC_PlanetSpawner[] planetButtons;
    public LC_PlanetSpawner lineButton;
    public GameObject[] planetPrefabs;
    private List<GameObject> spawnedObjs;
    public int CurrentButtonPressed;
    public InputField input;
    private XMLHandler outputHandler;
    public GameObject savePanelObj;
    private SortedDictionary<int, int> linkedPlanets;
    bool connectingPlanets = false;
    int currentlyConnecting = 0;

    private void Start()
    {
        outputHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<XMLHandler>();
        spawnedObjs = new List<GameObject>();
        linkedPlanets = new SortedDictionary<int, int>();
    }

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Input.GetMouseButtonDown(0) && planetButtons[CurrentButtonPressed].clicked)
        {
            planetButtons[CurrentButtonPressed].enabled = false;
            spawnedObjs.Add(Instantiate(planetPrefabs[CurrentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity));
            planetButtons[CurrentButtonPressed].clicked = false;
        }

        if(Input.GetMouseButtonDown(0) && (lineButton.clickedLine || connectingPlanets))
        {
            Vector2 raycast = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayCastHit = Physics2D.Raycast(raycast, Input.mousePosition);
            if(rayCastHit.collider.CompareTag("Small") || rayCastHit.collider.CompareTag("Medium") || rayCastHit.collider.CompareTag("Large"))
            {
                for (int i = 0; i < spawnedObjs.Count; ++i)
                {
                    if (rayCastHit.collider == spawnedObjs[i].GetComponent<Collider2D>())
                    {
                        if(!linkedPlanets.ContainsKey(i) && !linkedPlanets.ContainsValue(i))
                        {
                            if (!connectingPlanets)
                            {
                                linkedPlanets.Add(i, -1);
                                currentlyConnecting = i;
                                connectingPlanets = true;
                                Debug.Log("Connecting");
                            }
                            else
                            {
                                linkedPlanets[currentlyConnecting] = i;
                                currentlyConnecting = 0;
                                connectingPlanets = false;
                                lineButton.clickedLine = false;
                                Debug.Log("Connected");
                            }
                        }                    
                        else
                        {
                            Debug.Log("Already linked!");
                        }
                    }
                }
            }
        }
    }

    public void SaveLevel()
    {
        if (outputHandler.AddEntry(spawnedObjs, int.Parse(input.text), linkedPlanets))
        {
            foreach (GameObject obj in spawnedObjs)
            {
                Destroy(obj);
            }
            spawnedObjs.Clear();
        }
        else { Debug.Log("Failed to Save!");  }
    }

    public void CloseSavePanel()
    {
        if (savePanelObj != null)
        {
            savePanelObj.SetActive(false);
        }
    }

    public void ClearObjects()
    {
        foreach(GameObject obj in spawnedObjs)
        {
            Destroy(obj);
        }

        spawnedObjs.Clear();
    }
}
