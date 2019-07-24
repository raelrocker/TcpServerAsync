using System;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
            SemParar semParar = new SemParar();
            Task.Run(() => semParar.IniciarServidor());
            //Task.Run(() => IniciarServidor("Servidor SEM PARAR", 100, 2000));
            //Task.Run(() => IniciarServidor("Servidor CÂMERA", 101, 0));
            Console.ReadKey();
        }

        static async Task IniciarServidor(string nome, int porta, int delay)
        {
            Server servidor = new Server(nome, porta, delay);
            servidor.IniciarServidor();
            Console.WriteLine($"teste - Thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}