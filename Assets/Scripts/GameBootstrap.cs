using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private const int NumberOfCoins = 24;
    private readonly List<GameObject> spawnedCoins = new List<GameObject>();

    private GameObject player;
    private Vector3 playerStartPosition = new Vector3(0f, 0.9f, 0f);

    private void Start()
    {
        BuildScene();
        CreatePlayer();
        SpawnCoins();
    }

    private void Update()
    {
        UpdateCoinRespawn();
        if (Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }

    private void BuildScene()
    {
        var cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        var camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.11f, 0.17f, 0.28f);
        camera.nearClipPlane = 0.1f;
        camera.fieldOfView = 70f;

        cameraObject.transform.position = new Vector3(0f, 5.5f, -11f);
        cameraObject.transform.rotation = Quaternion.Euler(12f, 0f, 0f);

        var lightObject = new GameObject("Directional Light");
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        light.color = new Color(1f, 0.96f, 0.87f);
        lightObject.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(20f, 1f, 160f);
        ground.transform.position = new Vector3(0f, -0.5f, 65f);
        ground.GetComponent<Renderer>().material.color = new Color(0.11f, 0.45f, 0.18f);

        for (int i = 0; i < 12; i++)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = "TrackBlock_" + i;
            block.transform.localScale = new Vector3(3.5f, 0.8f, 6f);
            block.transform.position = new Vector3(0f, -0.1f, 10f + i * 12f);
            block.GetComponent<Renderer>().material.color = new Color(0.28f, 0.23f, 0.22f);
        }
    }

    private void CreatePlayer()
    {
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Jele";
        player.tag = "Player";
        player.transform.position = playerStartPosition;
        player.transform.localScale = new Vector3(1f, 1f, 1f);

        var controller = player.AddComponent<CharacterController>();
        controller.center = new Vector3(0f, 1f, 0f);
        controller.height = 2f;
        controller.radius = 0.5f;

        player.AddComponent<PlayerController>();

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterPlayer(player.GetComponent<PlayerController>());
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < NumberOfCoins; i++)
        {
            var coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Coin_" + i;
            coin.transform.localScale = new Vector3(0.6f, 0.08f, 0.6f);
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            var renderer = coin.GetComponent<Renderer>();
            renderer.material.color = new Color(1f, 0.8f, 0.2f);

            var collider = coin.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;

            coin.AddComponent<CoinPickup>();

            float xPos = Random.Range(-3f, 3f);
            float zPos = 16f + i * 9f;
            coin.transform.position = new Vector3(xPos, 1.2f, zPos);
            coin.tag = "Coin";
            spawnedCoins.Add(coin);
        }
    }

    private void UpdateCoinRespawn()
    {
        if (player == null)
            return;

        for (int i = 0; i < spawnedCoins.Count; i++)
        {
            var coin = spawnedCoins[i];
            if (coin == null)
                continue;

            if (coin.transform.position.z < player.transform.position.z - 12f)
            {
                coin.transform.position = new Vector3(Random.Range(-3f, 3f), 1.2f, player.transform.position.z + 35f + Random.Range(5f, 26f));
            }
        }
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 22;
        labelStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 220, 30), "Coins: " + GameManager.Instance.TotalCoins, labelStyle);
        GUI.Label(new Rect(20, 52, 260, 30), "Skin: " + GameManager.Instance.CurrentSkinIndex, labelStyle);

        int buttonX = 20;
        int buttonY = 90;
        int buttonW = 180;
        int buttonH = 42;

        for (int i = 0; i < GameManager.Instance.SkinColors.Length; i++)
        {
            string label = i == 0 ? "Starter" : "Buy " + GameManager.Instance.SkinPrices[i];
            if (GameManager.Instance.OwnedSkins[i])
                label = "Use Skin " + i;

            if (GUI.Button(new Rect(buttonX, buttonY + i * (buttonH + 10), buttonW, buttonH), label))
            {
                if (GameManager.Instance.OwnedSkins[i])
                {
                    GameManager.Instance.CurrentSkinIndex = i;
                    GameManager.Instance.ApplyCurrentSkin();
                    GameManager.Instance.SaveProgress();
                }
                else
                {
                    GameManager.Instance.TryBuySkin(i);
                }
            }
        }

        if (GUI.Button(new Rect(Screen.width - 210, 20, 180, 40), "Reset Save"))
        {
            GameManager.Instance.ResetProgress();
        }
    }
}
