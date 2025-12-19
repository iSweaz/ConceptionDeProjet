using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/// <summary>
/// Ensemble de méthodes lier au network.
/// </summary>
public static class C_NetworkUtils 
{
    /// <summary>
    /// Retourne l'adresse IPv4 locale de la machine.
    /// </summary>
    public static string GetLocalIP()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                continue;

            IPInterfaceProperties props = ni.GetIPProperties();

            foreach (UnicastIPAddressInformation ip in props.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.Address.ToString();
                }
            }
        }

        return "127.0.0.1";
    }

    /// <summary>
    /// Vérifie si un serveur UDP existe à l'adresse et au port spécifiés.
    /// </summary>
    public static bool IsServerRunning(string serverIP, int serverPort, int timeoutMs = 500)
    {
        try
        {
            UdpClient client = new UdpClient();
            client.Client.ReceiveTimeout = timeoutMs;

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            // Envoi d'un message “ping” simple
            byte[] message = Encoding.UTF8.GetBytes("PING");
            client.Send(message, message.Length, serverEP);

            // Attente de la réponse
            IPEndPoint remoteEP = null;
            byte[] response = client.Receive(ref remoteEP);

            string text = Encoding.UTF8.GetString(response);
            client.Close();

            // Si le serveur répond “PONG”, c'est qu'il est actif
            return text == "PONG";
        }
        catch
        {
            // Pas de réponse ou erreur → aucun serveur
            return false;
        }
    }
    
    private static UdpClient pingListener;
    private static Thread pingThread;

    public static void StartPingResponder(int pingPort = 5000)
    {
        pingListener = new UdpClient(pingPort);

        pingThread = new Thread(() =>
        {
            try
            {
                while(true)
                {
                    IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = pingListener.Receive(ref clientEP);
                    string msg = Encoding.UTF8.GetString(data);

                    if(msg == "PING")
                    {
                        byte[] reply = Encoding.UTF8.GetBytes("PONG");
                        pingListener.Send(reply, reply.Length, clientEP);
                    }
                }
            }
            catch(ThreadAbortException)
            {
                // Thread arrêté volontairement
            }
            catch
            {
                // gérer les erreurs si nécessaire
            }
        });

        pingThread.IsBackground = true;
        pingThread.Start();
    }

    public static void StopPingResponder()
    {
        if(pingThread != null && pingThread.IsAlive)
            pingThread.Abort();
        if(pingListener != null)
            pingListener.Close();
    }
}