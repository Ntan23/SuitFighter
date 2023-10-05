using UnityEngine;

[System.Serializable]
public class EnemyChoice
{
    public string choiceType;
    public string[] hints;
    [Range(0.0f, 1.0f)]
    public float chances;
}
