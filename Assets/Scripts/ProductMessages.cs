public static class ProductMessages
{
    // Mat connection messages
    const string err_mat_connection_android_phone = "Bluetooth Connection lost.\nMake sure that your active Yipli Mat and device bluetooth are turned on.";
    const string err_mat_connection_android_tv = "Mat Connection lost.\nMake sure that your active Yipli Mat is connected to Device via USB cable.";
    const string err_mat_connection_pc = "Mat Connection lost.\nMake sure that your active Yipli Mat is connected to Device via USB cable.";
    const string err_mat_connection_android_phone_register = "Register the YIPLI fitness mat from Yipli Hub to continue playing.";
    const string err_mat_connection_mat_off = "Make sure that your active Yipli mat is turned on.";
    const string err_mat_connection_no_ports = "Required (Serial ports) Interface is not available in the system. Mat can't be connected.";
    const string err_mat_connection_retry = "Please Retry Mat connection.";
    const string err_mat_connection_no_driver = "Mat Driver setup is incomplete.\nYou need to install driver to play games.";

    // go to yipli cases
    public const string noMatCase = "No Mat Added";
    public const string noUserFound = "No User Found";
    public const string noPlayerAdded = "No Player Added";

    public const string openYipliApp = "Open Yipli App";

    public const string relaunchGame = "relaunchGame";

    // firebase deep links urls
    //const string addMatAppPageUrl = "https://yipliapp.page.link/?apn=com.yipli.app&link=https%3A%2F%2Fyipliapp.page.link.com%3Fhello%3Dsaurabh%26another_param%3Dwow%26route%3DmatListScreen";
    const string addMatAppPageUrl = "https://yipliapp.page.link/?apn=com.yipli.app&isi=1561746308&ibi=com.yipli.iosapp&link=https://yipliapp.page.link.com?route=matListScreen";
    const string userFoundAppPageUrl = "https://yipliapp.page.link/?apn=com.yipli.app&isi=1561746308&ibi=com.yipli.iosapp&link=https%3A%2F%2Fyipliapp.page.link.com%3Fhello%3Dsaurabh%26another_param%3Dwow%26route%3DplayerListScreen";
    const string addPlayerAppPageUrl = "https://yipliapp.page.link/?apn=com.yipli.app&isi=1561746308&ibi=com.yipli.iosapp&link=https%3A%2F%2Fyipliapp.page.link.com%3Fhello%3Dsaurabh%26another_param%3Dwow%26route%3DplayerListScreen";

    const string relaunchGameUrl = "https://yipliapp.page.link/?apn=com.yipli.app&isi=1561746308&ibi=com.yipli.iosapp&link=https%3A%2F%2Fyipliapp.page.link.com%3Froute%3DgamesListScreen%26gameName%3D";

    const string openYipliAppUrl = "https://yipliapp.page.link/gotopage";

    const string getMatUrlIn = "https://in.playyipli.com";

    const string getMatUrlUS = "https://www.playyipli.com";

    // troubleshooting notes
    const string startNote = "Please make sure that your Mat is on and charged.\nMat should show green light in the button side.";

    public static string Err_mat_connection_android_phone => err_mat_connection_android_phone;

    public static string Err_mat_connection_android_tv => err_mat_connection_android_tv;

    public static string Err_mat_connection_pc => err_mat_connection_pc;

    public static string Err_mat_connection_android_phone_register => err_mat_connection_android_phone_register;

    public static string Err_mat_connection_mat_off => err_mat_connection_mat_off;

    public static string Err_mat_connection_no_ports => err_mat_connection_no_ports;

    public static string Err_mat_connection_retry => err_mat_connection_retry;

    public static string Err_mat_connection_no_driver => err_mat_connection_no_driver;

    public static string AddMatAppPageUrl => addMatAppPageUrl;

    public static string UserFoundAppPageUrl => userFoundAppPageUrl;

    public static string AddPlayerAppPageUrl => addPlayerAppPageUrl;

    public static string RelaunchGameUrl => relaunchGameUrl;

    public static string OpenYipliAppUrl => openYipliAppUrl;

    public static string GetMatUrlIn => getMatUrlIn;

    public static string GetMatUrlUS => getMatUrlUS;

    public static string StartNote => startNote;
}
