/*
Notes:

*/
using UnityEngine;

public class CometHandler : MonoBehaviour
{
    private Vector2 orbitCentrePoint;
    private Vector3 orbitDirection;
    private LevelLoader levelLoader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        levelLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelLoader>();
        orbitCentrePoint = new Vector2(0f, -71.0f);
        RandomizeCometDirectionAndSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelLoader != null && levelLoader.LevelLoaded)
        {
            transform.RotateAround(orbitCentrePoint, orbitDirection, 50 * Time.deltaTime);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RandomizeCometDirectionAndSpeed()
    {
        int randomNumber = Random.Range(0, 9);
        if (randomNumber < 5)
        {
            orbitDirection = Vector3.forward;
        }
        else
        {
            orbitDirection = -Vector3.forward;
        }
    }
}
