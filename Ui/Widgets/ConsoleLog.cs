using ImGuiNET;
using System.Numerics;

namespace MipsSimulator.Ui.Widgets; 
public sealed class ConsoleLog : IWidget, IDisposable {

    private string lines;
    private string input;


    public ConsoleLog() {
        lines = "";
        input = "";
    }

    public void Write(string text) {
        lines += text + "\n";
    }

    public void Show() {
        if (ImGui.Begin("Console")) {
            ImGui.InputText("##consoleinput", ref input, 80);
            ImGui.SameLine();
            if (ImGui.Button("Write")) {
                lines += input + "\n";
                OnInput?.Invoke(input);
                input = "";
            }
            ImGui.SameLine();
            if (ImGui.Button("Clear")) {
                lines = "";
            }

            ImGui.Text(lines);

            ImGui.End();
        }
    }

    public event Action<string>? OnInput;

    public void Dispose() {
    }
}
