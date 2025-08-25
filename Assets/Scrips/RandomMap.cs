using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapTile : MonoBehaviour
{
    [Header("Debug / Test")]
    public bool forceReRender = false; // bật để vẽ lại chunk cũ
    public bool isRender = false;       // bật/tắt render thủ công

    [Header("Tilemap Settings")]
    public Tilemap tilemap;
    public Tilemap detailTileMap;
    public TileBase waterTile;
    public Tile sandTile;
    public Tile grassTile;
    public Tile rockTile;

    [Header("Tilemap Detail")]
    public Tile[] deadTrees;
    public Tile[] rocks;
    public Tile[] skeleton;
    public Tile[] crystals;
    public Tile[] stones;
    public Tile[] drybrust;

    [Header("Noise Settings")]
    public float scale = 20f;
    public int octaves = 2;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public int seed = 42;

    [Header("Chunk Settings")]
    public int chunkSize = 16;          // kích thước 1 chunk (16x16)
    public int renderDistance = 2;      // khoảng cách (tính theo số chunk)
    public Transform player;            // tham chiếu tới người chơi


    private Vector2 offset;
    private Dictionary<Vector2Int, bool> generatedChunks = new Dictionary<Vector2Int, bool>();

    float sandLevel = 0.3f;
    float grassLevel = 0.4f;
    float rockLevel = 0.8f;

    private void Start()
    {
        Random.InitState(seed);
        offset.x = Random.Range(0f, 99999f);
        offset.y = Random.Range(0f, 99999f);
    }

    private void Update()
    {
        if (isRender)
        {
            UpdateChunksAroundPlayer();
        }

        // Test trigger: bấm R để vẽ lại toàn bộ chunk trong render distance
        if (Input.GetKeyDown(KeyCode.R))
        {
            forceReRender = true;
            UpdateChunksAroundPlayer();
            forceReRender = false;
        }
    }


    private void UpdateChunksAroundPlayer()
    {
        Vector2Int playerChunk = GetChunkCoord(player.position);

        for (int y = -renderDistance; y <= renderDistance; y++)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);

                if (forceReRender || !generatedChunks.ContainsKey(chunkCoord))
                {
                    GenerateChunk(chunkCoord);
                    generatedChunks[chunkCoord] = true;
                }
            }
        }
    }


    private Vector2Int GetChunkCoord(Vector3 pos)
    {
        int cx = Mathf.FloorToInt(pos.x / chunkSize);
        int cy = Mathf.FloorToInt(pos.y / chunkSize);
        return new Vector2Int(cx, cy);
    }

    private void GenerateChunk(Vector2Int chunkCoord)
    {
        int startX = chunkCoord.x * chunkSize;
        int startY = chunkCoord.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int worldX = startX + x;
                int worldY = startY + y;

                float xCoord = (float)worldX / chunkSize * scale + offset.x;
                float yCoord = (float)worldY / chunkSize * scale + offset.y;

                float sample = OctavePerlin(xCoord, yCoord);
                TileBase tile = GetTile(sample);
                Tile detail = getDetailTile(sample);

                if (detail != null)
                {
                    detailTileMap.SetTile(new Vector3Int(worldX, worldY, 0), detail);
                }

                tilemap.SetTile(new Vector3Int(worldX, worldY, 0), tile);
            }
        }
    }

    private float OctavePerlin(float x, float y)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            float noiseValue = Mathf.PerlinNoise(x * frequency, y * frequency) * 2 - 1;
            total += noiseValue * amplitude;

            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return (total / maxValue + 1) / 2f;
    }

    private TileBase GetTile(float sample)
    {
        if (sample < sandLevel)
        {
            return waterTile;
        }
        else if (sample < grassLevel)
        {
            return sandTile;
        }
        else if (sample < rockLevel)
        {
            return grassTile;
        }
        else
        {
            return rockTile;
        }
    }

    private Tile getDetailTile(float sample)
    {
        if (sample < sandLevel) return null;

        if (sample >= sandLevel && sample < grassLevel)
        {
            if (CanSetDitail(0.002f)) return GetRandomTile(rocks);
            if (CanSetDitail(0.001f)) return GetRandomTile(skeleton);
        }

        if (sample >= grassLevel && sample < rockLevel)
        {
            if (CanSetDitail(0.001f)) return GetRandomTile(deadTrees);
            if (CanSetDitail(0.001f)) return GetRandomTile(skeleton);
            if (CanSetDitail(0.0015f)) return GetRandomTile(rocks);
            if (CanSetDitail(0.0008f)) return GetRandomTile(drybrust);
        }

        if (sample >= rockLevel)
        {
            if (CanSetDitail(0.003f)) return GetRandomTile(rocks);
            if (CanSetDitail(0.0015f)) return GetRandomTile(crystals);
            if (CanSetDitail(0.001f)) return GetRandomTile(skeleton);
            if (CanSetDitail(0.001f)) return GetRandomTile(stones);
        }

        return null;
    }

    private bool CanSetDitail(float rate)
    {
        return Random.value < rate;
    }

    private Tile GetRandomTile(Tile[] tiles)
    {
        if (tiles == null || tiles.Length == 0) return null;
        int index = Random.Range(0, tiles.Length);
        return tiles[index];
    }
}
