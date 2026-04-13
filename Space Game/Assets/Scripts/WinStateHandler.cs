/*
Notes:

*/
using UnityEngine;

public class WinStateHandler : MonoBehaviour
{
    private GameObject rocketShipObj, soundObj;

    public void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
        soundObj = GameObject.FindGameObjectWithTag("SoundObject");
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == rocketShipObj.gameObject)
        {
            soundObj.GetComponent<SoundHandler>().StopAudioRocketFlyingClip();
            rocketShipObj.GetComponent<ShipControlsScript>().RemoveVelocityFromShip();
            rocketShipObj.GetComponent<BaseShipScript>().OnWinAnimation();
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
