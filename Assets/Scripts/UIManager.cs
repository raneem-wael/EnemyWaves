using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI fpsText;

    private float fpsTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= 0.5f)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = $"FPS: {fps}";
            fpsTimer = 0f;
        }
    }

    public void UpdateWave(int waveNumber) => waveText.text = $"Wave: {waveNumber}";
    public void UpdateEnemies(int aliveEnemies) => enemiesText.text = $"Enemies: {aliveEnemies}";
}
