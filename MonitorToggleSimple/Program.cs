// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string stateFile = "monitor_state.txt";
        bool isOn = File.Exists(stateFile) && File.ReadAllText(stateFile).Trim() == "on";

        if (isOn)
        {
            Console.WriteLine("Apagando monitor 1...");
            RunCommand("monitorcontrol --set-power-mode off_soft --monitor 1");
            File.WriteAllText(stateFile, "off");
        }
        else
        {
            Console.WriteLine("Encendiendo monitor 1...");
            RunCommand("monitorcontrol --set-power-mode on --monitor 1");
            File.WriteAllText(stateFile, "on");
        }

        Console.WriteLine("Toggle completado.");
    }

    static void RunCommand(string args)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "uvx",
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            })?.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
