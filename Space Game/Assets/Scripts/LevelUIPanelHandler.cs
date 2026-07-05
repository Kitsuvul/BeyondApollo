/*
Notes:

*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUIPanelHandler : MonoBehaviour
{
    public List<GameObject> levelButtons;
    [SerializeField] public GameObject closeMenuButton;
    [SerializeField] public GameObject firstTimeTutorialText;
    public GameObject levelButtonPrefab;
    private GameObject gameControllerObj;
    private GameObject settingsObj;
    private GameObject buttonContainer;
    private GameObject buttonHolder;
    public bool loadedAllButtons = false;
    private Vector2 buttonPos = new Vector2(-750f, -4850f);
    private Vector2 buttonSize = new Vector2(350f, 350f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        buttonContainer = GameObject.FindGameObjectWithTag("ButtonHolder");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        settingsObj = GameObject.FindGameObjectWithTag("PersistSettings");
        buttonHolder = GameObject.FindGameObjectWithTag("ButtonHolder");

        if (closeMenuButton != null)
        {
            closeMenuButton.SetActive(false);
        }

        if (settingsObj && firstTimeTutorialText != null)
        {
            if (settingsObj.GetComponent<SaveManager>().GetHighestLevelInPlayerPrefs() == 0)
            {
                firstTimeTutorialText.GetComponent<Text>().text = "Click the first Red Button to start a level!";
                GenerateButton(0);
            }
            else
            {
                firstTimeTutorialText.GetComponent<Text>().text = "Levels";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // currentlevel has changed 

        if (this.gameObject.activeSelf == true && !loadedAllButtons && gameControllerObj != null)
        {
            foreach (GameObject obj in levelButtons)
            {
                Destroy(obj);
            }
            levelButtons.Clear();

            ResizeLevelPanel(gameControllerObj.GetComponent<LevelLoader>().TotalLevels, levelButtonPrefab.GetComponent<RectTransform>().sizeDelta.y);
            Vector2 pos = buttonHolder.gameObject.GetComponent<RectTransform>().sizeDelta;
            float startingY = -(pos.y / 2) + 250f;
            buttonPos = new Vector2(-750f, startingY); // -4850f
            GenerateButton(gameControllerObj.GetComponent<LevelLoader>().CurrentLevel);

            loadedAllButtons = true;
        }
    }

    void ResizeLevelPanel(int amountOfLevels, float height)
    {
        Debug.Log(amountOfLevels);
        float rows = (amountOfLevels / 4) + 1;
        float totalHeight = (height + 150f) * rows;

        buttonHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(1900f, totalHeight);
        buttonHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, totalHeight / 2);
    }

    private void GenerateButton(int level)
    {
        for (int i = 1; i <= level; ++i)
        {
            if (buttonContainer != null)
            {
                GameObject button = Instantiate(levelButtonPrefab, buttonContainer.transform);

                button.transform.SetParent(buttonContainer.transform, false);
                button.transform.localPosition = buttonPos;
                button.GetComponentInChildren<Text>().text = i.ToString();
                if ((i + 1) % 5 == 0)
                {
                    buttonPos.y += 450f;
                    buttonPos.x = -750f;
                }
                else
                {
                    buttonPos.x += 500f;
                }
                levelButtons.Add(button);
            }
        }
    }

    public void EnableCloseMenuButton()
    {
        closeMenuButton.SetActive(true);
    }
}
