using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class SimpleUdpSrvr
{
    public static void Main()
    {
        int recv;
        byte[] data = new byte[1024];

        Console.WriteLine("*****             SERVIDOR UDP          *****\n");
        Console.WriteLine("***** Ecoa os dados recebido do cliente *****\n");
        


        // cria o IpEndPoint do servidor
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5000);
        // cria  o socket
        Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        // vincula o endereço do servidor  ao socket
        newsock.Bind(ipep);

        Console.WriteLine("Aguardando um  cliente ...");

        // cria um objeto IPEndPoint vazio para guardar o IPEndPoint de um cliente que enviou uma mensagem
        IPEndPoint cli = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(cli);

        // Assim que uma mensagem chegar, o IPEndPoit da mensagem será copiada por referencia,  em Remote
        recv = newsock.ReceiveFrom(data, ref Remote);
        Console.WriteLine("Mensagem recebida de: {0}", Remote.ToString());
        data = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(data, 0, recv) + " OK");
        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);


        // monta a mensagem de eco
        //string welcome = "Pedido de conexao aceito. Aguardando requisicoes";
        //data = Encoding.ASCII.GetBytes(welcome);
        // toda a mensagem que  chega e ecoada
        //newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
        while (true)
        {
            data = new byte[1024];
            recv = newsock.ReceiveFrom(data, ref Remote);
            Console.WriteLine("Texto recebido:       {0}", Encoding.ASCII.GetString(data, 0, recv));
            if (Encoding.ASCII.GetString(data, 0, recv) == "exit")
            {
                Console.WriteLine("Desligando o servidor..");
                break;
            }
            string resposta = Console.ReadLine();
            data = Encoding.ASCII.GetBytes(resposta);
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
        }
        Console.ReadKey();
    }
}