using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public ResourceManager ResourceManager;
    public float currentTime = 0f;  // เวลาในเกม
    public Pool_SpawnMonster monsterPrefab;  // Prefab ของมอนสเตอร์
    public Pool_SpawnMonster bombPrefab;  // Prefab ของมอนสเตอร์
    public Pool_SpawnMonster JokerPrefab;  // Prefab ของมอนสเตอร์
    public Pool_SpawnMonster TrapPrefab;  // Prefab ของมอนสเตอร์
    public SpawnSettings spawnSettings;  // ScriptableObject ที่เก็บค่าต่างๆ    
    public int currentSettingIndex = 0;  // อินเด็กซ์ของการตั้งค่าปัจจุบัน

    // เพิ่มตัวแปรสำหรับการตั้งค่า
    private float spawnFrequency;
    private int numberOfMonsters;
    public Vector3 spawnCenter = new Vector3(0, 2.13f, 0); // ตำแหน่งของการสปอน
    public float spawnRadius = 5f;  // รัศมีของวงกลม

    public float RateFruitSpawn = 100f;
    public float RateBombSpawn = 0f;
    public float RateJokerSpawn = 0f;
    public float RateTrapSpawn = 0f;
    private int timeIndex = 0;

    private Dictionary<string, float> spawnRates;
    private float totalWeight;

    private void Start()
    {
        spawnRates = new Dictionary<string, float>()
        {
            {"Fruit", RateFruitSpawn},
            {"Bomb", RateBombSpawn},
            {"Joker", RateJokerSpawn},
            {"Trap", RateTrapSpawn}
        };
        if (spawnSettings != null && spawnSettings.timeSettings.Count > 0)
        {
            ApplySpawnSettings(spawnSettings.timeSettings[0]);  // เริ่มต้นด้วยค่าตั้งค่าแรก
        }
        StartCoroutine(SpawnMonsters());
    }

    public string GetRandomSpawn()
    {
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (KeyValuePair<string, float> pair in spawnRates)
        {
            cumulativeWeight += pair.Value;
            if (randomValue < cumulativeWeight)
            {
                return pair.Key;
            }
        }

        return null; // ในกรณีที่ไม่มีอัตราการสปอนใดๆ
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // ตรวจสอบและเปลี่ยนค่า SpawnSettings ตามเวลาในเกม
        if (currentSettingIndex < spawnSettings.timeSettings.Count &&
            currentTime >= timeIndex + 10)
        {
            ApplySpawnSettings(spawnSettings.timeSettings[currentSettingIndex]);
            currentSettingIndex++;
            ResourceManager.AddWave();
            timeIndex += 10;
        }
    }

    private void ApplySpawnSettings(SpawnTimeSettings settings)
    {
        // Apply new settings
        spawnFrequency = settings.spawnFrequency;
        numberOfMonsters = settings.numberOfMonsters;
        RateFruitSpawn = settings.RateFruitSpawn;
        RateBombSpawn = settings.RateBombSpawn;
        RateJokerSpawn = settings.RateJokerSpawn;
        spawnRates = new Dictionary<string, float>()
        {
            {"Fruit", RateFruitSpawn},
            {"Bomb", RateBombSpawn},
            {"Joker", RateJokerSpawn},
            {"Trap", RateTrapSpawn}
        };
        totalWeight = RateFruitSpawn + RateBombSpawn + RateJokerSpawn + RateTrapSpawn;
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            for (int i = 0; i < numberOfMonsters; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                string randomSpawn = GetRandomSpawn();
                if (randomSpawn == "Fruit")
                {
                    monsterPrefab.GetPool(spawnPosition);
                }else if (randomSpawn == "Bomb")
                {
                    bombPrefab.GetPool(spawnPosition);
                }else if (randomSpawn == "Joker")
                {
                    JokerPrefab.GetPool(spawnPosition);
                }else if (randomSpawn == "Trap")
                {
                    TrapPrefab.GetPool(spawnPosition);
                }

            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(0f, spawnRadius);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return new Vector3(x, y, 0) + spawnCenter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnCenter, spawnRadius);
    }
}
