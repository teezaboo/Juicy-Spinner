using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettings", menuName = "ScriptableObjects/SpawnSettings", order = 1)]
public class SpawnSettings : ScriptableObject
{
    public List<SpawnTimeSettings> timeSettings; // ลิสต์ของค่าที่จะใช้ในแต่ละช่วงเวลา
}

[System.Serializable]
public class SpawnTimeSettings
{
    public float spawnFrequency;
    public int numberOfMonsters;
    public float RateFruitSpawn;
    public float RateBombSpawn;
    public float RateTrapSpawn;
    public float RateJokerSpawn;
}
