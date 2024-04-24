namespace PublicLobby_Server.Api.Servers;

public sealed class GameCode : IEquatable<GameCode>
{ 
    private const string V2 = "QWXRTYLPESDFGHUJKZOCVBINMA";

    private static readonly int[] V2Map = Enumerable.Range(65, 26).Select(v => V2.IndexOf((char)v)).ToArray();

    private static string IntToGameNameV2(int input)
    {
        var a = input & 0x3FF;
        var b = (input >> 10) & 0xFFFFF;

        return new string(new[]
        {
            V2[a % 26],
            V2[a / 26],
            V2[b % 26],
            V2[b / 26 % 26],
            V2[b / (26 * 26) % 26],
            V2[b / (26 * 26 * 26) % 26],
        });
    }

    private static int GameNameToIntV2(string code)
    {
        var a = V2Map[code[0] - 65];
        var b = V2Map[code[1] - 65];
        var c = V2Map[code[2] - 65];
        var d = V2Map[code[3] - 65];
        var e = V2Map[code[4] - 65];
        var f = V2Map[code[5] - 65];

        var one = (a + (26 * b)) & 0x3FF;
        var two = c + (26 * (d + (26 * (e + (26 * f)))));

        return (int)(one | ((two << 10) & 0x3FFFFC00) | 0x80000000);
    }
    
    public GameCode(int value)
    {
        Value = value;
        Code = IntToGameNameV2(value);
    }

    public GameCode(string code)
    {
        Value = GameNameToIntV2(code);
        Code = code.ToUpperInvariant();
    }

    public string Code { get; }

    public int Value { get; }

    public static implicit operator string(GameCode code) => code.Code;

    public static implicit operator int(GameCode code) => code.Value;

    public static implicit operator GameCode(string code) => From(code);

    public static implicit operator GameCode(int value) => From(value);

    public static bool operator ==(GameCode left, GameCode right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GameCode left, GameCode right)
    {
        return left.Equals(right);
    }

    public static GameCode From(int value) => new(value);

    public static GameCode From(string value) => new(value);

    /// <inheritdoc />
#pragma warning disable CS8767
    public bool Equals(GameCode other)
    {
        return Code == other.Code && Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is GameCode other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Value);
    }

    public override string ToString()
    {
        return Code;
    }
}