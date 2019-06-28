using UnityEngine;
using UnityEngine.UI;

public class MessageUnit : MonoBehaviour
{
    Text messageText;
    Image bkImage;
    MessagePanel messagePanel;

    public float lifeTime = 1;

    void Awake()
    {
        messageText = GetComponentInChildren<Text>();
        bkImage = GetComponentInChildren<Image>();
    }

    public void SetContent(MessagePanel mp, string content) {
        messagePanel = mp;
        messageText.text = content;
    }
    
    void Update()
    {
        if (lifeTime > 0) {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
                lifeTime = 0;
        }
        else{
            Color tempColor = bkImage.color;
            tempColor.a -= Time.deltaTime;
            bkImage.color = tempColor;

            tempColor = messageText.color;
            tempColor.a -= Time.deltaTime;
            messageText.color = tempColor;
        }

        if (messageText.color.a <= 0) {
            messagePanel.Remove(this);
            Destroy(gameObject);
        }
    }
}
