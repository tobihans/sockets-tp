using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace SocketListener
{
    /**
     * Utilitaire pour stocker les informations/évènements du serveur dans un fichier
     */
    public struct Logger
    {
        private const string LogSaveFileName = "SocketListenerLogs.txt"; 
        /**
         * Lancement du serveur
         */
        public static void LogServerStart()
        {
            using StreamWriter file = new(LogSaveFileName, append: true);
            file.WriteLine("-------------------------------------");
            file.WriteLine($"Started at : {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
        }
        
        /**
         * Informations sur la connexion
         */
        public static void LogServerInformation(IPAddress ip, int port)
        {
            using StreamWriter file = new(LogSaveFileName, append: true);
            file.WriteLine($"IP = {ip}, IP type = {ip.AddressFamily}, Port = {port}");
        }
        
        /**
         * Arrêt normal du serveur
         */
        public static void LogServerStop()
        {
            using StreamWriter file = new(LogSaveFileName, append: true);
            file.WriteLine($"Stopped at : {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
        }

        /**
         * Message reçu par le serveur
         */
        public static void LogReceivedData(string data)
        {
            using StreamWriter file = new(LogSaveFileName, append: true);
            file.WriteLine($"Message received : {data}");
        }
        
        /**
         * Exception dans notre programme
         */
        public static void LogServerException(Exception exceptionToLog)
        {
            using StreamWriter file = new(LogSaveFileName, append: true);
            file.WriteLine($"Exception : {exceptionToLog.Message}");
        }
    }
}