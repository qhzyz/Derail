using System.Collections.Generic;
using UnityEngine;

public class MessagePanel : MonoBehaviour
{
    List<MessageUnit> messages;
    public GameObject pbMessageUnit;

    private void Start() {
        messages = new List<MessageUnit>();
    }

    public void SetMessage(string content)
    {
        GameObject tempGameObject = Instantiate(pbMessageUnit, transform);
        tempGameObject.GetComponent<MessageUnit>().SetContent(this, content);
        tempGameObject.transform.localPosition = GetLocalPos(messages.Count);
        messages.Add(tempGameObject.GetComponent<MessageUnit>());
    }

    public void Remove(MessageUnit mu) {
        if (messages.Contains(mu)) {
            messages.Remove(mu);
            UpdatePos();
        }
    }

    void UpdatePos() {
        for (int i = 0; i < messages.Count; i++) {
            messages[i].transform.localPosition = GetLocalPos(i);
        }
    }

    Vector3 GetLocalPos(int value) {
        Vector3 valuePos = Vector3.zero;
        for (int i = 0; i < value; i++) 
            valuePos -= new Vector3(0, 40, 0);
        return valuePos;
    }
}
