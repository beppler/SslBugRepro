using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

namespace SslBugRepro
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MakeRequest("github.com", 443);
                MakeRequest("git.mps.com.br", 443);
                MakeRequest("cav.receita.fazenda.gov.br", 443);
                MakeRequest("api.integracao-bnmp.cnj.jus.br", 443);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private static void MakeRequest(string host, int port)
        {
            Console.WriteLine($"Connecting to {host}:{port}");
            using var tcpClient = new TcpClient();
            tcpClient.Connect(host, port);
            using var tcpStream = tcpClient.GetStream();
            using var stream = new SslStream(tcpStream, false);
            stream.AuthenticateAsClient(host, null, SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls, false);
            Console.WriteLine($"\tSSL Protocol: {stream.SslProtocol}");
            Console.WriteLine($"\tCipher Algorithm: {stream.CipherAlgorithm}");
            Console.WriteLine($"\tCipher Strength: {stream.CipherStrength}");
            Console.WriteLine($"\tHash Algorithm: {stream.HashAlgorithm}");
            Console.WriteLine($"\tHash Strength: {stream.HashStrength}");
            Console.WriteLine($"\tKey Exchange Algorithm: {stream.KeyExchangeAlgorithm}");
            Console.WriteLine($"\tKey Exchange Strength: {stream.KeyExchangeStrength}");
            Console.WriteLine($"\tCertificate:\n\t\t{string.Join("\n\t\t", stream.RemoteCertificate.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))}");
        }
    }
}
