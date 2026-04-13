/*
Notes:

*/
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    private LevelUIPanelHandler LevelUIPanelScript;
    private TopLevelUIHandler TopLevelUIScript;
    private int levelToLoad = 0;

    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelUIPanelScript = GameObject.FindGameObjectWithTag("LevelMenu").GetComponentInParent<LevelUIPanelHandler>();
        TopLevelUIScript = GameObject.FindGameObjectWithTag("CanvasUI").GetComponentInParent<TopLevelUIHandler>();
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        levelToLoad = int.Parse(this.gameObject.GetComponentInChildren<Text>().text);
        Debug.Log("Loaded Level: " + levelToLoad);
        TopLevelUIScript.ResetToLoadSpecificLevel(levelToLoad);
        TopLevelUIScript.OpenInGamePanel();
        LevelUIPanelScript.EnableCloseMenuButton();
        TopLevelUIScript.CloseLevelPanel();
    }
}
