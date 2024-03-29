﻿using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;
using System.Reflection;

namespace MipsSimulator.Ui.Silk.NET;

public sealed class WindowManager : IDisposable, IWindowManager {

    #region Private Variables

    private bool initialized = false;
    private IWindow window = null!;
    private GL gl = null!;
    IInputContext inputContext = null!;

    private ImGuiController imguiController = null!;
    private ImFontPtr? font = null;

    #endregion

    #region Public Variables

    public int WindowWidth { get; private set; }
    public int WindowHeight { get; private set; }
    public Vector3 ClearColor { get; set; } = new(1.0f, 1.0f, 1.0f);
    public string Title { get; set; } = "";


    #endregion

    public void Init(int width, int height) {
        window = Window.Create(WindowOptions.Default);

        window.Load += WindowLoad;
        window.FramebufferResize += WindowResize;
        window.Render += WindowRender;
        window.Closing += WindowClosed;

        LoadFont();
        initialized = true;
    }

    public void Start() {
        if (!initialized) {
            throw new InvalidOperationException("Window not initialized. Call Init(int,int) first");
        }
        window.Run();
    }

    private void WindowLoad() {
        gl = window.CreateOpenGL();
        inputContext = window.CreateInput();
        imguiController = new ImGuiController(gl, window, inputContext);
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }

    private void WindowResize(Vector2D<int> s) {
        gl.Viewport(s);
    }

    private void WindowRender(double delta) {
        imguiController.Update((float)delta);

        gl.ClearColor(Color.FromArgb((int)(ClearColor.X * 255), (int)(ClearColor.Y * 255), (int)(ClearColor.Z * 255)));
        gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        if (font is not null) {
            ImGui.PushFont(font.Value);
        }
        OnSubmitUI?.Invoke();
        if (font is not null) {
            ImGui.PopFont();
        }

        imguiController.Render();
    }

    private void WindowClosed() {
        imguiController.Dispose();
        inputContext.Dispose();
        gl.Dispose();
        OnCloseWindow?.Invoke();
    }


    private void LoadFont() {
        var io = ImGui.GetIO();
        //font = io.Fonts.AddFontFromFileTTF("Resources/Fonts/JetBrainsMono-Regular.ttf", 16.0f);
    }

    public void Dispose() {
        window.Dispose();
    }

    public event Action? OnSubmitUI;

    public event Action? OnCloseWindow;
}
