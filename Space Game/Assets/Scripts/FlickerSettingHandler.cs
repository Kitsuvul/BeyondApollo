using UnityEngine;
using UnityEngine.UI;

public class FlickerSettingHandler : MonoBehaviour
{
    private Toggle flickerToggle;
    private GameObject settingsObj;
    private SaveManager saveManagerScript;
    private bool canFlicker = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        flickerToggle = this.gameObject.GetComponent<Toggle>();
        settingsObj = GameObject.FindGameObjectWithTag("PersistSettings");
        saveManagerScript = settingsObj.GetComponent<SaveManager>();

        if (saveManagerScript != null && flickerToggle)
        {
            flickerToggle.isOn = saveManagerScript.GetUIFlickerInPlayerPrefs();
            flickerToggle.onValueChanged.AddListener(delegate { OnValueMuteAllToggle(); });
        }
    }

    private void Update()
    {
        if (canFlicker != saveManagerScript.GetUIFlickerInPlayerPrefs())
        {
            saveManagerScript.SetUIFlickerInPlayerPrefs(canFlicker);
        }
    }

    private void OnValueMuteAllToggle()
    {
        canFlicker = flickerToggle.isOn;
    }
}
