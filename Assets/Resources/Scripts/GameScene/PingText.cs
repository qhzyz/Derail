using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PingText : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Ping: "+PhotonNetwork.GetPing();
    }
}
