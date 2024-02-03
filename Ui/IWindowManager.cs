using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Ui {
    public interface IWindowManager {
        public void Init(int width, int height);
        public void Start();

        public event Action? OnSubmitUI;

        public event Action? OnCloseWindow;
    }
}
