using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/MapAmbience")]
public class MapAmbience : ScriptableObject
{
    [Header("Identity")]
    public string ambienceName;

    [Header("Background")]
    public Sprite backgroundSprite;

    [Header("Audio")]
    public AudioClip ambientSound;

    [Header("Light")]
    public Color ambientColor = Color.white;
    [Range(0f, 1f)] public float lightIntensity = 0.5f;
}
