using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.IO;

public class UDPExpressionReceiver : MonoBehaviour {
    public int port = 5005;
    public TMP_Text textUI;

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool running = false;
    void Awake() {
        udpClient = new UdpClient(port);
        running = true;

        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        UnityEngine.Debug.Log("🎧 Listening for mocap data on port " + port);
    }
    void Start() {

        string script = Path.Combine(
            Application.dataPath,
            "Scenes",
            "Webcam",
            "FaceTracking.py"
        );

        UnityEngine.Debug.Log("Python script path: " + script);

        if (!File.Exists(script)) {
            UnityEngine.Debug.LogError("SCRIPT NOT FOUND: " + script);
            return;
        }

        ProcessStartInfo psi = new ProcessStartInfo {
            FileName = "py",
            Arguments = $"-3.11 \"{script}\"",
            UseShellExecute = false,
            CreateNoWindow = false
        };

        try {
            Process p = Process.Start(psi);

            UnityEngine.Debug.Log("🐍 Python process started (PID: " + p.Id + ")");
        } catch (Exception e) {
            UnityEngine.Debug.LogError("Failed to start FaceTracking.py:\n" + e);
        }
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
