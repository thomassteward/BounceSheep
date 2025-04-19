using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro
using System.Collections.Generic;

[System.Serializable]
public class WeightedPrefab
{
    public GameObject prefab;
    [Range(0f, 100f)]
    public float spawnChance;

    [HideInInspector] public float originalChance;
    [HideInInspector] public int cooldownSpawnsLeft = 0;
}

[System.Serializable]
public class SpawnedPrefabData
{
    public Sprite sprite;
    public float spawnChance;

    public SpawnedPrefabData(Sprite sprite, float spawnChance)
    {
        this.sprite = sprite;
        this.spawnChance = spawnChance;
    }
}

public class PrefabSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<WeightedPrefab> prefabs;
    public Transform spawnPoint;
    public float spawnCooldown = 2f;

    [Header("UI Settings")]
    public GameObject spawnedEntryPrefab; // Prefab with Image + TMP_Text
    public Transform spawnedListPanel;    // UI parent panel (e.g. VerticalLayoutGroup)

    [Header("SFX")]
    public AudioClip legendarySFX;
    public AudioClip mythicSFX;
    public AudioClip divineSFX;

    private AudioSource audioSource;
    private float lastSpawnTime = -Mathf.Infinity;

    [Header("Runtime")]
    public List<SpawnedPrefabData> spawnedPrefabs = new List<SpawnedPrefabData>();

    void Start()
    {
        foreach (var item in prefabs)
        {
            item.originalChance = item.spawnChance;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SpawnRandomPrefab()
    {
        if (Time.time < lastSpawnTime + spawnCooldown)
        {
            Debug.Log("Spawn on cooldown...");
            return;
        }

        float totalWeight = 0f;
        foreach (var item in prefabs)
        {
            float currentChance = (item.cooldownSpawnsLeft > 0) ? item.spawnChance * 0.5f : item.spawnChance;
            totalWeight += currentChance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var item in prefabs)
        {
            float currentChance = (item.cooldownSpawnsLeft > 0) ? item.spawnChance * 0.5f : item.spawnChance;
            cumulative += currentChance;

            if (randomValue <= cumulative)
            {
                // Spawn
                Vector2 offset = Random.insideUnitCircle;
                Vector3 spawnPos = spawnPoint.position + new Vector3(offset.x, offset.y, 0f);
                GameObject instance = Instantiate(item.prefab, spawnPos, Quaternion.identity);
                lastSpawnTime = Time.time;

                item.cooldownSpawnsLeft = 10;

                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    bool alreadySpawned = spawnedPrefabs.Exists(data => data.sprite == sr.sprite);

                    if (!alreadySpawned)
                    {
                        spawnedPrefabs.Add(new SpawnedPrefabData(sr.sprite, item.originalChance));
                        Debug.Log("New prefab added to record: " + sr.sprite.name + " | Chance: " + item.originalChance);

                        if (spawnedEntryPrefab != null && spawnedListPanel != null)
                        {
                            GameObject entry = Instantiate(spawnedEntryPrefab, spawnedListPanel);
                            Image img = entry.transform.Find("Image").GetComponent<Image>();
                            TMP_Text text = entry.transform.Find("Text").GetComponent<TMP_Text>();

                            if (img != null) img.sprite = sr.sprite;
                            if (text != null)
                            {
                                string rarity = GetColoredRarityLabel(item.originalChance);
                                text.text = rarity;
                            }

                            PlayRaritySFX(item.originalChance);
                        }
                    }
                }

                break;
            }
        }

        foreach (var item in prefabs)
        {
            if (item.cooldownSpawnsLeft > 0)
            {
                item.cooldownSpawnsLeft--;
            }
        }
    }

    private string GetColoredRarityLabel(float chance)
    {
        if (chance <= 0.25f)
            return "<color=#FFD700>Divine</color>";         // Gold
        else if (chance <= 1f)
            return "<color=#FF4500>Mythic</color>";         // Orange Red
        else if (chance <= 5f)
            return "<color=#FF00FF>Legendary</color>";      // Magenta
        else if (chance <= 20f)
            return "<color=#9400D3>Epic</color>";           // Dark Violet
        else if (chance <= 50f)
            return "<color=#4169E1>Rare</color>";           // Royal Blue
        else if (chance <= 80f)
            return "<color=#228B22>Uncommon</color>";       // Forest Green
        else
            return "<color=#808080>Common</color>";         // Gray
    }

    private void PlayRaritySFX(float chance)
    {
        if (chance <= 0.25f && divineSFX != null)
            audioSource.PlayOneShot(divineSFX);
        else if (chance <= 1f && mythicSFX != null)
            audioSource.PlayOneShot(mythicSFX);
        else if (chance <= 5f && legendarySFX != null)
            audioSource.PlayOneShot(legendarySFX);
    }
}
 