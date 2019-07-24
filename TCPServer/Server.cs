using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Server
    {
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private List<Socket> clientsSockets = new List<Socket>();
        private byte[] buffer = new byte[1024];

        public int Porta { get; set; }
        public string Nome { get; set; }

        public Server(string nome, int porta, int delay)
        {
            Porta = porta;
            Nome = nome;
            if (delay > 0)
                Thread.Sleep(delay);
        }

        public async Task IniciarServidor()
        {
            Console.WriteLine($"Iniciando servidor {Nome} na porta {Porta} - Thread {Thread.CurrentThread.ManagedThreadId}");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, Porta));
            serverSocket.Listen(5);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCB), null);
        }

        private void AcceptCB(IAsyncResult AR)
        {
            Socket socket = serverSocket.EndAccept(AR);
            clientsSockets.Add(socket);
            Console.WriteLine($"Servidor {Nome}: cliente conectado");
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCB), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCB), null);
        }

        private void ReceiveCB(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuffer = new byte[received];
            Array.Copy(buffer, dataBuffer, received);
            string text = Encoding.ASCII.GetString(dataBuffer);
            Console.WriteLine($"Servidor {Nome} recebeu: {text}");
            string response = string.Empty;
            if (text.ToLower().Equals("get time"))
            {
                response = DateTime.Now.ToLongDateString();
            }
            else
            {
                response = "Request inválido";
            }

            SendText(socket, response);
        }

        public void Teste(string texto)
        {
            Console.WriteLine($"{texto} - Thread {Thread.CurrentThread.ManagedThreadId}");
        }

        private void SendText(Socket socket, string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCB), socket);
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCB), socket);
        }

        private void SendCB(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}
