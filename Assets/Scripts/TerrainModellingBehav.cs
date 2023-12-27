using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TerrainModellingBehav : MonoBehaviour
{
    public Terrain terrain;
    int hmWidth; 
    int hmHeight;
    public int size = 20;
    public float desiredHeight = 0.01f;
    public float modellingSpeed;
    Vector3 worldPosition;
    public GameObject target;
    Ray ray;
    private bool holding;
    public TerrainData StartingTerrainData;
    public Sprite spriteBuild;
    public Sprite spriteGrab;
    DragAndDrop draganddrop;
    bool canBuild = true;
    public CurrentHand hand;
    public List<Image> HandsImage;
    public List<Sprite> HandsSprite;
    public Text nOfVillagerstxt;
    public GameObject humanscontainer;
    public List<Texture2D> cursorTexture;
    public Earthquake earthquake;
    public Vector2 cursorSize = new Vector2(32, 32);

    public enum CurrentHand
    {
        DIGHAND,
        GRABHAND,
        BUILDHAND
    }
    private void Start()
    {
        hmWidth = terrain.terrainData.heightmapResolution;
        hmHeight = terrain.terrainData.heightmapResolution;     
        holding = false;
        draganddrop = GetComponent<DragAndDrop>();
        hand = CurrentHand.DIGHAND;
        Cursor.SetCursor(cursorTexture[0], new Vector2(cursorSize.x / 2, cursorSize.y / 2), CursorMode.Auto);

        for (int i = 0; i < HandsSprite.Count; i++)
        {
            HandsImage[i].sprite = HandsSprite[i];
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        nOfVillagerstxt.text = ": " + humanscontainer.transform.childCount.ToString();

        if (!canBuild)
            return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (terrain.GetComponent<TerrainCollider>().Raycast(ray, out hitData, 1000))
        {
            if (EventSystem.current.currentSelectedGameObject != null) return;

            worldPosition = hitData.point;
            target.transform.position = worldPosition;
            if (hand == CurrentHand.BUILDHAND)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    holding = true;
                    StartCoroutine(ChangeHeightTerrain(terrain, modellingSpeed));
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    holding = false;
                }
            }
            else if (hand == CurrentHand.DIGHAND)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    holding = true;
                    StartCoroutine(ChangeHeightTerrain(terrain, -modellingSpeed));
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    holding = false;
                }
            }

        }
    }
     IEnumerator ChangeHeightTerrain(Terrain terr,float speed)
    {
        float height = 0;
        int offset = size / 2;

        Vector3 posInTerrain = ConvertWordCor2TerrCor(worldPosition);
        float[,] heights = terrain.terrainData.GetHeights((int)posInTerrain.x, (int)posInTerrain.z, size, size);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                height = (heights[i, j]);

        while (holding)
        {
            Vector3 posInTerrain_2 = ConvertWordCor2TerrCor(worldPosition);
            if (posInTerrain_2.x > posInTerrain.x + 10 || posInTerrain_2.x < posInTerrain.x - 10 || posInTerrain_2.z > posInTerrain.z + 10 || posInTerrain_2.z < posInTerrain.z - 10)
                posInTerrain = ConvertWordCor2TerrCor(worldPosition);

            heights = terrain.terrainData.GetHeights((int)posInTerrain.x, (int)posInTerrain.z, size, size);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    heights[i, j] = height;

            height += speed;
            terrain.terrainData.SetHeights((int)posInTerrain.x - offset, (int)posInTerrain.z - offset, heights);
            yield return 0;
        }

    }

    public void ChangeHand(bool right)
    {
        #region Ui_side
        if (right)
        {
            hand += 1;
            Sprite tmp = HandsSprite[0];
            for (int i = 0; i < HandsSprite.Count -1; i++)
            {
                  HandsSprite[i] = HandsSprite[(i + 1)];        
            }
            HandsSprite[HandsSprite.Count-1] = tmp;

        }
        else
        {
            hand -= 1;
            Sprite tmp = HandsSprite[HandsSprite.Count - 1];
            for (int i = HandsSprite.Count -1; i > 0; i--)
            {
                HandsSprite[i] = HandsSprite[(i - 1)];
            }
            HandsSprite[0] = tmp;
        }   

        for (int i = 0; i < HandsSprite.Count; i++)
        {
            HandsImage[i].sprite = HandsSprite[i];
        }

        #endregion

        if ((int)hand > 2)
            hand = 0;

        if ((int)hand < 0)
            hand += 3;

            switch (hand)
            {
                case (CurrentHand.BUILDHAND):
                Cursor.SetCursor(cursorTexture[2], Vector2.zero, CursorMode.Auto);
                draganddrop.CanDrag = false;
                    break;
                case (CurrentHand.DIGHAND):
                Cursor.SetCursor(cursorTexture[0], Vector2.zero, CursorMode.Auto);
                draganddrop.CanDrag = false;
                break;
                case (CurrentHand.GRABHAND):
                Cursor.SetCursor(cursorTexture[1], Vector2.zero, CursorMode.Auto);
                draganddrop.CanDrag = true;
                break;
            }
    }
    public void ResetTerrain()
    {
        
        terrain.terrainData.SetHeights(0, 0, StartingTerrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution));

        earthquake.RestoreHouses();
        //td1.SetAlphamaps(0, 0, td2.GetAlphamaps(0, 0, td1.alphamapWidth, td1.alphamapHeight));
        //td1.treeInstances = td2.treeInstances;
        //td1.SetDetailLayer(0, 0, 0, td2.GetDetailLayer(0, 0, td1.detailWidth, td1.detailHeight, 0));
    }

     private Vector3 ConvertWordCor2TerrCor(Vector3 wordCor)
     {
         Vector3 vecRet = new Vector3();
         Terrain ter = Terrain.activeTerrain;
         Vector3 terPosition = ter.transform.position;
         vecRet.x = ((wordCor.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
         vecRet.z = ((wordCor.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
         return vecRet;
     }


}
