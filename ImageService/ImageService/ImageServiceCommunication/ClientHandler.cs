using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    class ClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string commandLine = reader.ReadLine();
                        Console.WriteLine("Got command: {0}", commandLine);

                        //string result = ExecuteCommand(commandLine, client);
                        //writer.Write(result);
                    }
                    client.Close();
                }).Start();

        }
    }
}
