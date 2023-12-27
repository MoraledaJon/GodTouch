using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject selectedObject;
    Ray ray;
    Vector3 worldPosition;
    RaycastHit hitData;
    public Terrain terrain;
    public bool CanDrag = false;
    public Texture2D openhandtexture;
    public Texture2D closedhadntexture;
    public bool draggingTerrain = false;

    void Update()
    {
        if(!CanDrag)
            return;
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (terrain.GetComponent<TerrainCollider>().Raycast(ray, out hitData, 1000))
        {
            worldPosition = hitData.point;
            if(selectedObject != null)
            {
                selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            }
           
        }

        if (Physics.Raycast(ray, out hitData, 1000) && hitData.transform.tag == "dragable")
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedObject = hitData.transform.gameObject;
                if (selectedObject.GetComponent<NpcBehaviour>() != null) //if its an npc
                    selectedObject.GetComponent<NpcBehaviour>().isbeingDragged = true;
                Cursor.SetCursor(closedhadntexture, Vector2.zero, CursorMode.Auto);
            }
        }
        else if(Physics.Raycast(ray, out hitData, 1000) && hitData.transform.tag == "Terrain")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(CanDrag)
                draggingTerrain = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingTerrain = false;
            if (selectedObject != null)
            {
                Cursor.SetCursor(openhandtexture, Vector2.zero, CursorMode.Auto);
                if (selectedObject.GetComponent<NpcBehaviour>() != null) //if its an npc
                    selectedObject.GetComponent<NpcBehaviour>().isbeingDragged = false;
                selectedObject = null;
                
            }
        }



    }
}
