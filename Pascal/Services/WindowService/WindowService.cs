using Pascal.Models;
using Pascal.Views.SubPages;
using Windows.Graphics;

namespace Pascal.Services.WindowService;

public class WindowService : IWindowService
{
    public void ShowWindowsUpdateSimulation()
    {
        var monitors = Models.Monitor.All.ToArray();

        if (monitors.Length == 0)
            return;

        foreach (var monitor in monitors)
        {
            var window = CreateWindowForMonitor(monitor);
            PositionWindow(window, monitor);
            window.Activate();
        }
    }

    private WindowsUpdateWindow CreateWindowForMonitor(Models.Monitor monitor)
    {
        var window = new WindowsUpdateWindow
        {
            ShowUIContent = monitor.IsPrimary
        };
        return window;
    }

    private void PositionWindow(WindowsUpdateWindow window, Models.Monitor monitor)
    {
        window.AppWindow.Move(new PointInt32(
            monitor.WorkingArea.X,
            monitor.WorkingArea.Y
        ));
    }
}
