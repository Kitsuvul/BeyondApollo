using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetLargeScript : PlanetBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GravityRadius = 15f;
        GravityStrength = 4.0f;
        PlanetSize = PlanetSizeEnum.Large;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = planetSprites[Random.Range(0, planetSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlanet();
        DrawGravityIndicator();
    }

    private void FixedUpdate()
    {
        float rocketPlanetMag = CalculateDistance(rocketShipObj.transform.position, this.transform.position);

        if (rocketPlanetMag > maxDistance)
        {
            isOutOfRange = true;
        }

        if (isSatellite)
        {
            HandleSatellite(planetToOrbit);
        }

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("LevelCreator"))
        {
            if (!rocketShipObj.GetComponent<BaseShipScript>().IsDead)
            {
                HandleGravity();
            }
        }
    }
}
