using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject CenterTerrain;
    Vector3 mousePosStart = Vector3.zero;
    Vector3 mousePosEnd = Vector3.zero;
    float previous_scrollDelta;
    private bool rotating;
    [Range(0,5)]
    public float SmoothRotationFactor;

    private void Start()
    {
        previous_scrollDelta = Input.mouseScrollDelta.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            mousePosStart = Input.mousePosition;
            rotating = true;

        }
        if (Input.GetMouseButton(2))
        {
            mousePosEnd = Input.mousePosition;
            transform.parent.transform.Translate(Vector3.right * Time.deltaTime * (mousePosStart.x - mousePosEnd.x) / 10);      
        }
        transform.parent.LookAt(CenterTerrain.transform);
        if (Input.GetMouseButtonUp(2))
        {
            mousePosEnd = Input.mousePosition;
            StartCoroutine(SlowDownRotation());
        }
        if (Input.mouseScrollDelta.y != previous_scrollDelta)
        {
            GetComponent<Camera>().fieldOfView -= Input.mouseScrollDelta.y;
            previous_scrollDelta -= Input.mouseScrollDelta.y;
        }

        if (!rotating)
        {
            if (Input.mousePosition.x < 10)
                transform.Translate(new Vector3(-0.1f, 0, 0));

            if (Input.mousePosition.x > Screen.width -10)
                transform.Translate(new Vector3(0.1f, 0, 0));

            if (Input.mousePosition.y < 10)
                transform.Translate(new Vector3(0, -0.1f, -0.1f));

            if (Input.mousePosition.y > Screen.height -10)
                transform.Translate(new Vector3(0, 0.1f, 0.1f));
        }
    }

    IEnumerator SlowDownRotation()
    {
        if ((mousePosStart.x - mousePosEnd.x) > 0)
        {
            while ((mousePosStart.x - mousePosEnd.x) > 0)
            {
                mousePosEnd.x += SmoothRotationFactor;
                transform.parent.transform.Translate(Vector3.right * Time.deltaTime * (mousePosStart.x - mousePosEnd.x) / 10);
                yield return 0;
            }
        }
        else
        {
            while ((mousePosStart.x - mousePosEnd.x) < 0)
            {
                mousePosEnd.x -= SmoothRotationFactor;
                transform.parent.transform.Translate(Vector3.right * Time.deltaTime * (mousePosStart.x - mousePosEnd.x) / 10);
                yield return 0;
            }
        }
        rotating = false;
    }
}
