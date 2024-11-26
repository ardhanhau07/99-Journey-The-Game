using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

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
    public int width = 30;
    public int depth = 30;
    public int scale = 6;
    public GameObject Enemy;
    public GameObject Character;
    public int EnemyCount = 3;
    public int RoomCount = 3;
    public int RoomMinSize = 6;
    public int RoomMaxSize = 10;
    public NavMeshSurface surface;
    public List<GameObject> Cube;
    public byte[,] map;
    GameObject[,] BuildingList;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMap();
        GenerateMaps();
        AddRooms(RoomCount, RoomMinSize, RoomMaxSize);
        DrawMaps();
        PlaceCharacter();
        PlaceEnemy();
        surface.BuildNavMesh();
    }

    void Update()
    {

    }

    void InitializeMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
                Debug.Log("Setting map[" + x + ", " + z + "] = " + map[x, z]);
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
                    if (x >= 0 && z >= 0 && x < width && z < depth) // Check boundaries
                    {
                        map[x, z] = 2;
                    }
                }
            }
        }
    }

    public virtual void PlaceCharacter()
    {
        for (int attempts = 0; attempts < 100; attempts++)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, depth);
            if (map[x, z] == 0)
            {
                Debug.Log("Placing character at (" + x + ", " + z + ")");
                Character.transform.position = new Vector3(x * scale, 0, z * scale);
                return; // Exit once placed
            }
        }
        Debug.LogWarning("Failed to place character after 100 attempts");
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
                Debug.Log("Setting map[" + x + ", " + z + "] = " + map[x, z]);
            }
        }
    }

    void DrawMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 position = new Vector3(x * scale, 0, z*scale);
                    GameObject wall = Instantiate(Cube[Random.Range(0, Cube.Count)], position, Quaternion.identity);
                    wall.transform.localScale= new Vector3(scale, scale, scale);
                }
            }
        }
    }


    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width -1 || z <= 0 || z>= depth -1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;

    }

    public virtual void PlaceEnemy()
    {
        int EnemySet = 0;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x,z] == 2 && EnemySet != EnemyCount)
                {
                    Debug.Log("placing Enemy");
                    EnemySet++;
                    Instantiate(Enemy, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                }
                else if (EnemySet == EnemyCount)
                {
                Debug.Log("already Placing All the Enemy");
                return;
                }
            }
        }
    }
}
    
