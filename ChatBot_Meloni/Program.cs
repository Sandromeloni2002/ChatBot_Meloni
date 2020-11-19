using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatBot_Meloni
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddr = IPAddress.Any;

            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            listenerSocket.Bind(ipep);

            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Socket client = listenerSocket.Accept();

            Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());

            byte[] buff = new byte[128];
            int receivedBytes = 0;
            int sendedBytes = 0;
            string receivedString, sendString;

            int ora = DateTime.Now.Hour;

            while (true)
            {
                receivedBytes = client.Receive(buff);
                Console.WriteLine("Numero di byte ricevuti: " + receivedBytes);
                receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
                Console.WriteLine("Stringa ricevuta: " + receivedString);

                receivedString = receivedString.ToUpper().Trim();

                if (receivedString != "\r\n")
                {
                    if (receivedString == "QUIT")
                    {
                        break;
                    }
                    else if (receivedString == "HELP")
                    {
                        sendString = "Stringhe accettate dal server:\r\nCiao, Come stai?, Che fai?\r\nQuit - chiude il server";
                    }
                    else if (receivedString == "CIAO")
                    {
                        if (ora >= 5 && ora <= 15) sendString = "Buongiorno";
                        else sendString = "Buonasera";
                    }
                    else if (receivedString == "COME STAI?")
                    {
                        sendString = "Bene";
                    }
                    else if (receivedString == "CHE FAI?")
                    {
                        sendString = "Niente";
                    }
                    else sendString = "Non ho capito..";

                    sendString += "\r\n";

                    Array.Clear(buff, 0, buff.Length);
                    sendedBytes = 0;

                    buff = Encoding.ASCII.GetBytes(sendString);

                    sendedBytes = client.Send(buff);

                    Array.Clear(buff, 0, buff.Length);
                }
            }
        }
    }
}
