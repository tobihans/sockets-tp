using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class SocketClient
    {
        // Nom du seveur qui sera utilisé pour le dns
        private const string ServerName = "localhost";
        // Port sur lequel nous allons écouter
        private const int PortNumber = 5_000;
        // Chaine qui marque la fin de communication chez notre serveur
        private const string TextEof = "<EOF>";

        // Méthode principale
        static void Main()
        {
            // Création de notre socket tcp
            var sc = ConnectSocket(serverName : ServerName, portNumber: PortNumber);
            // L'utilisateur doit appuyer sur entrée pour confirmer qu'il veut envoyer un message
            // Sinon, le programme envoie la chaine de fin de communication
            while (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.Write("-> ");
                var msgToSend = Console.ReadLine();
                Console.WriteLine($"Message : {msgToSend}");
                // Envoi du message
                SendMessage(sc,msgToSend);
            }
            SendMessage(sc,TextEof);
            sc.Close();
        }

        /**
         * Méthode pour créer un socket tcp
         * Lorsqu'aucune adresse ip n'est passée, on utilise
         * le dns pour retrouver l'adresse de notre serveur
         * Sinon, on se connecte directement à l'adresse
         */
        private static Socket ConnectSocket(string serverName,int portNumber,string ipAddr = null)
        {
            Socket socket = null;
            // Utilisation du dns
            var host = Dns.GetHostEntry(serverName);
            if (ipAddr == null)
            {
                foreach (var ipAddress in host.AddressList)
                {
                    var endPoint = new IPEndPoint(ipAddress, portNumber);
                    var tempSocket = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream,ProtocolType.Tcp);
                    tempSocket.Connect(endPoint);
                    // On parcourt les adresses obtenues grâce au dns jusqu'à avoir la connexion
                    if (tempSocket.Connected)
                    {
                        socket = tempSocket;
                        break;
                    }
                }
            }
            else // Adresse ip donnée en argument donc on utilise pas le dns
            {
                var ipAddress = IPAddress.Parse(ipAddr);
                var endPoint = new IPEndPoint(ipAddress, portNumber);
                var tempSocket = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream,ProtocolType.Tcp);
                tempSocket.Connect(endPoint);
                if (tempSocket.Connected)
                {
                    socket = tempSocket;
                }
            }
            return socket;
        }

        /**
         * Envoi du message
         */
        private static void SendMessage(Socket paramSocket, string message)
        {
            // Vérification de l'état des paramètres
            if ((paramSocket == null) || (message == null))
            {
                throw new Exception("Cannot send the message");
            }
            // Envoi du message proprement dit
            var sent = Encoding.ASCII.GetBytes(message);
            paramSocket.Send(sent,sent.Length,0);
            Console.WriteLine("Message sent");
            // On initialise la chaine qui va recevoir la réponse
            string data;
            var bytes = new byte[1024];
            // On écoute pour recevoir la réponse du serveur
            var bytesRec = paramSocket.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Console.WriteLine($">> {data} <<"); // Affichage de la réponse
        }
    }
}