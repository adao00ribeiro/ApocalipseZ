[System.Serializable]
public struct servidor
{
    public int id;
    public string name;
    public string ip;
    public string port;
}

[System.Serializable]
public class ListServer 
{
    public servidor[] servers;
}
