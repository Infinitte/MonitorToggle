using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MonitorToggleTray;

public partial class Form1 : Form
{
    private NotifyIcon notifyIcon;
    private ContextMenuStrip contextMenu;
    private bool isMonitor1On = false;
    private bool isMonitor2On = false;

    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_ID1 = 1;
    private const int HOTKEY_ID2 = 2;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_ALT = 0x0001;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    public Form1()
    {
        InitializeComponent();
        SetupTrayIcon();
        RegisterHotKey(this.Handle, HOTKEY_ID1, MOD_CONTROL | MOD_ALT, (uint)Keys.F1);
        RegisterHotKey(this.Handle, HOTKEY_ID2, MOD_CONTROL | MOD_ALT, (uint)Keys.F2);
    }

    private void SetupTrayIcon()
    {
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = SystemIcons.Information; // Icono tipo información, puedes cambiar a uno personalizado
        notifyIcon.Text = "Monitor Toggle";
        contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Toggle Monitor 1", null, (s, e) => Toggle1());
        contextMenu.Items.Add("Toggle Monitor 2", null, (s, e) => Toggle2());
        contextMenu.Items.Add("Turn On Monitor 1", null, (s, e) => TurnOn1());
        contextMenu.Items.Add("Turn Off Monitor 1", null, (s, e) => TurnOff1());
        contextMenu.Items.Add("Turn On Monitor 2", null, (s, e) => TurnOn2());
        contextMenu.Items.Add("Turn Off Monitor 2", null, (s, e) => TurnOff2());
        contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
        notifyIcon.ContextMenuStrip = contextMenu;
        notifyIcon.Visible = true;
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
        this.Hide();
    }

    private void TurnOn1()
    {
        RunCommand("monitorcontrol --set-power-mode on --monitor 1");
        isMonitor1On = true;
    }

    private void TurnOff1()
    {
        RunCommand("monitorcontrol --set-power-mode off_soft --monitor 1");
        isMonitor1On = false;
    }

    private void Toggle1()
    {
        if (isMonitor1On)
            TurnOff1();
        else
            TurnOn1();
    }

    private void TurnOn2()
    {
        RunCommand("monitorcontrol --set-power-mode on --monitor 2");
        isMonitor2On = true;
    }

    private void TurnOff2()
    {
        RunCommand("monitorcontrol --set-power-mode off_soft --monitor 2");
        isMonitor2On = false;
    }

    private void Toggle2()
    {
        if (isMonitor2On)
            TurnOff2();
        else
            TurnOn2();
    }

    private void RunCommand(string args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "uvx",
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY)
        {
            int id = m.WParam.ToInt32();
            if (id == HOTKEY_ID1)
                Toggle1();
            else if (id == HOTKEY_ID2)
                Toggle2();
        }
        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnregisterHotKey(this.Handle, HOTKEY_ID1);
        UnregisterHotKey(this.Handle, HOTKEY_ID2);
        notifyIcon.Dispose();
        base.OnFormClosing(e);
    }
}
