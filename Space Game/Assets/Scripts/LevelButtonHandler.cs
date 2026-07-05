/*
Notes:

*/
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    private GameObject gameController;
    private LevelUIPanelHandler LevelUIPanelScript;
    private TopLevelUIHandler TopLevelUIScript;
    private int levelToLoad = 0;

    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        LevelUIPanelScript = GameObject.FindGameObjectWithTag("LevelMenu").GetComponentInParent<LevelUIPanelHandler>();
        TopLevelUIScript = GameObject.FindGameObjectWithTag("CanvasUI").GetComponentInParent<TopLevelUIHandler>();
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        levelToLoad = int.Parse(this.gameObject.GetComponentInChildren<Text>().text);
        Debug.Log("Loaded Level: " + levelToLoad);
        gameController.GetComponent<GameManagerScript>().ResetToLoadSpecificLevel(levelToLoad);
        TopLevelUIScript.OpenInGamePanel();
        LevelUIPanelScript.EnableCloseMenuButton();
        TopLevelUIScript.CloseLevelPanel();
    }
}
