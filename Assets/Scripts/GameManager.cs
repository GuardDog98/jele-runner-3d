using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progress")]
    public int TotalCoins;
    public int CurrentSkinIndex;
    public bool[] OwnedSkins;

    [Header("Skins")]
    public Color[] SkinColors =
    {
        new Color(0.22f, 0.70f, 0.95f),
        new Color(0.96f, 0.72f, 0.22f),
        new Color(0.63f, 0.86f, 0.33f),
        new Color(0.90f, 0.35f, 0.55f),
        new Color(0.44f, 0.38f, 0.94f)
    };

    [Header("Shop")]
    public int[] SkinPrices = { 0, 15, 30, 50, 80 };

    public PlayerController Player { get; private set; }

    private const string SaveKey = "JeleRunnerSave";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        OwnedSkins = new[] { true, false, false, false, false };
        LoadProgress();
    }

    public void RegisterPlayer(PlayerController player)
    {
        Player = player;
        ApplyCurrentSkin();
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        SaveProgress();
    }

    public bool TryBuySkin(int index)
    {
        if (index < 0 || index >= SkinPrices.Length)
            return false;

        if (OwnedSkins[index])
        {
            CurrentSkinIndex = index;
            ApplyCurrentSkin();
            SaveProgress();
            return true;
        }

        if (TotalCoins >= SkinPrices[index])
        {
            TotalCoins -= SkinPrices[index];
            OwnedSkins[index] = true;
            CurrentSkinIndex = index;
            ApplyCurrentSkin();
            SaveProgress();
            return true;
        }

        return false;
    }

    public void ApplyCurrentSkin()
    {
        if (Player == null)
            return;

        if (CurrentSkinIndex < 0 || CurrentSkinIndex >= SkinColors.Length)
            CurrentSkinIndex = 0;

        Player.SetSkin(SkinColors[CurrentSkinIndex]);
    }

    public void SaveProgress()
    {
        var save = new SaveData
        {
            totalCoins = TotalCoins,
            currentSkinIndex = CurrentSkinIndex,
            ownedSkins = OwnedSkins
        };

        string json = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            CurrentSkinIndex = 0;
            return;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        var save = JsonUtility.FromJson<SaveData>(json);

        if (save == null)
            return;

        TotalCoins = save.totalCoins;
        CurrentSkinIndex = Mathf.Clamp(save.currentSkinIndex, 0, SkinColors.Length - 1);

        if (save.ownedSkins != null && save.ownedSkins.Length == OwnedSkins.Length)
            OwnedSkins = save.ownedSkins;
    }

    public void ResetProgress()
    {
        TotalCoins = 0;
        CurrentSkinIndex = 0;
        OwnedSkins = new[] { true, false, false, false, false };
        SaveProgress();
        ApplyCurrentSkin();
    }

    [System.Serializable]
    private class SaveData
    {
        public int totalCoins;
        public int currentSkinIndex;
        public bool[] ownedSkins;
    }
}
