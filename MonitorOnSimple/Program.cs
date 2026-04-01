// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

Console.WriteLine("Encendiendo monitor 1...");

try
{
    Process.Start(new ProcessStartInfo
    {
        FileName = "uvx",
        Arguments = "monitorcontrol --set-power-mode on --monitor 1",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    })?.WaitForExit();
    Console.WriteLine("Comando ejecutado.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
