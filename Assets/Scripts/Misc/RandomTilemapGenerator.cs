using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    public GameObject tilemapPrefab;
    public TileBase[] tiles;

    //[Range(0, 1)]
    public float fillPercentage = 0.5f;

    void Start()
    {
        for (int i = 0; i < 20; i++) GenerateRandomTilemap(i * 20);
    }

    void GenerateRandomTilemap(float xPos)
    {
        Tilemap tilemap = Instantiate(tilemapPrefab, new Vector2(xPos, 0), Quaternion.identity).GetComponentInChildren<Tilemap>();

                //if (Random.value < fillPercentage)
        for (int i = 0, x = -10; i < 20; i++, x++)
        {
            for (int j = 0, y = -10; j < 20; j++, y++)
            {
                int randomTileIndex = Random.Range(0, tiles.Length);
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[randomTileIndex]);

                Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, GetRandAngle()));
                tilemap.SetTransformMatrix(new Vector3Int(x, y, 0), matrix);
            }
        }
    }

    float GetRandAngle()
    {
        int rand = Random.Range(0, 4);
        return rand == 1 ? 90 : rand == 2 ? 180 : rand == 3 ? 360 : 0; 
    }
}
