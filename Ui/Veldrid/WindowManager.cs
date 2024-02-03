using ImGuiNET;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace MipsSimulator.Ui.Veldrid;

public sealed class WindowManager : IDisposable, IWindowManager
{

    #region Private Variables

    private Sdl2Window window = null!;
    private GraphicsDevice graphicsDevice = null!;
    private CommandList commandList = null!;
    private ImGuiController imGuiRenderer = null!;
    private bool closeWindow = false;
    private ImFontPtr font;

    #endregion

    #region Public Variables

    public int WindowWidth { get; private set; }
    public int WindowHeight { get; private set; }
    public Vector3 ClearColor { get; set; } = new(1.0f, 1.0f, 1.0f);
    public string Title { get; set; } = "";


    #endregion

    public void Init(int width, int height)
    {
        window = VeldridStartup.CreateWindow(new WindowCreateInfo(50, 50, width, height, WindowState.Normal, Title));
        graphicsDevice = VeldridStartup.CreateDefaultOpenGLGraphicsDevice(
            options: new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
            window,
            GraphicsBackend.OpenGL);


        window.Resized += () =>
        {
            graphicsDevice.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
            imGuiRenderer.WindowResized(window.Width, window.Height);
            WindowWidth = window.Width;
            WindowHeight = window.Height;
        };
        commandList = graphicsDevice.ResourceFactory.CreateCommandList();
        imGuiRenderer = new ImGuiController(graphicsDevice, graphicsDevice.MainSwapchain.Framebuffer.OutputDescription, window.Width, window.Height);

        LoadFont();

    }

    public void Start()
    {
        Stopwatch sw = new();
        float deltaTime;
        ImGui.PushFont(font);

        while (window.Exists && !closeWindow)
        {
            deltaTime = sw.ElapsedTicks / (float)Stopwatch.Frequency;
            sw.Restart();
            InputSnapshot snapshot = window.PumpEvents();
            if (!window.Exists)
            {
                break;
            }
            // passa pro renderer que deveria passar pro imgui
            imGuiRenderer.Update(deltaTime, snapshot);

            OnSubmitUI?.Invoke();

            commandList.Begin();
            commandList.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, new RgbaFloat(ClearColor.X, ClearColor.Y, ClearColor.Z, 1f));
            imGuiRenderer.Render(graphicsDevice, commandList);
            commandList.End();
            graphicsDevice.SubmitCommands(commandList);
            graphicsDevice.SwapBuffers(graphicsDevice.MainSwapchain);
        }
        ImGui.PopFont();
        graphicsDevice.WaitForIdle();
        OnCloseWindow?.Invoke();
    }

    private unsafe void LoadFont()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        using Stream? s = asm.GetManifestResourceStream("Resources/Fonts/JetBrainsMono-Regular.ttf");
        if (s is null)
        {
            return;
        }

        byte[] buffer = new byte[s.Length];
        s.Read(buffer);
        fixed (byte* bufferPtr = buffer)
        {
            font = ImGui.GetIO().Fonts.AddFontFromMemoryTTF((nint)bufferPtr, buffer.Length, 16);
        }
    }

    public void Dispose()
    {
        imGuiRenderer.Dispose();
        commandList.Dispose();
        graphicsDevice.Dispose();
    }

    public void CloseWindow()
    {
        closeWindow = true;
    }

    public event Action? OnSubmitUI;

    public event Action? OnCloseWindow;
}
