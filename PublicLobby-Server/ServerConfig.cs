namespace PublicLobby_Server;

public class ServerConfig
{
    public const string Section = "Server";

    public string ListenIp { get; set; } = "127.0.0.1";
    
    public int ListenPort { get; set; } = 20000;
}