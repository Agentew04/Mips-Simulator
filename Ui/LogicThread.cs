using ImGuiNET;
using MipsSimulator.Mips;
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

    private Cpu cpu;

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
        cpu = new();   
        
        // small program
        cpu.Memory.WriteWord(0, 0x24080001); // li t0, 1
        cpu.Memory.WriteWord(4, 0x24090002); // li t1, 2
        cpu.Memory.WriteWord(8, 0x01095020); // add t2, t0, t1

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

            if (ImGui.MenuItem("Step")) {
                cpu.Step();
            }
            ImGui.EndMainMenuBar();
        }

        // pelo visto as docks vao sempre na esquerda
        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        if(ImGui.Begin("Console")) {
            ImGui.Text("Hello World!");
            ImGui.End();
        }
        //ImGui.ShowDemoWindow();

        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        if (ImGui.Begin("Registers"))
        {
            ShowRegistersTable();
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

    private void ShowRegistersTable() {
        ImGui.BeginTable("Registers", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingFixedFit);

        // setup headers
        // Name - Number - Value
        ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.None);
        ImGui.TableSetupColumn("Number", ImGuiTableColumnFlags.None);
        ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.None);
        ImGui.TableHeadersRow();
 
        foreach(Register r in Enum.GetValues<Register>()) {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text(r.ToString());
            ImGui.TableNextColumn();
            string regNumber = ((int)r).ToString();
            if(r is >= Register.Pc) {
                regNumber = "";
            }
            ImGui.Text(regNumber);
            ImGui.TableNextColumn();
            ImGui.Text(cpu.Registers.GetRegister(r).ToString());
        }
        ImGui.EndTable();
    }

    #endregion
}
