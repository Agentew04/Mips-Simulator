using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Vortice.Direct3D11;

namespace MipsSimulator.Ui;
public class LogicThread
{

    public LogicThread(WindowManager windowManager)
    {
        this.windowManager = windowManager;
        cts = new();
        windowManager.OnSubmitUI += SubmitUI;
        windowManager.OnCloseWindow += () =>
        {
            cts.Cancel();
        };

        thread = new Thread(Main);
    }

    #region Private Variables

    private readonly WindowManager windowManager;
    private readonly Thread thread;
    private readonly CancellationTokenSource cts;

    #endregion

    private async void Main()
    {
        int i = 0;
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync(cts.Token).NoThrow())
        {
            i++;
            Console.WriteLine($"Main: {i}");
        }
        Console.WriteLine("bye");
    }

    public void Start()
    { // main thread
        thread.Start();
    }

    public void WaitForExit()
    { // main thread
        thread.Join();
    }

    #region Event Handlers

    private void SubmitUI()
    {
        CreateDockSpace(true, false);
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                ImGui.MenuItem("New", "CTRL+N");
                ImGui.MenuItem("Open", "CTRL+O");
                ImGui.MenuItem("Save", "CTRL+S");
                ImGui.MenuItem("Save As", "CTRL+SHIFT+S");
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }


        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        ImGui.ShowDemoWindow();

        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        if (ImGui.Begin("SMS - Simple MIPS Simulator"))
        {
            ImGui.Text("Hello World!");
            if (ImGui.Button("Close"))
            {
                windowManager.CloseWindow();
            }
            ImGui.End();
        }
    }

    private void CreateDockSpace(bool fullscreen, bool padding)
    {
        ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDocking; // | ImGuiWindowFlags.MenuBar -> caso menu
        ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None;

        if (fullscreen)
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();

            ImGui.SetNextWindowPos(viewport.WorkPos);
            ImGui.SetNextWindowSize(viewport.WorkSize);
            ImGui.SetNextWindowViewport(viewport.ID);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            window_flags |=
                ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
        }
        else
        {
            dockspace_flags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
        }

        if (dockspace_flags.HasFlag(ImGuiDockNodeFlags.PassthruCentralNode))
        {
            window_flags |= ImGuiWindowFlags.NoBackground;
        }

        if (!padding)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
        }

        ImGui.Begin("DockSpace Demo", window_flags);

        if (!padding)
        {
            ImGui.PopStyleVar();
        }

        if (fullscreen)
        {
            ImGui.PopStyleVar(2); // remove os estilos da janela para o usuario fazer o proprio
        }

        if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.DockingEnable))
        {
            uint dockspace_id = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(dockspace_id, Vector2.Zero, dockspace_flags);
        }
        else
        {
            ImGui.Text("ERROR: Docking is not enabled!");
        }

        ImGui.End();
    }

    #endregion
}
