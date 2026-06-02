using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FlickerTextScript : MonoBehaviour
{
    Text thisText;
    Outline thisOutline;
    RectTransform thisPosition;
    float currOppacity;
    bool moveToggle = false;
    float directionX, directionY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        thisText = this.GetComponent<Text>();
        thisOutline = this.GetComponent<Outline>();
        directionX = Random.Range(-2.0f, 2.0f);
        directionY = Random.Range(-2.0f, 2.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlickerColour();
        FlickerPosition();
    }

    void FlickerColour()
    {
        currOppacity = GetColour(currOppacity);
        thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, currOppacity);
        thisOutline.effectColor = new Color(thisOutline.effectColor.r, thisOutline.effectColor.g, thisOutline.effectColor.b, currOppacity);
    }

    void FlickerPosition()
    {
        thisPosition = this.GetComponent<RectTransform>();
        thisPosition.transform.position = new Vector3(thisPosition.transform.position.x + GetMovement(moveToggle, directionX), thisPosition.transform.position.y + GetMovement(moveToggle, directionY), thisPosition.transform.position.z);
        moveToggle = !moveToggle;
        if (!moveToggle)
        {
            directionX = Random.Range(-2.0f, 2.0f);
            directionY = Random.Range(-2.0f, 2.0f);
        }
    }

    float GetColour(float oppacity)
    {
        if(oppacity == 1.0f)
        {
            return 0.65f;
        }
        else
        {
            return 1.0f;
        }
    }

    float GetMovement(bool toggle, float dir)
    {
        float ret;
        if(!toggle)
        {
            ret = dir;
        }
        else
        {
            ret = -dir;
        }
        return ret;
    }
}
