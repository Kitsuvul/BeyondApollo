/*
Notes:

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetScript : PlanetClass {

    #region Private Variables
    private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 10.0f);
    private LineRenderer lineRenderer;
    private int lineSteps = 50;
    private int maxDistance = 500;
    private bool isOutOfRange = false;
    private bool isSatellite = false;
    private GameObject planetToOrbit;
    private Vector3 orbitDirection;
    #endregion

    public bool IsOutOfRange
    { get { return isOutOfRange; } }

    public bool IsSatellite
    { 
        get { return isOutOfRange; }
        set { isSatellite = value; }
    }

    private void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RotatePlanet();
        if (PlanetSize != PlanetSizeEnum.Tiny)
        {
            DrawGravityIndicator();
        }
    }

    private void FixedUpdate()
    {
        float rocketPlanetMag = CalculateDistance(rocketShipObj.transform.position, this.transform.position);

        if (rocketPlanetMag > maxDistance)
        {
            isOutOfRange = true;
        }

        if(isSatellite)
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

    public void SetSatellite(GameObject planet)
    {
        planetToOrbit = planet;
    }

    private void HandleSatellite(GameObject planetToOrbit)
    {
        transform.RotateAround(planetToOrbit.transform.position, Vector3.forward, 25 * Time.deltaTime);
    }

    private float CalculateDistance(Vector3 rocketPos, Vector3 planetPos)
    {
        return (planetPos - rocketPos).magnitude;
    }

    private void HandleGravity()
    {
        PlanetGravity(this.GravityRadius, this.GravityStrength);
    }

    /// <summary>
    /// Makes the planet rotate around its z axis
    /// </summary>
    private void RotatePlanet()
    {
        this.transform.Rotate(rotationVec * Time.deltaTime);
    }

    void PlanetGravity(float gravitySize, float gravityForce)
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

    void DrawGravityIndicator()
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
