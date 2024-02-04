using ImGuiNET;
using MipsSimulator.Mips.Runtime;

namespace MipsSimulator.Ui.Widgets
{
    public class RegisterTable : IWidget {

        public RegisterTable(CpuRegisters registers) {
            this.registers = registers;
        }

        private readonly CpuRegisters registers;

        public void Show() {
            if (ImGui.Begin("Registers")) {
                ShowTable();
                ImGui.End();
            }
        }

        private void ShowTable() {
            ImGui.BeginTable("Registers", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingFixedFit);

            // setup headers
            // Name - Number - Value
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.None);
            ImGui.TableSetupColumn("Number", ImGuiTableColumnFlags.None);
            ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.None);
            ImGui.TableHeadersRow();

            foreach (Register r in Enum.GetValues<Register>()) {
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text(r.ToString());
                ImGui.TableNextColumn();
                string regNumber = ((int)r).ToString();
                if (r is >= Register.Pc) {
                    regNumber = "";
                }
                ImGui.Text(regNumber);
                ImGui.TableNextColumn();
                ImGui.Text(registers.GetRegister(r).ToString());
            }
            ImGui.EndTable();
        }
    }
}
