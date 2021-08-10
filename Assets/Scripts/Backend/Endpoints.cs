public static class Endpoints
{
    public static string Base_url { private get; set; }
    
    public static string Login_url => Base_url + "/session/login";
    public static string Login_get_session_url => Base_url + "/session/login/get-session";
    public static string Race_start_url => Base_url + "/race/start";
    public static string Race_finish_url => Base_url + "/race/finish";
}