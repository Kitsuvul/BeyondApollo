using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetExtraLargeScript : PlanetBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GravityRadius = 25f;
        GravityStrength = 7.5f;
        PlanetSize = PlanetSizeEnum.ExtraLarge;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = planetSprites[0];
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
