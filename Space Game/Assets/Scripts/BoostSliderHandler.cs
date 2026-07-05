/*
Notes:
 - 

*/
using UnityEngine;
using UnityEngine.UI;

public class BoostSliderHandler : MonoBehaviour
{
    protected GameObject rocketShipObj;

    #region Unity Functions
    private void Awake()
    {
        rocketShipObj = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        UpdateBoostSlider();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Function to update the slider to the current boost amount.
    /// </summary>
    private void UpdateBoostSlider()
    {
        this.gameObject.GetComponent<Slider>().value = rocketShipObj.GetComponent<ShipControlsScript>().Boost;
    }
    #endregion
}
