using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [Range(1, 11)]
    public int partCount = 11;

    [Range(1, 6)]
    public int deathPartCount = 1;
}

[CreateAssetMenu(fileName ="New Stage")]
public class Stage : ScriptableObject
{
    public Color backGroundColor = Color.grey;
    public Color triangleColor = Color.grey;
    public Color ballColor = Color.grey;
    public List<Level> levels = new List<Level>();
}
