using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;
using System.Reflection;

public class MQTTMessages : M2MqttUnityClient
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI messageText;

    private float messageTimeout = 5.0f;

    private List<string> messages = new List<string>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        messageTimeout -= Time.deltaTime;

        if (messages.Count > 0)
        {
            foreach (string msg in messages)
            {
                ProcessMessage(msg);
            }
            messages.Clear();
            messageTimeout = 5.0f;
        }

        if (messageTimeout <= 0 && ModuleManager.Instance.ModulesInScene.Count > 0)
        {
            ModuleManager.Instance.ClearCircuit();
        }
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "LucaTest" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "BossThePro" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
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

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("Received: " + msg);
        StoreMessage(msg);
    }
}
