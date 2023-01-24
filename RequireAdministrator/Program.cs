// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Security.Principal;

CheckAdministrator();

Console.WriteLine("Hello, World!");

Console.Read();


/// <summary>
/// Check if already get administrator priviledge
/// </summary>
static void CheckAdministrator()
{
    var wi = WindowsIdentity.GetCurrent();
    var wp = new WindowsPrincipal(wi);

    bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

    if (!runAsAdmin)
    {
        var path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        // It is not possible to launch a ClickOnce app as administrator directly,
        // so instead we launch the app as administrator in a new process.
        var processInfo = new ProcessStartInfo(path);

        // The following properties run the new process as administrator
        processInfo.UseShellExecute = true;
        processInfo.Verb = "runas";

        // Start the new process
        try
        {
            Process.Start(processInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Shut down the current process
        Environment.Exit(0);
    }
}
