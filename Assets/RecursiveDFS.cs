using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDFS : MazeLogic
{
    public GameObject bossPrefab;  // Declare bossPrefab
    public override void Start()
    {
        base.Start(); // Call base Start to maintain the logic from MazeLogic
        AddBattleAreas(); // Custom logic for battle areas
    }

    // Menambahkan area bertarung dan bos
    private void AddBattleAreas()
    {
        // Area untuk bertarung dengan musuh biasa
        CreateOpenArea(5, 5, 5, 5); // Area pertama
        CreateOpenArea(12, 5, 4, 4); // Area kedua

        // Area untuk bertarung dengan bos
        CreateOpenArea(8, 15, 8, 4); // Area bos

        // Menghasilkan musuh di area bertarung
        PlaceEnemy();  // Call PlaceEnemy instead of SpawnEnemies

        // Menghasilkan bos di area bos
        SpawnBoss();
    }

    // Menempatkan musuh di lokasi yang sesuai
    public new void PlaceEnemy()
    {
        int EnemySet = 0;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x, z] == 2 && EnemySet < EnemyCount) // Jika ada ruang terbuka untuk musuh
                {
                    Debug.Log("Placing Enemy at (" + x + ", " + z + ")");
                    EnemySet++;
                    Instantiate(Enemy, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                }
                if (EnemySet >= EnemyCount) return;
            }
        }
    }

    // Menempatkan boss di area tertentu
    private void SpawnBoss()
    {
        // Spawn bos di titik yang lebih luas (area bertarung dengan bos)
        Vector3 bossPosition = new Vector3(8 * scale, 0, 15 * scale); // Lokasi bos
        Instantiate(bossPrefab, bossPosition, Quaternion.identity);
    }

    // Mengubah metode untuk menambah ruang bertarung
    private void CreateOpenArea(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < depth)
                {
                    map[x, y] = 2; // Buat jalan di area terbuka
                }
            }
        }
    }

    // Override untuk tempatkan karakter
    public override void PlaceCharacter()
    {
        // Menempatkan karakter di posisi awal
        int x = 1; // X posisi
        int z = 1; // Z posisi
        Character.transform.position = new Vector3(x * scale, 0, z * scale);
        Debug.Log("Placing character at (" + x + ", " + z + ")");
    }
}