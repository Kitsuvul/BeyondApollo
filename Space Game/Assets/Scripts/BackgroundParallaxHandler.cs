using UnityEngine;

public class BackgroundParallaxHandler : MonoBehaviour
{
    private float startPosX, startPosY, length, height;
    private GameObject mainCamera;
    public float parallaxEffect;
    private HelperScript helperScript;

    private Vector2 ResetPosition = new Vector2(0.0f, 50.0f);
    private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 10.0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        helperScript = GameObject.FindGameObjectWithTag("PersistSettings").GetComponent<HelperScript>();
        mainCamera = Camera.main.gameObject;
        startPosX = this.gameObject.transform.position.x;
        startPosY = this.gameObject.transform.position.x;
        if(this.gameObject.GetComponent<SpriteRenderer>() != null )
        {
            length = this.gameObject.GetComponent<SpriteRenderer>().size.x * 5;
            height = this.gameObject.GetComponent<SpriteRenderer>().size.y * 5;
        }
        else
        {
            length = 100.0f;
            height = 100.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(helperScript.IsMainMenu())
        {
            RotateBackGround();
        }
        else
        {
            CalculateParallaxMovement();
        }
    }

    private void RotateBackGround()
    {
        if (this.gameObject.activeSelf)
        {
            if(this.gameObject.tag == "BackgroundStars")
            {
                this.transform.Rotate(new Vector3(0.0f, 10.0f, 0.0f) * Time.deltaTime);
            }
            else
            {
                this.transform.Rotate(new Vector3(0.0f, 0.0f, 10.0f) * Time.deltaTime);
            }

        }
    }

    private void CalculateParallaxMovement()
    {
        float distanceX = mainCamera.transform.position.x * parallaxEffect;
        float distanceY = mainCamera.transform.position.y * parallaxEffect;
        float movementX = mainCamera.transform.position.x * (1 - parallaxEffect);
        float movementY = mainCamera.transform.position.y * (1 - parallaxEffect);

        this.transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (movementX > startPosX + length)
        {
            startPosX += length;
        }
        else if (movementX < startPosX - length)
        {
            startPosX -= length;
        }

        if (movementY > startPosY + height)
        {
            startPosY += height;
        }
        else if (movementY < startPosY - height)
        {
            startPosY -= height;
        }
    }

    public void ResetOnLevelLoad()
    {
        this.gameObject.transform.position = ResetPosition;
    }
}
