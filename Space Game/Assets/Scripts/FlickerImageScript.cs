using UnityEngine;
using UnityEngine.UI;

public class FlickerImageScript : MonoBehaviour
{
    Image thisImage;
    Outline thisOutline;
    float currOppacity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisImage = this.gameObject.GetComponent<Image>();
        thisOutline = this.gameObject.GetComponent<Outline>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currOppacity = GetColour(currOppacity);
        thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, currOppacity);
        thisOutline.effectColor = new Color(thisOutline.effectColor.r, thisOutline.effectColor.g, thisOutline.effectColor.b, currOppacity);
    }

    float GetColour(float oppacity)
    {
        if (oppacity == 1.0f)
        {
            return 0.65f;
        }
        else
        {
            return 1.0f;
        }
    }
}
