/*
Notes:

*/
using UnityEngine;

public class MaxDistanceScript : MonoBehaviour
{
    private GameObject rocketShipObj, soundObj;
    private BaseShipScript baseShipScript;

    private void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
        baseShipScript = rocketShipObj.GetComponent<BaseShipScript>();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (rocketShipObj != null && rocketShipObj.GetComponent<ShipControlsScript>().HasLaunched)
        {
            if (collision == rocketShipObj.GetComponent<Collider2D>())
            {
                soundObj.GetComponent<SoundHandler>().StopAudioRocketFlyingClip();
                baseShipScript.DestroyShip();
                baseShipScript.OnDeathAnimation();
            }
        }
    }
}
