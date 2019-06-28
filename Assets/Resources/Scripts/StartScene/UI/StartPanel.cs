using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button configButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private ConnectPanel connectingPanel;
    [SerializeField]
    private ConfigPanel configPanel;
    // Use this for initialization
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start () {
        startButton.onClick.AddListener(delegate () {
            configPanel.gameObject.SetActive(false);
            connectingPanel.gameObject.SetActive(true);
            connectingPanel.SetConnect();
            gameObject.SetActive(false);
        });
        configButton.onClick.AddListener(delegate () {
            bool tempBool = configPanel.gameObject.activeSelf;
            configPanel.gameObject.SetActive(!tempBool);
            GetComponent<AudioSource>().Play();
        });
        quitButton.onClick.AddListener(delegate () {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        });
    }

    // Update is called once per frame
    void Update () {
	}

}
