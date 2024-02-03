using ImGuiNET;
using System.Numerics;

namespace MipsSimulator.Ui.Widgets {
    public class ConsoleLog : IWidget {

        private string lines = "";
        private string input = "";
        private const uint maxLenght = 160 * 200; // 160 chars per line, 200 lines

        public ConsoleLog() {
            lines = "";
        }

        public void Show() {
            if (ImGui.Begin("Console")) {
                if (ImGui.Button("Clear")) {
                    lines = "";
                }

                int padding = 10;
                Vector2 windowSize = new(ImGui.GetWindowSize().X, ImGui.GetWindowSize().Y);
                windowSize.X -= padding;
                windowSize.Y -= 100;
                ImGui.InputTextMultiline("Output", ref lines, maxLenght, windowSize, ImGuiInputTextFlags.ReadOnly);

                ImGui.Text("Input: ");
                ImGui.SameLine();
                ImGui.InputText("##consoleinput", ref input, 80);
                ImGui.SameLine();
                if (ImGui.Button("Send")) {
                    WriteMessage(input);
                    input = "";
                }

                ImGui.End();
            }
        }

        public void WriteMessage(string message) {
            lines += message + "\n";
        }

    }
}
