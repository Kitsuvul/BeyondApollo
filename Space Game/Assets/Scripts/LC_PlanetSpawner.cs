using UnityEngine;
using UnityEngine.UI;

public class LC_PlanetSpawner : MonoBehaviour
{
    public int iD;
    public bool clicked = false;
    public bool clickedLine = false;
    private LevelEditorManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    public void OnClick()
    {
        if (!clicked)
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            clicked = true;
            manager.CurrentButtonPressed = iD;
        }
    }

    public void OnClickLine()
    {
        if (!clickedLine)
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            clickedLine = true;
        }
    }
}
