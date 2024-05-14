using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;
using Rocworks.Mqtt;

public class MQTTMessages : MonoBehaviour
{
    public MqttClient MqttClient;
    public bool clearCircuit;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI messageText;

    private float messageTimeout = 5.0f;

    private List<string> messages = new List<string>();

    protected void Update()
    {
        messageTimeout -= Time.deltaTime;

        if (messages.Count > 0)
        {
            foreach (string msg in messages)
            {
                ProcessMessage(msg);
            }
            messages.Clear();
            messageTimeout = 3.0f;
        }

        if (messageTimeout <= 0 && ModuleManager.Instance.ModulesInScene.Count > 0 && clearCircuit)
        {
            ModuleManager.Instance.ClearCircuit();
        }
    }

    private void StoreMessage(string eventMsg)
    {
        messages.Add(eventMsg);
    }

    private void ProcessMessage(string msg)
    {
        Debug.Log("Processing: " + msg);
        string[] messages = msg.Split('p');
        ModuleManager.Instance.UpdateCiruit(messages);
    }

    public void OnMessageArrived(MqttMessage m)
    {
        Debug.Log("Received: " + m.GetString());
        StoreMessage(m.GetString());
    }
}
