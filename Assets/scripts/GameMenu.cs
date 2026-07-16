using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance;

    public TextMeshProUGUI higtScoreTxt;
    public TextMeshProUGUI lastScoreTxt;
    public TextMeshProUGUI Coins;

    public GameObject PainelConfig;
    public GameObject PainelShop;
    public GameObject PainelRecorde;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SaveManager.OnCoinsChanged += UpdateCoinsDisplay;
    }

    private void OnDisable()
    {
        SaveManager.OnCoinsChanged -= UpdateCoinsDisplay;
    }

    private void Start()
    {
        // Garante que o SaveManager existe antes de qualquer operação
        if (SaveManager.instance == null)
        {
            Debug.LogWarning("SaveManager não encontrado na cena! Criando um novo...");
            // Se o SaveManager for um Singleton que se cria sozinho,
            // você pode instanciá-lo aqui ou simplesmente aguardar
            return;
        }

        // Inicializa itens padrão se necessário
        if (SaveManager.instance.data.itemsUnlock.Count == 0)
        {
            SaveManager.instance.data.itemsUnlock.Add("item01");
            SaveManager.instance.data.equipedItem = "item01";
            SaveManager.instance.Save();
        }

        // Agora sim, atualiza a UI
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Verificação completa antes de acessar
        if (SaveManager.instance == null)
        {
            Debug.LogWarning("RefreshUI: SaveManager.instance é null");
            return;
        }

        if (SaveManager.instance.data == null)
        {
            Debug.LogWarning("RefreshUI: SaveManager.instance.data é null");
            return;
        }

        // Atualiza textos com verificações individuais
        if (Coins != null)
            Coins.text = SaveManager.instance.data.totalCoins.ToString();

        if (lastScoreTxt != null)
            lastScoreTxt.text = "Last Score: " + SaveManager.instance.data.lastScore;

        if (higtScoreTxt != null)
            higtScoreTxt.text = "Record: " + SaveManager.instance.data.highScore;
    }

    public void UpdateCoinsDisplay()
    {
        if (Coins == null || SaveManager.instance == null || SaveManager.instance.data == null)
            return;

        Coins.text = SaveManager.instance.data.totalCoins.ToString();
    }

    public void CarregarJogo()
    {
        SceneManager.LoadScene(1);
    }

    public void Config()
    {
        if (PainelConfig != null)
            PainelConfig.SetActive(true);
    }

    public void ExitConfig()
    {
        if (PainelConfig != null)
            PainelConfig.SetActive(false);
    }

    public void Shop()
    {
        if (PainelShop != null)
            PainelShop.SetActive(true);
    }

    public void ExitShop()
    {
        if (PainelShop != null)
            PainelShop.SetActive(false);
    }

    public void Record()
    {
        if (PainelRecorde != null)
            PainelRecorde.SetActive(true);
    }

    public void ExitRecord()
    {
        if (PainelRecorde != null)
            PainelRecorde.SetActive(false);
    }
}