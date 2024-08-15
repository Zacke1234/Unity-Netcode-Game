using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Chat : NetworkBehaviour
{
    
    [SerializeField] private InputReader inputreader;
    [SerializeField] private TextMeshProUGUI chat_text;
    [SerializeField] private TMP_InputField chat_inputfield;
    
    // Start is called before the first frame update
    private void Start()
    {
        //SubmitMessageRPC("Hello World");
        if (inputreader != null)
        {
            inputreader.SendEvent += OnSend;
        }
    }
    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
    }

    private void OnSend()
    {   
        FixedString128Bytes message = new("Hello");
        message = chat_inputfield.text;
        SubmitMessageRPC(message);
    }
    [Rpc(SendTo.Everyone)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        chat_text.text += "\n "+ message.ToString();
    }
}
