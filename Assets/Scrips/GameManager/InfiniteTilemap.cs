using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class InfiniteTilemap : MonoBehaviour
{
    [Header("Map Settings")]
    public int chunkSize = 16;
    public float noiseScale = 0.05f;
    public int renderDistance = 3;

    [Header("Noise Settings")]
    public int octaves = 3;
    public float persistence = 0.5f; // độ giảm biên độ mỗi tầng
    public float lacunarity = 2f;    // độ tăng tần số mỗi tầng

    [Header("Player")]
    public Transform player;

    [Header("Tiles")]
    public TileBase grassTile;
    public TileBase sandTile;
    public TileBase waterTile;

    [Header("References")]
    public Tilemap tilemap;

    private Vector2Int currentPlayerChunk;
    private float seedX, seedY;

    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();

    void Start()
    {
        seedX = Random.Range(0f, 9999f);
        seedY = Random.Range(0f, 9999f);

        currentPlayerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        UpdateChunks();
    }

    void Update()
    {
        Vector2Int newPlayerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        if (newPlayerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = newPlayerChunk;
            UpdateChunks();
        }
    }

    void UpdateChunks()
    {
        HashSet<Vector2Int> activeChunks = new HashSet<Vector2Int>();

        for (int cx = -renderDistance; cx <= renderDistance; cx++)
        {
            for (int cy = -renderDistance; cy <= renderDistance; cy++)
            {
                Vector2Int chunkCoord = new Vector2Int(currentPlayerChunk.x + cx, currentPlayerChunk.y + cy);
                activeChunks.Add(chunkCoord);

                if (!generatedChunks.Contains(chunkCoord))
                {
                    GenerateChunk(chunkCoord.x, chunkCoord.y);
                    generatedChunks.Add(chunkCoord);
                }
            }
        }

        // Xóa chunk cũ nằm ngoài renderDistance
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var chunk in generatedChunks)
        {
            if (!activeChunks.Contains(chunk))
            {
                ClearChunk(chunk);
                toRemove.Add(chunk);
            }
        }
        foreach (var c in toRemove) generatedChunks.Remove(c);
    }

    void GenerateChunk(int cx, int cy)
    {
        Vector3Int startPos = new Vector3Int(cx * chunkSize, cy * chunkSize, 0);
        TileBase[] tiles = new TileBase[chunkSize * chunkSize];

        int i = 0;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                int worldX = cx * chunkSize + x;
                int worldY = cy * chunkSize + y;

                float sample = GetNoise(worldX, worldY);

                if (sample < 0.3f) tiles[i] = waterTile;
                else if (sample < 0.5f) tiles[i] = sandTile;
                else tiles[i] = grassTile;

                i++;
            }
        }

        tilemap.SetTilesBlock(new BoundsInt(startPos, new Vector3Int(chunkSize, chunkSize, 1)), tiles);
    }

    void ClearChunk(Vector2Int chunkCoord)
    {
        Vector3Int startPos = new Vector3Int(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0);
        tilemap.SetTilesBlock(
            new BoundsInt(startPos, new Vector3Int(chunkSize, chunkSize, 1)),
            new TileBase[chunkSize * chunkSize]
        );
    }

    float GetNoise(int worldX, int worldY)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0f;
        float totalAmplitude = 0f;

        for (int o = 0; o < octaves; o++)
        {
            float nx = (worldX + seedX) * noiseScale * frequency;
            float ny = (worldY + seedY) * noiseScale * frequency;

            noiseHeight += Mathf.PerlinNoise(nx, ny) * amplitude;
            totalAmplitude += amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return noiseHeight / totalAmplitude; // chuẩn hóa về 0-1
    }
}
