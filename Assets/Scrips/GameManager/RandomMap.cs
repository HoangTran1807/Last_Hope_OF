using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapTile : MonoBehaviour
{
    [Header("Tilemap Layers")]
    public Tilemap WaterLayer;
    public Tilemap SandLayer;
    public Tilemap GrassLayer;
    public Tilemap RockLayer;
    public Tilemap DetailTileMap;

    [Header("Base Tiles")]
    public TileBase waterTile;
    public TileBase sandTile;
    public TileBase grassTile;
    public TileBase rockTile;

    [Header("Detail Tiles")]
    public Tile[] deadTrees;
    public Tile[] rocks;
    public Tile[] skeleton;
    public Tile[] crystals;
    public Tile[] stones;
    public Tile[] drybrush;

    [Header("Noise Settings")]
    public float scale = 20f;
    public int octaves = 2;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public int seed = 42;

    [Header("Chunk Settings")]
    public int chunkSize = 16;
    public int renderDistance = 2;
    public Transform player;

    private Vector2 offset;
    private Dictionary<Vector2Int, bool> generatedChunks = new Dictionary<Vector2Int, bool>();

    // Thresholds
    float sandLevel = 0.3f;
    float grassLevel = 0.4f;
    float rockLevel = 0.8f;

    private void Start()
    {
        InitSeed();
    }

    private void Update()
    {
        // Chỉ update map khi game đang thực sự chạy
        if (GameController.Instance != null &&
            GameController.Instance.CurrentState == GameState.Playing)
        {
            UpdateChunksAroundPlayer();
        }

        // Debug: nhấn R để reload map
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadMap();
        }
    }


    private void InitSeed()
    {
        Random.InitState(seed);
        offset.x = Random.Range(0f, 99999f);
        offset.y = Random.Range(0f, 99999f);
    }

    private void UpdateChunksAroundPlayer()
    {
        if (player == null) return;

        Vector2Int playerChunk = GetChunkCoord(player.position);

        for (int y = -renderDistance; y <= renderDistance; y++)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);

                if (!generatedChunks.ContainsKey(chunkCoord))
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

    /// <summary>
    /// Xóa toàn bộ map và vẽ lại với seed mới nếu được truyền
    /// </summary>
    public void ReloadMap(int? newSeed = null)
    {
        ClearAllChunks();

        if (newSeed.HasValue)
            seed = newSeed.Value;

        InitSeed();
        UpdateChunksAroundPlayer();
    }

    private void ClearAllChunks()
    {
        WaterLayer.ClearAllTiles();
        SandLayer.ClearAllTiles();
        GrassLayer.ClearAllTiles();
        RockLayer.ClearAllTiles();
        DetailTileMap.ClearAllTiles();

        generatedChunks.Clear();
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

                SetTileToLayer(sample, new Vector3Int(worldX, worldY, 0));

                Tile detail = GetDetailTile(sample);
                if (detail != null)
                    DetailTileMap.SetTile(new Vector3Int(worldX, worldY, 0), detail);
            }
        }
    }

    private void SetTileToLayer(float sample, Vector3Int pos)
    {
        WaterLayer.SetTile(pos, waterTile);

        if (sample >= sandLevel)
            SandLayer.SetTile(pos, sandTile);
        if (sample >= grassLevel)
            GrassLayer.SetTile(pos, grassTile);
        if (sample >= rockLevel)
            RockLayer.SetTile(pos, rockTile);
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

    private Tile GetDetailTile(float sample)
    {
        if (sample < sandLevel) return null;

        if (sample >= sandLevel && sample < grassLevel)
        {
            if (CanSetDetail(0.002f)) return GetRandomTile(rocks);
            if (CanSetDetail(0.001f)) return GetRandomTile(skeleton);
        }

        if (sample >= grassLevel && sample < rockLevel)
        {
            if (CanSetDetail(0.001f)) return GetRandomTile(deadTrees);
            if (CanSetDetail(0.001f)) return GetRandomTile(skeleton);
            if (CanSetDetail(0.0015f)) return GetRandomTile(rocks);
            if (CanSetDetail(0.0008f)) return GetRandomTile(drybrush);
        }

        if (sample >= rockLevel)
        {
            if (CanSetDetail(0.003f)) return GetRandomTile(rocks);
            if (CanSetDetail(0.0015f)) return GetRandomTile(crystals);
            if (CanSetDetail(0.001f)) return GetRandomTile(skeleton);
            if (CanSetDetail(0.001f)) return GetRandomTile(stones);
        }

        return null;
    }

    private bool CanSetDetail(float rate) => Random.value < rate;

    private Tile GetRandomTile(Tile[] tiles)
    {
        if (tiles == null || tiles.Length == 0) return null;
        return tiles[Random.Range(0, tiles.Length)];
    }
}
