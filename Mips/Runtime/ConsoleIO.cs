using MipsSimulator.Ui.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipsSimulator.Mips.Runtime; 
public sealed class ConsoleIO : IDisposable {

    private readonly ConsoleLog consoleLog;

    private readonly Queue<string> stdin;

    public ConsoleIO(ConsoleLog consoleLog)
    {
        this.consoleLog = consoleLog;
        stdin = new();
        this.consoleLog.OnInput += OnInput;
    }

    private void OnInput(string input) {
        stdin.Enqueue(input);
    }

    public async Task<string> Read() {
        await Extensions.WaitUntil(() => stdin.Count > 0);
        return stdin.Dequeue();
    }

    public void Write(string str) => consoleLog.Write(str);

    public void Dispose() {
        consoleLog.OnInput -= OnInput;
    }
}
