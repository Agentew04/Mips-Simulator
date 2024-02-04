using ImGuiNET;
using System.Numerics;

namespace MipsSimulator.Ui.Widgets; 
public sealed class ConsoleLog : IWidget, IDisposable {

    private string text;
    private string input;
    private int lines = 0;


    public ConsoleLog() {
        text = "";
        input = "";
    }

    public void Write(string text) {
        this.text += text + "\n";
        lines++;

        if (lines > 100) {
            int index = this.text.IndexOf('\n');
            this.text = this.text[(index + 1)..];
            lines--;
        }
    }

    public void Show() {
        if (ImGui.Begin("Console")) {
            ImGui.InputText("##consoleinput", ref input, 80);
            ImGui.SameLine();
            if (ImGui.Button("Write")) {
                text += input + "\n";
                OnInput?.Invoke(input);
                input = "";
            }
            ImGui.SameLine();
            if (ImGui.Button("Clear")) {
                text = "";
            }

            ImGui.Text(text);

            ImGui.End();
        }
    }

    public event Action<string>? OnInput;

    public void Dispose() {
    }
}
