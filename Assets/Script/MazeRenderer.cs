using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject MazeCellPrefab;

    // Physical size of maze cells.
    public float CellSize = 1f;

    private void Start()
    {
        if (mazeGenerator == null || MazeCellPrefab == null)
        {
            Debug.LogError("MazeGenerator or MazeCellPrefab is not assigned!");
            return;
        }

        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(
                    MazeCellPrefab,
                    new Vector3(x * CellSize, 0f, y * CellSize),
                    Quaternion.identity,
                    transform
                );

                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                bool right = maze[x, y].rightWall;
                bool bottom = maze[x, y].bottomWall;

                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
}
