using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MonitorToggleTray;

public partial class Form1 : Form
{
    private NotifyIcon notifyIcon;
    private ContextMenuStrip contextMenu;
    private bool[] monitorStates = new bool[4]; // Support up to 4 monitors
    private int monitorCount = 0;

    private const int WM_HOTKEY = 0x0312;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_ALT = 0x0001;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    public Form1()
    {
        InitializeComponent();
        DetectMonitors();
        SetupTrayIcon();
        RegisterHotKeys();
    }

    private void DetectMonitors()
    {
        // Detect connected monitors (using Screen.AllScreens as reference)
        monitorCount = Math.Min(Screen.AllScreens.Length, 4);
        if (monitorCount == 0) monitorCount = 1; // At least 1 monitor assumed
    }

    private void RegisterHotKeys()
    {
        Keys[] fKeys = { Keys.F1, Keys.F2, Keys.F3, Keys.F4 };
        for (int i = 0; i < monitorCount; i++)
        {
            RegisterHotKey(this.Handle, i + 1, MOD_CONTROL | MOD_ALT, (uint)fKeys[i]);
        }
    }

    private void SetupTrayIcon()
    {
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = CreateMonitorIcon();
        notifyIcon.Text = "Monitor Toggle";
        
        contextMenu = new ContextMenuStrip();

        // Add dynamic menu items for each detected monitor
        for (int i = 1; i <= monitorCount; i++)
        {
            Keys[] fKeys = { Keys.F1, Keys.F2, Keys.F3, Keys.F4 };
            string shortcut = $"Ctrl+Alt+{fKeys[i - 1].ToString()}";
            
            int monitorIndex = i; // Capture for closure
            contextMenu.Items.Add($"Toggle Monitor {i}  [{shortcut}]", null, 
                (s, e) => ToggleMonitor(monitorIndex));
        }

        contextMenu.Items.Add("-"); // Separator

        for (int i = 1; i <= monitorCount; i++)
        {
            int monitorIndex = i;
            contextMenu.Items.Add($"Turn On Monitor {i}", null, 
                (s, e) => TurnOnMonitor(monitorIndex));
            contextMenu.Items.Add($"Turn Off Monitor {i}", null, 
                (s, e) => TurnOffMonitor(monitorIndex));
        }

        contextMenu.Items.Add("-"); // Separator
        contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

        notifyIcon.ContextMenuStrip = contextMenu;
        notifyIcon.Visible = true;
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
        this.Hide();
    }

    private void TurnOnMonitor(int monitor)
    {
        RunCommand($"monitorcontrol --set-power-mode on --monitor {monitor}");
        monitorStates[monitor - 1] = true;
    }

    private void TurnOffMonitor(int monitor)
    {
        RunCommand($"monitorcontrol --set-power-mode off_soft --monitor {monitor}");
        monitorStates[monitor - 1] = false;
    }

    private void ToggleMonitor(int monitor)
    {
        if (monitorStates[monitor - 1])
            TurnOffMonitor(monitor);
        else
            TurnOnMonitor(monitor);
    }

    private Icon CreateMonitorIcon()
    {
        // Create a 256x256 bitmap
        Bitmap bitmap = new Bitmap(256, 256);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw monitor screen (rounded rectangle)
            int screenX = 40;
            int screenY = 30;
            int screenWidth = 175;
            int screenHeight = 130;

            // Draw left side (blue)
            Rectangle leftRect = new Rectangle(screenX, screenY, screenWidth / 2, screenHeight);
            using (Brush blueBrush = new SolidBrush(Color.FromArgb(52, 168, 224)))
            {
                g.FillRectangle(blueBrush, leftRect);
            }

            // Draw right side (dark gray)
            Rectangle rightRect = new Rectangle(screenX + screenWidth / 2, screenY, screenWidth / 2, screenHeight);
            using (Brush grayBrush = new SolidBrush(Color.FromArgb(64, 64, 64)))
            {
                g.FillRectangle(grayBrush, rightRect);
            }

            // Draw border (black rounded outline)
            using (Pen borderPen = new Pen(Color.Black, 3))
            {
                g.DrawRectangle(borderPen, screenX, screenY, screenWidth, screenHeight);
            }

            // Draw diagonal line from top-left to bottom-right
            using (Pen linePen = new Pen(Color.Black, 4))
            {
                g.DrawLine(linePen, screenX + screenWidth / 2, screenY, screenX + screenWidth, screenY + screenHeight);
            }

            // Draw power symbol in the center
            int centerX = screenX + screenWidth / 2;
            int centerY = screenY + screenHeight / 2;
            int powerSize = 30;

            // Draw power circle
            using (Pen powerPen = new Pen(Color.Black, 3))
            {
                g.DrawArc(powerPen, centerX - powerSize, centerY - powerSize, powerSize * 2, powerSize * 2, 
                    -45, 270);
            }

            // Draw power line (vertical)
            using (Pen powerPen = new Pen(Color.Black, 3))
            {
                g.DrawLine(powerPen, centerX, centerY - powerSize - 10, centerX, centerY - powerSize);
            }

            // Draw monitor stand
            int standX = screenX + screenWidth / 2 - 20;
            int standY = screenY + screenHeight + 5;
            using (Brush standBrush = new SolidBrush(Color.Black))
            {
                // Vertical stand
                g.FillRectangle(standBrush, standX + 5, standY, 30, 25);
                // Base
                g.FillRectangle(standBrush, standX - 15, standY + 25, 80, 8);
            }
        }

        // Convert bitmap to icon
        IntPtr hIcon = bitmap.GetHicon();
        Icon icon = Icon.FromHandle(hIcon);
        bitmap.Dispose();
        return icon;
    }

    private void RunCommand(string args)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "uvx",
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error executing command: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY)
        {
            int hotkeyId = m.WParam.ToInt32();
            if (hotkeyId >= 1 && hotkeyId <= monitorCount)
            {
                ToggleMonitor(hotkeyId);
            }
        }
        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        for (int i = 1; i <= monitorCount; i++)
        {
            UnregisterHotKey(this.Handle, i);
        }
        notifyIcon.Dispose();
        base.OnFormClosing(e);
    }
}
