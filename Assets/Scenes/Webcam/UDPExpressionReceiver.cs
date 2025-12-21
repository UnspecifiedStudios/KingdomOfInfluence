using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;

public class UDPExpressionReceiver : MonoBehaviour {
    public int port = 5005;
    public TMP_Text textUI;

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool running = false;

    void Start() {
        if (UnityMainThreadDispatcher.Instance() == null) {
            Debug.LogError("Dispatcher missing! Add UnityMainThreadDispatcher to scene.");
            return;
        }

        udpClient = new UdpClient(port);
        running = true;

        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log("🎧 Listening for mocap data on port " + port);
    }

    void ReceiveLoop() {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);

        while (running) {
            try {
                byte[] data = udpClient.Receive(ref remoteEP);
                string json = Encoding.UTF8.GetString(data);

                ExpressionData parsed = JsonUtility.FromJson<ExpressionData>(json);

                if (parsed != null && !string.IsNullOrEmpty(parsed.expression)) {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        textUI.text = parsed.expression;
                    });
                }
            } catch (SocketException) { }
        }
    }

    void OnApplicationQuit() {
        running = false;
        udpClient.Close();
        if (receiveThread != null && receiveThread.IsAlive)
            receiveThread.Abort();
    }

    [Serializable]
    private class ExpressionData {
        public string expression;
    }
}
