public static class Endpoints
{
    public static string Base_url { private get; set; }
    
    public static string Login_url => Base_url + "/session/login";
    public static string Logout_url => Base_url + "/session/logout";
    public static string Login_get_session_url => Base_url + "/session/login/get-session";
    public static string Session_validate_url => Base_url + "/session/validate";
    public static string Race_start_url => Base_url + "/race/start";
    public static string Race_finish_url => Base_url + "/race/finish";
    public static string Ranking_url => Base_url + "/race/ranking";
    public static string Shop_url => Base_url + "/shop";
    public static string Shop_buy_url => Base_url + "/shop/buy-upgrades";
    public static string User_status_url => Base_url + "/user/status";
}