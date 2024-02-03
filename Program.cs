using MipsSimulator.Ui;
using MipsSimulator.Ui.Silk.NET;


// using MipsSimulator.Ui.Silk.Veldrid to use Veldrid(SDL2)
using System.Numerics;

namespace MipsSimulator;

public class Program {

    static void Main(string[] args) {
        using WindowManager windowManager = new();
        LogicThread logicThread = new(windowManager);
        windowManager.ClearColor = new Vector3(0.45f, 0.55f, 0.6f);
        windowManager.Title = "SMS - Simple MIPS Simulator";

        windowManager.Init(1280, 720);
        logicThread.Start();
        windowManager.Start();
        logicThread.WaitForExit();
    }
}
