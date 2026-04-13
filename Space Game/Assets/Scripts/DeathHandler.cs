using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    private bool hasDied = false;
    private BaseShipScript baseShipScript;
    protected GameObject rocketShipObj;
    protected GameObject soundObj;
    private GameObject[] DebrisObjs;
    [SerializeField] private GameObject DebrisPrefab;
    [SerializeField] private GameObject explosionObj;
    private readonly Vector2[] debrisDirections = new Vector2[3];

    private void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        baseShipScript = rocketShipObj.GetComponent<BaseShipScript>();

        DebrisObjs = new GameObject[3];
        debrisDirections[0] = Vector2.left * 1000;
        debrisDirections[1] = Vector2.right * 1000;
        debrisDirections[2] = Vector2.down * 1000;
    }

    public void HandleDeath(GameObject spawner)
    {
        if (!hasDied)
        {
            soundObj.GetComponent<SoundHandler>().StopAudioRocketFlyingClip();
            Instantiate(explosionObj, rocketShipObj.transform.position, rocketShipObj.transform.rotation);
            for (int i = 0; i < 3; ++i)
            {
                DebrisObjs[i] = Instantiate(DebrisPrefab, rocketShipObj.transform.position, rocketShipObj.transform.rotation);
                DebrisObjs[i].GetComponent<DebrisHandler>().SetDirection(debrisDirections[i]);
            }

            baseShipScript.DestroyShip();
            baseShipScript.OnDeathAnimation();
            hasDied = true;
        }
    }

    public void ResetDebris()
    {
        foreach (GameObject debris in DebrisObjs)
        {
            Destroy(debris);
        }
        hasDied = false;
    }
}
