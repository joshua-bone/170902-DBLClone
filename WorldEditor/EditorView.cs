using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorView : MonoBehaviour
{
    new public Camera camera; //new keyword hides Component.camera property
    public int deck = 0;
    public int edgeSizeInQuads;
    private int edgeSizeInVertices; //avoid fencepost errors
    private World world;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        edgeSizeInVertices = edgeSizeInQuads + 1;
        world = World.DefaultInstance();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector2 GetPositionOfCenterTile()
    {
        Vector3 playerPos = camera.transform.position;
        return new Vector2(
            (int)Mathf.Floor(playerPos.x),
            (int)Mathf.Floor(playerPos.y));
    }
}


//Individual MeshMaster objects
//Dictionary<Coordinates, Cell>