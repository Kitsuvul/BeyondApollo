/*
Notes:
 - 

*/
using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    private struct PlanetToLoad
    {
        public GameObject planetPrefab;
        public Vector3 Position;
        public bool isSatellite;
    };

    XmlDocument levelDataXml;

    #region Variables
    private GameObject winTriggerObj, rocketShipObj, settingsObj, cometObj, storageObj;
    [SerializeField] private GameObject PlanetTinyPrefab;
    [SerializeField] private GameObject PlanetSmallPrefab;
    [SerializeField] private GameObject PlanetMediumPrefab;
    [SerializeField] private GameObject PlanetLargePrefab;
    [SerializeField] private GameObject PlanetExtraLargePrefab;
    [SerializeField] private GameObject CometPrefab;
    private SaveManager saveManagerScript;
    private HelperScript helperScript;
    private int currentLevel = 0;
    private bool levelLoaded = false;
    private List<GameObject> loadedPlanetObjs;
    private Vector3 previousPos;
    private Vector2 cometStartPoint;
    int frames;
    int totalLevels;
    #endregion

    #region Properties
    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public bool LevelLoaded
    {
        get { return levelLoaded; }
        private set { levelLoaded = value; }
    }

    public int TotalLevels
    {
        get { return totalLevels; }
        private set { totalLevels = value; }
    }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        InitXML();

        loadedPlanetObjs = new List<GameObject>();

        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        settingsObj = GameObject.FindGameObjectWithTag("PersistSettings");
        cometObj = GameObject.FindGameObjectWithTag("Comet");
        helperScript = this.gameObject.GetComponent<HelperScript>();

        if (settingsObj != null)
        {
            saveManagerScript = settingsObj.GetComponent<SaveManager>();
            currentLevel = saveManagerScript.GetHighestLevelInPlayerPrefs();
        }

        if (!helperScript.IsEndlessMode())
        {
            winTriggerObj = GameObject.FindGameObjectWithTag("WinBox");
            winTriggerObj.transform.position = new Vector3(0.0f, 95.0f, 0.0f);
        }
        else
        {
            storageObj = GameObject.FindGameObjectWithTag("PlanetHolder");
        }
        
        previousPos = new Vector3(0.0f, 0.0f, 0.0f);
        frames = 0;
    }

    public void Update()
    {
        if (helperScript.IsStoryMode())
        {
            if (!levelLoaded && PlanetSmallPrefab != null && PlanetMediumPrefab != null && PlanetLargePrefab != null)
            {
                LoadLevel(currentLevel);
            }
        }
        else if (helperScript.IsEndlessMode())
        {
            GenerateEndlessLevel();
        }
    }
    #endregion

    #region Endless Methods
    private void GenerateEndlessLevel()
    {
        frames++;

        Vector3 currentPos = rocketShipObj.transform.position;

        if (frames % 5 == 0)
        {
            if (currentPos != previousPos && rocketShipObj.GetComponent<ShipControlsScript>().HasLaunched)
            {
                Vector2 newSpawnPoint;
                Vector2 planetSpawnPoint = FindCentrePoint(currentPos, previousPos);

                newSpawnPoint = (new Vector2(planetSpawnPoint.x, planetSpawnPoint.y) + Random.insideUnitCircle * 200);
                if (CheckSpawn(newSpawnPoint) == true)
                {
                    SpawnPlanet(Random.Range(0, 3), newSpawnPoint, new Quaternion(0, 0, 0, 0));
                }

            }

            foreach (GameObject planet in loadedPlanetObjs)
            {
                if(planet != null && planet.GetComponent<PlanetScript>().IsOutOfRange)
                {
                    loadedPlanetObjs.Remove(planet);
                    DestroyPlanet(planet);
                    break;
                }
            }

            previousPos = currentPos;
        }

        levelLoaded = true;
    }

    public Vector2 FindCentrePoint(Vector3 cPos, Vector3 pPos)
    {
        Vector2 shipDirection = cPos - pPos;

        Vector2 finaldirection = shipDirection + (shipDirection.normalized * 350);

        Vector2 targetPos = new Vector2(pPos.x, pPos.y) + finaldirection;

        return targetPos;
    }

    public bool CheckSpawn(Vector3 planetLoc)
    {
        foreach (GameObject planet in loadedPlanetObjs)
        {
            if (planet != null)
            {
                int magni = Mathf.RoundToInt(Vector3.Distance(planetLoc, planet.transform.position));

                if (magni <= 12 && magni >= 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SpawnPlanet(int size, Vector3 location, Quaternion rotation)
    {
        GameObject spawnedPlanet;
        switch (size)
        {
            case 0:
                spawnedPlanet = Instantiate(PlanetSmallPrefab, location, rotation);
                break;
            case 1:
                spawnedPlanet = Instantiate(PlanetMediumPrefab, location, rotation);
                break;
            case 2:
                spawnedPlanet = Instantiate(PlanetLargePrefab, location, rotation);
                break;
            default:
                spawnedPlanet = Instantiate(PlanetSmallPrefab, location, rotation);
                break;
        }

        spawnedPlanet.transform.SetParent(storageObj.transform);
        loadedPlanetObjs.Add(spawnedPlanet);
    }
    #endregion

    #region Story Methods
    /// <summary>
    /// 
    /// </summary>
    private void InitXML()
    {
        levelDataXml = new XmlDocument();
        levelDataXml.LoadXml(Resources.Load<TextAsset>("XML/LevelsNew").text);
        totalLevels = int.Parse(levelDataXml.SelectSingleNode("/Levels/TotalLevels").InnerText);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelID"></param>
    /// <returns></returns>
    private List<PlanetToLoad> GetLevelData(int levelID)
    {
        List<PlanetToLoad> planetsToLoad = new List<PlanetToLoad>();

        XmlNode level = levelDataXml.SelectSingleNode("/Levels/Level[@ID='" + levelID + "']");
        if(level != null)
        {
            foreach (XmlNode node in level.ChildNodes)
            {
                PlanetToLoad planet = new PlanetToLoad();
                string size = node["Size"].InnerText;
                GameObject prefab;

                switch (size)
                {
                    case "Tiny":
                        prefab = PlanetTinyPrefab;
                        break;
                    case "Small":
                        prefab = PlanetSmallPrefab;
                        break;
                    case "Medium":
                        prefab = PlanetMediumPrefab;
                        break;
                    case "Large":
                        prefab = PlanetLargePrefab;
                        break;
                    case "ExtraLarge":
                        prefab = PlanetExtraLargePrefab;
                        break;
                    default:
                        Debug.LogError("Planet size not set in Levels.xml at level: " + levelID + ". Defaulting to Small Planet");
                        prefab = PlanetSmallPrefab;
                        break;
                }

                XmlNode posNode = node.SelectSingleNode("Position");
                Vector3 planetPos = new Vector3(float.Parse(posNode["x"].InnerText), float.Parse(posNode["y"].InnerText), float.Parse(posNode["z"].InnerText));

                bool isSatellite = false;
                if (node["Satellite"] != null)
                {
                    isSatellite = int.Parse(node["Satellite"].InnerText) != 0;
                }

                planet.planetPrefab = prefab;
                planet.Position = planetPos;
                planet.isSatellite = isSatellite;
                planetsToLoad.Add(planet);
            }
        }
        else
        {
            Debug.LogError("Data missing for level: " + levelID);
        }

            return planetsToLoad;
    }

    /// <summary>
    /// Loads the level defined by the passed int
    /// </summary>
    /// <param name="x">The level to be loaded</param>
    private void LoadLevel(int x)
    {
        Debug.Log("Level:" + currentLevel);
        cometStartPoint = new Vector2(Random.Range(100f, 141f), -71f);
        cometObj = Instantiate(CometPrefab, cometStartPoint, Quaternion.identity);

        List<PlanetToLoad> levelToLoad = GetLevelData(currentLevel);

        foreach (var planet in levelToLoad)
        {
            GameObject planetObj = Instantiate(planet.planetPrefab, planet.Position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            if(planet.isSatellite)
            {
                planetObj.GetComponent<PlanetScript>().SetSatellite(loadedPlanetObjs[loadedPlanetObjs.Count() - 1]);
                planetObj.GetComponent<PlanetScript>().IsSatellite = planet.isSatellite;
            }
            loadedPlanetObjs.Add(planetObj);
        }
        levelLoaded = true;
    }

    /// <summary>
    /// Loads the next level by incrementing the currentLevel and reseting bools
    /// </summary>
    public void LoadNextLevel()
    {
        rocketShipObj.GetComponent<ShipControlsScript>().ResetShipToStart();
        rocketShipObj.GetComponent<PreviousPathHandler>().ClearMarkers();
        DestroyPlanets();
        currentLevel++;
        if (settingsObj != null && currentLevel > saveManagerScript.GetHighestLevelInPlayerPrefs())
        {
            saveManagerScript.SetHighestLevelInPlayerPrefs(currentLevel);
        }
        levelLoaded = false;
    }

    /// <summary>
    /// Loads the next level by incrementing the currentLevel and reseting bools
    /// </summary>
    public void LoadSpecificLevel(int level)
    {
        rocketShipObj.GetComponent<ShipControlsScript>().ResetShipToStart();
        rocketShipObj.GetComponent<PreviousPathHandler>().ClearMarkers();
        DestroyPlanets();
        currentLevel = level;
        levelLoaded = false;
    }

    /// <summary>
    /// Reloads the current level
    /// </summary>
    public void ReloadLevel()
    {
        rocketShipObj.GetComponent<ShipControlsScript>().ResetShipToStart();
        DestroyPlanets();
        levelLoaded = false;
    }

    /// <summary>
    /// Destroys all the loaded planets, used for reseting or loading a new level
    /// </summary>
    private void DestroyPlanets()
    {
        if (loadedPlanetObjs.Count != 0)
        {
            foreach (GameObject planet in loadedPlanetObjs)
            {
                Destroy(planet);
            }
        }
        Destroy(cometObj);
    }

    /// <summary>
    /// Destroys all the loaded planets, used for reseting or loading a new level
    /// </summary>
    private void DestroyPlanet(GameObject planet)
    {
        if (planet != null)
        {
            Destroy(planet);
        }
    }
    #endregion
}