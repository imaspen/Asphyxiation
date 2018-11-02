using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject backgroundTile;

    public int x;
    public int y;

    void Start()
    {
        ClearBackground();
        GenerateBackground();
    }

    private void ClearBackground()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateBackground()
    {
        float baseX = x / 2;
        float baseY = y / 2 - 0.5f;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var newBrick = Instantiate(backgroundTile, gameObject.transform);
                newBrick.transform.position = new Vector3(baseX - i, baseY - j, 0);
            }
        }
    }
}
