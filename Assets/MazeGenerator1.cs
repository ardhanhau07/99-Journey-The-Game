using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator1 : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject almariPrefab;
    public GameObject kursiPrefab;
    public Transform planeParent;
    public Vector3 planeScale = new Vector3(2.700886f, 1f, 1.927449f);
    public int mazeWidth = 10;
    public int mazeHeight = 10;
    public float cellSize = 6f;
    public int maxWalls = 120;
    public int maxAlmari = 10;
    public int maxKursi = 10;

    private bool[,] maze;

    void Start()
    {
        maze = new bool[mazeWidth, mazeHeight];
        GenerateMaze();
        BuildMaze();
    }

    void GenerateMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                maze[x, z] = false;
            }
        }

        for (int x = 1; x < mazeWidth; x += 2)
        {
            for (int z = 1; z < mazeHeight; z += 2)
            {
                maze[x, z] = true;

                List<Vector2Int> neighbors = new List<Vector2Int>();
                if (x > 1) neighbors.Add(new Vector2Int(x - 1, z));
                if (x < mazeWidth - 2) neighbors.Add(new Vector2Int(x + 1, z));
                if (z > 1) neighbors.Add(new Vector2Int(x, z - 1));
                if (z < mazeHeight - 2) neighbors.Add(new Vector2Int(x, z + 1));

                if (neighbors.Count > 0)
                {
                    Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                    maze[randomNeighbor.x, randomNeighbor.y] = true;
                }
            }
        }
    }

    void BuildMaze()
    {
        Vector3 origin = planeParent.position;
        int wallCount = 0;
        int almariCount = 0;
        int kursiCount = 0;

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                if (wallCount >= maxWalls) break;

                Vector3 cellPosition = origin + new Vector3(x * cellSize, 0, z * cellSize);

                if (!maze[x, z])
                {
                    // Spawn Wall
                    GameObject wall = Instantiate(wallPrefab, cellPosition, Quaternion.identity);
                    wall.transform.SetParent(planeParent);

                    if ((x + z) % 3 == 0)
                    {
                        wall.transform.Rotate(0, 90, 0);
                    }

                    wallCount++;
                }
                else
                {
                    // Spawn Almari or Kursi
                    if (almariCount < maxAlmari && Random.Range(0, 10) < 2)
                    {
                        GameObject almari = Instantiate(almariPrefab, cellPosition, Quaternion.identity);
                        almari.transform.SetParent(planeParent);
                        almariCount++;
                    }
                    else if (kursiCount < maxKursi && Random.Range(0, 10) < 2)
                    {
                        GameObject kursi = Instantiate(kursiPrefab, cellPosition, Quaternion.Euler(90, 0, 0));
                        kursi.transform.SetParent(planeParent);
                        kursiCount++;
                    }
                }
            }

            if (wallCount >= maxWalls) break;
        }
    }

    void OnDrawGizmos()
    {
        if (planeParent == null) return;

        Gizmos.color = Color.red;
        Vector3 origin = planeParent.position;

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                Vector3 cellPosition = origin + new Vector3(x * cellSize, 0, z * cellSize);
                Gizmos.DrawSphere(cellPosition, 0.2f);
            }
        }
    }
}
