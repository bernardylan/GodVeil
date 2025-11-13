using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] private List<ResourcesSO> resourceTypes;

    // List of all resources
    private Dictionary<ResourcesSO, float> resourceAmounts = new();

    public event Action<ResourcesSO, float> OnResourceChanged;

    private void Awake()
    {
        Instance = this;

        // Initialize all resources to 0 : CHANGE LATER TO LOAD RESOURCES IF SAVE IS PRESENT
        foreach (var res in resourceTypes)
            resourceAmounts[res] = 0;
    }

    // Used to add a resource when called
    public void Add(ResourcesSO resource, float amount)
    {
        if (!resourceAmounts.ContainsKey(resource))
            resourceAmounts[resource] = 0;

        resourceAmounts[resource] += amount;
        OnResourceChanged?.Invoke(resource, resourceAmounts[resource]);
    }

    // Used to spend resources for various needs
    public bool TrySpend(ResourcesSO resource, float amount)
    {
        if (!resourceAmounts.ContainsKey(resource) || resourceAmounts[resource] < amount) return false;

        resourceAmounts[resource] -= amount;
        OnResourceChanged?.Invoke(resource, resourceAmounts[resource]);
        return true;
    }

    // Check if there's enough resources of type
    public float GetAmount(ResourcesSO resource)
    {
        // Return the amount of said resource
        return resourceAmounts.TryGetValue(resource, out float value) ? value : 0f;
    }


    //================================================================
    // TEMPORARY FUNCTIONS FOR TEST PURPOSES, DELETE WHEN UNNECESSARY
    //================================================================

    public ResourcesSO GetResource(ResourceType type)
    {
        return resourceTypes.Find(r => r.type == type);
    }

    [ContextMenu("Add Gold")]
    public void AddGold()
    {
        Add(GetResource(ResourceType.Gold), 100);
    }

    [ContextMenu("Add Monster Essence")]
    public void AddMonsterEssence()
    {
        Add(GetResource(ResourceType.MonsterEssence), 100);
    }

    [ContextMenu("Add Divine Essence")]
    public void AddDivineEssence()
    {
        Add(GetResource(ResourceType.DivineEssence), 100);
    }

    [ContextMenu("Add God Crystal")]
    public void AddGodCrystal()
    {
        Add(GetResource(ResourceType.GodCrystal), 100);
    }
}
