using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Pool_SpawnMonster monsterPrefab;  // Prefab ของมอนสเตอร์
    public SpawnSettings spawnSettings;  // ScriptableObject ที่เก็บค่าต่างๆ
    private float currentTime = 0f;  // เวลาในเกม
    private int currentSettingIndex = 0;  // อินเด็กซ์ของการตั้งค่าปัจจุบัน

    // เพิ่มตัวแปรสำหรับการตั้งค่า
    private float spawnFrequency;
    private int numberOfMonsters;
    public Vector3 spawnCenter = new Vector3(0, 2.13f, 0); // ตำแหน่งของการสปอน
    public float spawnRadius = 5f;  // รัศมีของวงกลม

    private void Start()
    {
        if (spawnSettings != null && spawnSettings.timeSettings.Count > 0)
        {
            ApplySpawnSettings(spawnSettings.timeSettings[0]);  // เริ่มต้นด้วยค่าตั้งค่าแรก
        }
        StartCoroutine(SpawnMonsters());
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // ตรวจสอบและเปลี่ยนค่า SpawnSettings ตามเวลาในเกม
        if (currentSettingIndex < spawnSettings.timeSettings.Count &&
            currentTime >= spawnSettings.timeSettings[currentSettingIndex].time)
        {
            ApplySpawnSettings(spawnSettings.timeSettings[currentSettingIndex]);
            currentSettingIndex++;
        }
    }

    private void ApplySpawnSettings(SpawnTimeSettings settings)
    {
        // Apply new settings
        spawnFrequency = settings.spawnFrequency;
        numberOfMonsters = settings.numberOfMonsters;
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            for (int i = 0; i < numberOfMonsters; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                monsterPrefab.GetPool(spawnPosition, new Vector3(0, 0, Random.Range(0, 360)));
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
