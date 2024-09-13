using System.Net;

namespace OrleansOnK8s.Voting.Helpers;

public static class ConfigurationExtensions
{
    public static DnsEndPoint ToDnsEndPoint(this string value, int defaultPort)
    {
        Console.WriteLine($"EndPoint value: {value}");
        var parts = value.Split(":");
        Console.WriteLine($"EndPoint Parts: {parts}");

        string host;
        var port = defaultPort;

        switch (parts.Length)
        {
            case 1:
                host = parts[0];
                break;
            case 2:
                host = parts[0];
                port = int.Parse(parts[1]);
                break;
            case 3:
                host = parts[1];
                port = int.Parse(parts[2]);
                break;
            default:
                throw new InvalidOperationException("Invalid endpoint value");
        }

        host = host.Trim('/');
        var endpoint = new DnsEndPoint(host, port);
        Console.WriteLine($"Redis Endpoint: {endpoint}");
        return endpoint;
    }
}

