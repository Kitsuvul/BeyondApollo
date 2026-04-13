/*
Notes:

*/
using UnityEngine;

public enum PlanetSizeEnum
{
    Tiny,
    Small,
    Medium,
    Large,
    ExtraLarge
}

public class PlanetClass : MonoBehaviour
{
private
    PlanetSizeEnum planetSize;
    float gravityRadius;
    float gravityStrength;
    bool isInGravity = false;
    protected GameObject rocketShipObj;
    protected GameObject canvasObj;
    protected GameObject soundObj;
    protected GameObject GameControllerObj;
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private Sprite[] smallPlanetSprites;
    [SerializeField] private Sprite[] mediumPlanetSprites;
    [SerializeField] private Sprite[] largePlanetSprites;
    [SerializeField] private Sprite[] blackHoleSprites;
    [SerializeField] private GameObject explosionObj;

    public PlanetSizeEnum PlanetSize
    {
        get { return planetSize; }
        private set { planetSize = value; }
    }
    public float GravityRadius
    {
        get { return gravityRadius; }
        private set { gravityRadius = value; }
    }
    public float GravityStrength
    {
        get { return gravityStrength; }
        private set { gravityStrength = value; }
    }
    public bool IsInGravity
    {
        get { return isInGravity; }
        set { isInGravity = value; }
    }

    void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        canvasObj = GameObject.FindGameObjectWithTag("CanvasUI");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        GameControllerObj = GameObject.FindGameObjectWithTag("GameController");

        if (this.name.Contains("Asteroid"))
        {
            GravityRadius = 0.0f;
            GravityStrength = 0.0f;
            PlanetSize = PlanetSizeEnum.Tiny;
        }
        if (this.name.Contains("Planet S"))
        {
            GravityRadius = 10.0f;
            GravityStrength = 2.5f;
            PlanetSize = PlanetSizeEnum.Small;
        }
        else if (this.name.Contains("Planet M"))
        {
            GravityRadius = 12.5f;
            GravityStrength = 3.0f;
            PlanetSize = PlanetSizeEnum.Medium;
        }
        else if (this.name.Contains("Planet L"))
        {
            GravityRadius = 15f;
            GravityStrength = 4.0f;
            PlanetSize = PlanetSizeEnum.Large;
        }
        else if (this.name.Contains("Black Hole"))
        {
            GravityRadius = 25f;
            GravityStrength = 7.5f;
            PlanetSize = PlanetSizeEnum.ExtraLarge;
        }
    }

    private void Start()
    {
        if (this.name.Contains("Asteroid"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = asteroidSprites[0];
        }
        if (this.name.Contains("Planet S"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = smallPlanetSprites[1];
        }
        else if (this.name.Contains("Planet M"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = mediumPlanetSprites[1];
        }
        else if (this.name.Contains("Planet L"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = largePlanetSprites[1];
        }
        else if (this.name.Contains("Black Hole"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = blackHoleSprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == rocketShipObj.GetComponent<Collider2D>())
        {
            GameControllerObj.GetComponent<DeathHandler>().HandleDeath(this.gameObject);
        }
    }
}
