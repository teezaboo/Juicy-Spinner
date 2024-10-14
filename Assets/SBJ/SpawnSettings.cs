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
    public float time; // เวลาที่จะเปลี่ยนค่า
    public float spawnRadius; // รัศมีการสปอน
    public float spawnFrequency; // ความถี่ในการสปอน
    public int numberOfMonsters; // จำนวนมอนสเตอร์ที่จะสปอน
}
