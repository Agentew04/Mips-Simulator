using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Ui.Widgets; 

public interface IWidget {

    /// <summary>
    /// Displays this widget in ImGui.
    /// </summary>
    void Show();
}
