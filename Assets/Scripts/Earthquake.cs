using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private List<GameObject> House;
    [SerializeField]
    private GameObject DestroyHouse;
    public GameObject NotdestroyedHouse;

    public float moveSpeed = 0.5f;
    private float time;
    public DragAndDrop dragAndDrop;
    bool isDestroyed = false;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        time = 0.0f;
    }

    // Update is called once per frame
    public void RestoreHouses()
    {
        for (int i = 0; i < House.Count; i++)
        {
            GameObject newhouse = Instantiate(NotdestroyedHouse, new Vector3(House[i].transform.position.x,
                                           House[i].transform.position.y,
                                           House[i].transform.position.z),
                                           House[i].transform.rotation);
            Destroy(House[i]);
            House[i] = newhouse;
        }
        isDestroyed = false;
    }

    void Update()
    {
        if (dragAndDrop.draggingTerrain)
        {
            time += Time.deltaTime;
            float x = Input.GetAxis("Mouse X") * moveSpeed;
            cam.transform.localPosition -= new Vector3(x, 0, 0);
            if (time > 3.0f && !isDestroyed)
            {
                for (int i = 0; i < House.Count; i++)
                {
                    GameObject newhouse = Instantiate(DestroyHouse, new Vector3(House[i].transform.position.x,
                                                   House[i].transform.position.y,
                                                   House[i].transform.position.z),
                                                   House[i].transform.rotation);
                    Destroy(House[i]);
                    House[i] = newhouse;
                }
                isDestroyed = true;
            }
        }
        else
        {
            time = 0;
        }
    }
}
