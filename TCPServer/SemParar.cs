using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class SemParar
    {
        Server server = null;

        public SemParar()
        {
            server = new Server("Sem Parar", 100, 0);
        }

        public async Task IniciarServidor()
        {
            await server.IniciarServidor();
            Task.Run(() => SegundoPlano());
            PrimeiroPlano();
        }

        private async Task PrimeiroPlano()
        {
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(2000);
                server.Teste($"Execução Primeiro plano {i}");
            }
        }

        private async Task SegundoPlano()
        {
            for(int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(5000);
                server.Teste($"Execução Segundo plano {i}");
            }
        }

    

    }
}
