using ImGuiNET;
using ImGuiTeste.Ui;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ImGuiTeste;

public class Program {

    static void Main(string[] args) {
        using WindowManager windowManager = new();
        LogicThread logicThread = new(windowManager);
        windowManager.ClearColor = new Vector3(0.45f, 0.55f, 0.6f);
        windowManager.Title = "SMS - Simple MIPS Simulator";

        windowManager.Init(1280,720);
        logicThread.Start();
        windowManager.StartMainLoop();
        logicThread.WaitForExit();
    }

    public static void ThreadMain(object? wndmng) {
        WindowManager windowManager = (WindowManager)wndmng!;

        windowManager.OnSubmitUI += () => {
            if (ImGui.BeginMainMenuBar()) {
                if (ImGui.BeginMenu("File")) {
                    ImGui.MenuItem("New");
                    ImGui.MenuItem("Open");
                    ImGui.MenuItem("Save");
                    ImGui.MenuItem("Save As");
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }


            if (ImGui.Begin("SMS - Simple MIPS Simulator")) {
                ImGui.Text("Hello World!");
                if (ImGui.Button("Close")) {
                    windowManager.CloseWindow();
                }
                ImGui.End();
            }
            //    // ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse
            //    // | ImGuiWindowFlags.NoTitleBar);
            ////ImGui.SetNextWindowSize(ImGui.GetIO().DisplaySize);
            ////ImGui.SetNextWindowPos(Vector2.Zero);
            //ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            //uint dockspaceId = ImGui.GetID("MyDockSpace");
            //ImGui.DockSpace(dockspaceId, Vector2.Zero, ImGuiDockNodeFlags.NoUndocking);
            
            //ImGuiNET.
            
            
            //ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.PassthruCentralNode;
            
        };

        int i = 0;
        while (true) {
            Console.WriteLine($"Main: {i++}");
            Thread.Sleep(1000);
        }
    }
}
