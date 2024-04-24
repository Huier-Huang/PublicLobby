using PublicLobby_Server.Api.Servers;

namespace PublicLobby_Server.Api.Clients;

public class Client
{
    public static Client? Local;

    public static List<Client> _Clients = [];

    public List<Mod> _mods = [];

    public string Puid;

    public string FriendCode;
    
    public string Name;

    public HttpClient _Client = new();

    public string UrlPath;
    
    public void Init()
    {
        Local = this;
        _Clients.Add(this);
    }

    public void InitMod(IEnumerable<Mod> mods)
    {
        _mods = mods.ToList();
    }

    public void InitInfo(string puid, string friendCode, string name)
    {
        Puid = puid;
        FriendCode = friendCode;
        Name = name;
    }
}