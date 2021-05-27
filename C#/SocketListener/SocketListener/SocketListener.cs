using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static SocketListener.Logger;

namespace SocketListener
{
    /**
     * Serveur qui va écouter sur un port
     */
    class SocketListener
    {
        private const string ServerName = "localhost";
        /**
         * Port sur lequel notre serveur va écouter
         */
        private const int PortNumber = 5_000;
        /**
         * Chaîne indiquant qu'on doit arrêter d'écouter et stopper le serveur
         */
        private const string TextEof = "<EOF>";

        /**
         * Programme principal
         */
        static void Main(string[] args)
        {
            RunSocketListener();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /**
         * Lancement du serveur
         * On a besoin du nom du serveur qui sera résolu grâce au dns; localhost dans notre cas
         * Et du numéro du port sur lequel écouter
         */
        private static void StartServer(string serverName,int portNumber)
        {
            LogServerStart();
            var host = Dns.GetHostEntry(serverName);
            var ipAddress = host.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, portNumber);
            // Création de notre socket tcp
            var listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream,ProtocolType.Tcp);
            LogServerInformation(ipAddress,portNumber);
            // On lie notre socket à un endpoint, on écoute et on accepte la connexion entrante
            listener.Bind(localEndPoint);
            listener.Listen();
            Console.WriteLine("Waiting for a connection...");
            var handler = listener.Accept();
            Console.WriteLine("Connected");
            string data;
            var bytes = new byte[1024];

            // On continue à écouter et à envoyer tant qu'on a pas
            // reçu la chaine marquant la fin de notre communication
            do
            {
                // On recoit le message sérialisé en octets et on le reconstitue
                var bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                // Affichage du message reçu
                Console.WriteLine(data);
                LogReceivedData(data); // Dans notre fichier
                Console.Write(">");
                // On lit grâce à l'entrée standard le message à envoyer
                // Pour l'envoi, le message est décomposé en octets
                var toSend = Encoding.ASCII.GetBytes(Console.ReadLine() ?? string.Empty);
                handler.Send(toSend);
            } while (data.IndexOf(TextEof) <= -1);
            LogServerStop(); // Serveur arrêté
        }

        /**
         * On lance notre serveur
         */
        private static void RunSocketListener()
        {
            try
            {
                StartServer(ServerName,PortNumber);
            }
            catch (SocketException e)
            {
                LogServerException(e);
                Console.WriteLine("\n A problem happened. Go to see the server logs");
            }
        }
    }
}