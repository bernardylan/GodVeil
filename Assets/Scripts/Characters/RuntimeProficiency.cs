

[System.Serializable]
public class RuntimeProficiency
{
    public StatType type;
    public float proficiency;

    public RuntimeProficiency(StatType type, float value)
    {
        this.type = type;
        this.proficiency = value;
    }
}