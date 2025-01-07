namespace APM.Core;

public static class ErrorReporter
{
    public static void Show(this Exception error, string text = "")
    {
        _ = text != "" 
            ? WindowManager.ShowMessage("Error",text + Environment.NewLine + error.Message) 
            : WindowManager.ShowMessage("Error",error.Message);
    }
}