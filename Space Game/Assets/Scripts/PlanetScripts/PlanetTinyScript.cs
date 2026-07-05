using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetExtraSmallScript : PlanetBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GravityRadius = 0.0f;
        GravityStrength = 0.0f;
        PlanetSize = PlanetSizeEnum.Tiny;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = planetSprites[Random.Range(0, planetSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlanet();
    }
}
