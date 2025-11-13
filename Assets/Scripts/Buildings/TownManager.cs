using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    public static TownManager Instance;
    private void Awake() => Instance = this;

    public List<BuildingUpgrade> builtBuildings = new List<BuildingUpgrade>();

    public bool IsBuilt(BuildingSO building)
    {
        return builtBuildings.Exists(b => b.data == building && b.IsBuilt);
    }
}
