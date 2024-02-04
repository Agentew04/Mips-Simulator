using ImGuiNET;
using MipsSimulator.Mips;
using MipsSimulator.Mips.Runtime;
using MipsSimulator.Ui.Widgets;
using System.Numerics;

namespace MipsSimulator.Ui;

public class LogicThread {

    #region Main Thread

    public LogicThread(IWindowManager windowManager) {
        this.windowManager = windowManager;
        cts = new();
        windowManager.OnSubmitUI += SubmitUI;
        windowManager.OnCloseWindow += () => {
            cts.Cancel();
        };

        thread = new Thread(Main);
    }

    public void Start() { // main thread
        thread.Start();
    }

    public void WaitForExit() { // main thread
        thread.Join();
    }

    #endregion

    #region Private Variables

    private readonly IWindowManager windowManager;
    private readonly Thread thread;
    private readonly CancellationTokenSource cts;
    private Cpu cpu;
    private RegisterTable registerTable;
    private ConsoleLog consoleLog;
    private ConsoleIO stdio;

    #endregion

    private void Main() {
        cpu = new();
        registerTable = new(cpu.Registers);
        consoleLog = new();
        stdio = new(consoleLog);
        cpu.SetStdIO(() => {
            return stdin.Count > 0 ? stdin.Dequeue() : "";
        }, consoleLog.Write);
        consoleLog.OnInput += (input) => {
            stdin.Enqueue(input);
        };

        // load test program
        for (int j = 0; j < TestProgram.Length; j++) {
            cpu.Memory.WriteWord((uint)j * 4, TestProgram[j]);
        }
    }

    

    private readonly uint[] TestProgram = [
        // li v0, 1
        0x24020001,
        // li a0, 5
        0x24040005,
        // syscall
        0x0000000c,
        // li v0, 5
        0x24020005,
        // syscall
        0x0000000c,
        // add a0, zero, v0
        0x00842020,
        // li v0, 1
        0x24020001,
        // syscall
        0x0000000c,
        //0x24080001, // li t0, 1
        //0x24090002, // li t1, 2
        //0x01095020, // add t2, t0, t1
    ];

    #region Event Handlers

    private void SubmitUI() {
        CreateDockSpace(true, false);
        if (ImGui.BeginMainMenuBar()) {
            if (ImGui.BeginMenu("File")) {
                ImGui.MenuItem("New", "CTRL+N");
                ImGui.MenuItem("Open", "CTRL+O");
                ImGui.MenuItem("Save", "CTRL+S");
                ImGui.MenuItem("Save As", "CTRL+SHIFT+S");
                ImGui.EndMenu();
            }

            if (ImGui.MenuItem("Step")) {
                consoleLog.Write("Step");
                cpu.Step();
            }

            if (ImGui.MenuItem("Continue")) {
                consoleLog.Write("Continue");
                cpu.Continue();
            }

            if (ImGui.MenuItem("Stop")) {
                consoleLog.Write("stop");
                cpu.Stop();
            }
            ImGui.EndMainMenuBar();
        }

        // pelo visto as docks vao sempre na esquerda
        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        ImGui.ShowDemoWindow();

        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        consoleLog.Show();
        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
        registerTable.Show();
    }

    private void CreateDockSpace(bool fullscreen, bool padding) {
        ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDocking; // | ImGuiWindowFlags.MenuBar -> caso menu
        ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None;

        if (fullscreen) {
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
        } else {
            dockspace_flags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
        }

        if (dockspace_flags.HasFlag(ImGuiDockNodeFlags.PassthruCentralNode)) {
            window_flags |= ImGuiWindowFlags.NoBackground;
        }

        if (!padding) {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
        }

        ImGui.Begin("DockSpace Demo", window_flags);

        if (!padding) {
            ImGui.PopStyleVar();
        }

        if (fullscreen) {
            ImGui.PopStyleVar(2); // remove os estilos da janela para o usuario fazer o proprio
        }

        if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.DockingEnable)) {
            uint dockspace_id = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(dockspace_id, Vector2.Zero, dockspace_flags);
        } else {
            ImGui.Text("ERROR: Docking is not enabled!");
        }

        ImGui.End();
    }


    #endregion
}
