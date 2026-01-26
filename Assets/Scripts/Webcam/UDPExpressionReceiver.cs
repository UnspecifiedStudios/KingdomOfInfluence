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

    // The faces to display (matches enum in RandomFaceScript)
    public string[] playerFaces = {
        "(˶ᵔ ᵕ ᵔ˶)",
        "( O _ O ))",
        "(╥.╥)"
    };

    // Public field to be read by RandomFaceScript
    [HideInInspector]
    public int expression = -1;

    [Serializable]
    private class ExpressionData {
        public string expression;
    }

    void Awake() {
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

                if (parsed != null && int.TryParse(parsed.expression, out int exprIndex)) {
                    // Assuming UDP sends 1,2,3 → map to 0-based index
                    expression = exprIndex - 1;

                    // Update UI on main thread
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        if (expression >= 0 && expression < playerFaces.Length)
                            textUI.text = playerFaces[expression];
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
}
