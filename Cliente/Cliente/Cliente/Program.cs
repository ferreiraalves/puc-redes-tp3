using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class SimpleUdpClient
{
    public static void Main()
    {
        byte[] data = new byte[1024];
        string input, stringData;

        Console.WriteLine("*****              CLIENTE UDP               *****\n");
        Console.WriteLine("***** Digite algo para enviar para o Servidodr *****");
        Console.WriteLine("***** Para encerrar o Cliente digite exit     *****\n");

        // cria um IPEndPoint com o enderço do Servidor (no caso localhost)
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
        
        // cria o socket UDP para comunicação com o servidor
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // monta a mensagem para a ser enviada ao Servidor

        Console.WriteLine("Digite o nome do Usuário:\n");
        //string user = Console.ReadLine();
        string user = "lucas";
        data = Encoding.ASCII.GetBytes("USUARIO " + user);

        // Envia o dado para o Servidor. Para  enviar uma mensagem UDP, é necessário colocar o endereço do Servidor na mensagem (ipep)
        sock.SendTo(data, data.Length, SocketFlags.None, ipep);

        
        Console.WriteLine("endPoint do clente:                  {0} ", sock.LocalEndPoint.ToString());
        Console.WriteLine("endPoint do servidor:                {0} ", ipep.ToString());

        // cria um IPEndPoint com o endereço do cliente para  receber as mensagens  do servidor.
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remoto = (EndPoint)ep;
        Console.WriteLine("EndPoint Remoto antes da recepcao:   {0} ", Remoto.ToString());
       

        data = new byte[1024];

        // o método ReceiveFrom() irá colocar a informação do IPEndPoit do servidor no objeto Remoto (via referencia)
        // isto ocorre no momento que a mensagem chega.
        int recv = sock.ReceiveFrom(data, ref Remoto);
        Console.WriteLine("EndPoint Remoto depois da recepcao : {0}", Remoto.ToString());
        Console.WriteLine("Mensagem recebida de:                {0}", Remoto.ToString());
        Console.WriteLine("O Servidor ecoou:                    {0}", Encoding.ASCII.GetString(data, 0, recv));

        while (true)
        {
            input = Console.ReadLine();
            if (input == "exit")
            {
                Console.WriteLine("Eviando pedido de desligamento...");
                sock.SendTo(Encoding.ASCII.GetBytes(input), Remoto);
                break;
            }  
            sock.SendTo(Encoding.ASCII.GetBytes(input), Remoto);

            data = new byte[1024];
            recv = sock.ReceiveFrom(data, ref Remoto);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            //Console.WriteLine(stringData);
            Console.WriteLine("O Servidor respondeu:                    {0}", stringData, 0, recv);
        }
        Console.WriteLine("Stopping client");
        Console.ReadKey();
        sock.Close();
    }
}