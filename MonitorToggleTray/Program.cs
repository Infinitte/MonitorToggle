using System;
using System.Threading;

namespace MonitorToggleTray;

static class Program
{
    private static Mutex? instanceMutex;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Ensure only one instance of the application runs
        const string mutexName = "MonitorToggleTray_SingleInstance";
        instanceMutex = new Mutex(true, mutexName, out bool isNewInstance);

        if (!isNewInstance)
        {
            MessageBox.Show("MonitorToggleTray is already running.", "Already Running", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        finally
        {
            instanceMutex?.ReleaseMutex();
            instanceMutex?.Dispose();
        }
    }    
}