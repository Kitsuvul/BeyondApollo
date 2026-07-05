/*
Notes:

*/
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlanetSizeEnum
{
    Tiny,
    Small,
    Medium,
    Large,
    ExtraLarge
}

public class PlanetBase : MonoBehaviour
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

    [SerializeField] protected Sprite[] planetSprites;
    [SerializeField] private GameObject explosionObj;

    protected Vector3 rotationVec = new Vector3(0.0f, 0.0f, 10.0f);
    protected LineRenderer lineRenderer;
    protected int lineSteps = 50;
    protected int maxDistance = 500;
    protected bool isOutOfRange = false;
    protected bool isSatellite = false;
    protected GameObject planetToOrbit;
    protected Vector3 orbitDirection;

    public PlanetSizeEnum PlanetSize
    {
        get { return planetSize; }
        protected set { planetSize = value; }
    }
    public float GravityRadius
    {
        get { return gravityRadius; }
        protected set { gravityRadius = value; }
    }
    public float GravityStrength
    {
        get { return gravityStrength; }
        protected set { gravityStrength = value; }
    }
    public bool IsInGravity
    {
        get { return isInGravity; }
        set { isInGravity = value; }
    }
    public bool IsOutOfRange
    { get { return isOutOfRange; } }

    public bool IsSatellite
    {
        get { return isOutOfRange; }
        set { isSatellite = value; }
    }

    void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        canvasObj = GameObject.FindGameObjectWithTag("CanvasUI");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        GameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == rocketShipObj.GetComponent<Collider2D>())
        {
            GameControllerObj.GetComponent<DeathHandler>().HandleDeath(this.gameObject);
        }
    }

    public void SetSatellite(GameObject planet)
    {
        planetToOrbit = planet;
    }

    protected void HandleSatellite(GameObject planetToOrbit)
    {
        transform.RotateAround(planetToOrbit.transform.position, Vector3.forward, 25 * Time.deltaTime);
    }

    protected float CalculateDistance(Vector3 rocketPos, Vector3 planetPos)
    {
        return (planetPos - rocketPos).magnitude;
    }

    protected void HandleGravity()
    {
        PlanetGravity(this.GravityRadius, this.GravityStrength);
    }

    /// <summary>
    /// Makes the planet rotate around its z axis
    /// </summary>
    protected void RotatePlanet()
    {
        this.transform.Rotate(rotationVec * Time.deltaTime);
    }

    protected void PlanetGravity(float gravitySize, float gravityForce)
    {
        float rocketPlanetMag = CalculateDistance(rocketShipObj.transform.position, this.transform.position);

        if (rocketPlanetMag < gravitySize)
        {
            rocketShipObj.GetComponent<Rigidbody2D>().AddForce((this.transform.position - rocketShipObj.transform.position) * gravityForce);
            IsInGravity = true;
            Debug.Log("IS IN GRAVITY");
        }
        else { IsInGravity = false; }
    }

    protected void DrawGravityIndicator()
    {
        lineRenderer.positionCount = lineSteps;

        for (int currentStep = 0; currentStep < lineSteps; currentStep++)
        {
            float circumfrerenceProg = (float)currentStep / lineSteps;
            float currentRaidan = circumfrerenceProg * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRaidan);
            float yScaled = Mathf.Sin(currentRaidan);

            float x = xScaled * GravityRadius;
            float y = yScaled * GravityRadius;

            Vector3 currentPosition = new Vector3(x, y, 0) + this.gameObject.transform.position;
            lineRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
