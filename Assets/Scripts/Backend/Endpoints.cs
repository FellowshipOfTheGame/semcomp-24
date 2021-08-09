
public static class Endpoints
{
    //TODO: ler url base do api_settings.json
    private const string base_url = "http://ff0848782688.ngrok.io";
    
    public const string login_url = base_url + "/session/login";
    public const string get_session_url = base_url + "/session/login/get-session";
    public const string race_start = base_url + "/race/start";
    public const string race_finish = base_url + "/race/finish";
}