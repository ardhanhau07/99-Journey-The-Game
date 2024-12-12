using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public override string ToString()
    {
        return $"({x}, {z})";
    }
}

public class MazeLogic : MonoBehaviour
{
    public Transform mazeManager;
    public int width = 30;
    public int depth = 30;
    public int scale = 6;
    public GameObject Enemy;
    public int EnemyCount = 1;
    public int RoomCount = 1;
    public int RoomMinSize = 6;
    public int RoomMaxSize = 10;
    public List<GameObject> Cube;
    public byte[,] map;
    GameObject[,] BuildingList;

    public virtual void Start()
    {
        InitializeMap();
        GenerateMaps();
        DrawMaps();
        PlaceEnemy();
        AddRooms(RoomCount, RoomMinSize, RoomMaxSize);

        if (mazeManager != null)
        {
            mazeManager.position = new Vector3(383.0772f, 24.35f, 686.24f);
        }
    }

    void Update() {
        mazeManager.position = transform.position;
        if (Input.GetMouseButton(0)) // Jika tombol mouse ditekan (untuk dragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = transform.position.y; // Pastikan Y tidak berubah
            transform.position = mousePos;
        }
    }

    void InitializeMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
            }
        }
    }

    public virtual void AddRooms(int count, int minSize, int maxSize)
    {
        for (int c = 0; c < count; c++)
        {
            int startX = Random.Range(3, width - 3);
            int startZ = Random.Range(3, depth - 3);
            int roomWidth = Random.Range(minSize, maxSize);
            int roomDepth = Random.Range(minSize, maxSize);

            for (int x = startX; x < width - 3 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < depth - 3 && z < startZ + roomDepth; z++)
                {
                    if (x >= 0 && z >= 0 && x < width && z < depth)
                    {
                        map[x, z] = 2;
                    }
                }
            }
        }
    }

    public virtual void GenerateMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                {
                    map[x, z] = 0;
                }
            }
        }
    }

    void DrawMaps()
    {
        Vector3 mazeManagerPosition = mazeManager.position;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    // Sesuaikan posisi berdasarkan skala plane
                    Vector3 position = new Vector3(
                        mazeManagerPosition.x + (x * scale),
                        mazeManagerPosition.y, // Tetap di posisi Y yang sama
                        mazeManagerPosition.z + (z * scale)
                    );

                    // Periksa apakah berada di dalam area plane
                    if (IsInsidePlane(position))
                    {
                        GameObject wall = Instantiate(Cube[Random.Range(0, Cube.Count)], position, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Fungsi untuk memastikan posisi berada dalam area plane
    // Ubah akses dari private menjadi public atau internal
    public bool IsInsidePlane(Vector3 position)
    {
        float planeXMin = mazeManager.position.x - (scale * width / 2);
        float planeXMax = mazeManager.position.x + (scale * width / 2);
        float planeZMin = mazeManager.position.z - (scale * depth / 2);
        float planeZMax = mazeManager.position.z + (scale * depth / 2);

        return position.x >= planeXMin && position.x <= planeXMax &&
               position.z >= planeZMin && position.z <= planeZMax;
    }


    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    public virtual void PlaceEnemy()
    {
        int EnemySet = 0;
        Vector3 mazeManagerPosition = mazeManager.position;

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);

                if (map[x, z] == 2 && EnemySet != EnemyCount)
                {
                    Vector3 spawnPosition = new Vector3(
                        mazeManagerPosition.x + (x * scale),
                        mazeManagerPosition.y,
                        mazeManagerPosition.z + (z * scale)
                    );

                    EnemySet++;
                    Instantiate(Enemy, spawnPosition, Quaternion.identity);
                }

                if (EnemySet == EnemyCount)
                {
                    return;
                }
            }
        }
    }
}
