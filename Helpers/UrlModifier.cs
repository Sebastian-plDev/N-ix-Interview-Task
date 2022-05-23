public class UrlModifier 
{
    public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
    public static readonly int Base = Alphabet.Length;
    // this could be obtianed from e.g configuration but I live it like it is for simplicity
    public static readonly string Prefix = "https://localhost:7102/";

    public static string Encode(int i)
    {
        if (i == 0) return Prefix + Alphabet[0].ToString();
        //for larger collections use StringBuilder, for now string is good enough
        var s = string.Empty;
        while (i > 0)
        {  
            s += Alphabet[i % Base];
            i = i / Base;
        }
        string result = Prefix;
        result += string.Join(string.Empty, s.Reverse());
        return result;
    }

    public static int Decode(string s)
    {
        var i = 0;
        foreach (var c in s)
        {
            i = (i * Base) + Alphabet.IndexOf(c);
        }
        return i;
    }
}