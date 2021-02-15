
using System;
using Avalonia.Input;
using AvaloniaKey = global::Avalonia.Input.Key;
using AvaloniaModifiers = global::Avalonia.Input.KeyModifiers;
using Keys = engenious.Input.Keys;
using KeyModifiers = OpenTK.Windowing.GraphicsLibraryFramework.KeyModifiers;
using MouseButton = engenious.Input.MouseButton;

namespace engenious.Avalonia
{
    public static class AvaloniaKeyMap
    {
        public static readonly engenious.Input.Keys[] Map;

        static AvaloniaKeyMap()
        {
            Map = GenerateAvaloniaKeyMapping();
        }

        public static MouseButton MapMouseButton(PointerUpdateKind updateKind)
        {
            MouseButton button = updateKind switch
            {
                PointerUpdateKind.LeftButtonPressed => MouseButton.Left,
                PointerUpdateKind.MiddleButtonPressed => MouseButton.Middle,
                PointerUpdateKind.RightButtonPressed => MouseButton.Right,
                PointerUpdateKind.XButton1Pressed => MouseButton.Button1,
                PointerUpdateKind.XButton2Pressed => MouseButton.Button2,
                PointerUpdateKind.LeftButtonReleased => MouseButton.Left,
                PointerUpdateKind.MiddleButtonReleased => MouseButton.Middle,
                PointerUpdateKind.RightButtonReleased => MouseButton.Right,
                PointerUpdateKind.XButton1Released => MouseButton.Button1,
                PointerUpdateKind.XButton2Released => MouseButton.Button2,
                _ => throw new ArgumentOutOfRangeException()
            };
            return button;
        }
        public static KeyModifiers MapModifier(AvaloniaModifiers modifiers)
        {
            if (modifiers.HasFlag(AvaloniaModifiers.Shift))
                return KeyModifiers.Shift;
            if (modifiers.HasFlag(AvaloniaModifiers.Alt))
                return KeyModifiers.Alt;
            if (modifiers.HasFlag(AvaloniaModifiers.Control))
                return KeyModifiers.Control;
            if (modifiers.HasFlag(AvaloniaModifiers.Meta))
                return KeyModifiers.Super;
            return 0;
        }
        private static engenious.Input.Keys[] GenerateAvaloniaKeyMapping()
        {
            var map = new engenious.Input.Keys[(int)global::Avalonia.Input.Key.FnDownArrow + 1];
            map[(int)AvaloniaKey.Space] = Keys.Space;
            //map[(int)AvaloniaKey.Apostrophe] = Key.Quote;
            map[(int)AvaloniaKey.OemComma] = Keys.Comma;
            map[(int)AvaloniaKey.OemMinus] = Keys.Minus;
            map[(int)AvaloniaKey.OemPeriod] = Keys.Period;
            //map[(int)AvaloniaKey.Slash] = Key.Slash;
            map[(int)AvaloniaKey.D0] = Keys.D0;
            map[(int)AvaloniaKey.D1] = Keys.D1;
            map[(int)AvaloniaKey.D2] = Keys.D2;
            map[(int)AvaloniaKey.D3] = Keys.D3;
            map[(int)AvaloniaKey.D4] = Keys.D4;
            map[(int)AvaloniaKey.D5] = Keys.D5;
            map[(int)AvaloniaKey.D6] = Keys.D6;
            map[(int)AvaloniaKey.D7] = Keys.D7;
            map[(int)AvaloniaKey.D8] = Keys.D8;
            map[(int)AvaloniaKey.D9] = Keys.D9;
            map[(int)AvaloniaKey.OemSemicolon] = Keys.Semicolon;
            //map[(int)AvaloniaKey.OemPlus] = Keys.Plus;
            map[(int)AvaloniaKey.A] = Keys.A;
            map[(int)AvaloniaKey.B] = Keys.B;
            map[(int)AvaloniaKey.C] = Keys.C;
            map[(int)AvaloniaKey.D] = Keys.D;
            map[(int)AvaloniaKey.E] = Keys.E;
            map[(int)AvaloniaKey.F] = Keys.F;
            map[(int)AvaloniaKey.G] = Keys.G;
            map[(int)AvaloniaKey.H] = Keys.H;
            map[(int)AvaloniaKey.I] = Keys.I;
            map[(int)AvaloniaKey.J] = Keys.J;
            map[(int)AvaloniaKey.K] = Keys.K;
            map[(int)AvaloniaKey.L] = Keys.L;
            map[(int)AvaloniaKey.M] = Keys.M;
            map[(int)AvaloniaKey.N] = Keys.N;
            map[(int)AvaloniaKey.O] = Keys.O;
            map[(int)AvaloniaKey.P] = Keys.P;
            map[(int)AvaloniaKey.Q] = Keys.Q;
            map[(int)AvaloniaKey.R] = Keys.R;
            map[(int)AvaloniaKey.S] = Keys.S;
            map[(int)AvaloniaKey.T] = Keys.T;
            map[(int)AvaloniaKey.U] = Keys.U;
            map[(int)AvaloniaKey.V] = Keys.V;
            map[(int)AvaloniaKey.W] = Keys.W;
            map[(int)AvaloniaKey.X] = Keys.X;
            map[(int)AvaloniaKey.Y] = Keys.Y;
            map[(int)AvaloniaKey.Z] = Keys.Z;
            map[(int)AvaloniaKey.OemOpenBrackets] = Keys.BracketLeft;
            map[(int)AvaloniaKey.OemBackslash] = Keys.BackSlash;
            map[(int)AvaloniaKey.OemCloseBrackets] = Keys.BracketRight;
            //map[(int)AvaloniaKey.GraveAccent] = Key.Grave;

            // TODO: What are these world keys and how do I handle them.
            // map[(int)Keys.World1] = Key.Z;
            // map[(int)Keys.World2] = Key.Z;
            map[(int)AvaloniaKey.Escape] = Keys.Escape;
            map[(int)AvaloniaKey.Enter] = Keys.Enter;
            map[(int)AvaloniaKey.Tab] = Keys.Tab;
            map[(int)AvaloniaKey.Back] = Keys.BackSpace;
            map[(int)AvaloniaKey.Insert] = Keys.Insert;
            map[(int)AvaloniaKey.Delete] = Keys.Delete;
            map[(int)AvaloniaKey.Right] = Keys.Right;
            map[(int)AvaloniaKey.Left] = Keys.Left;
            map[(int)AvaloniaKey.Down] = Keys.Down;
            map[(int)AvaloniaKey.Up] = Keys.Up;
            map[(int)AvaloniaKey.PageUp] = Keys.PageUp;
            map[(int)AvaloniaKey.PageDown] = Keys.PageDown;
            map[(int)AvaloniaKey.Home] = Keys.Home;
            map[(int)AvaloniaKey.End] = Keys.End;
            map[(int)AvaloniaKey.CapsLock] = Keys.CapsLock;
            map[(int)AvaloniaKey.Scroll] = Keys.ScrollLock;
            map[(int)AvaloniaKey.NumLock] = Keys.NumLock;
            map[(int)AvaloniaKey.PrintScreen] = Keys.PrintScreen;
            map[(int)AvaloniaKey.Pause] = Keys.Pause;
            map[(int)AvaloniaKey.F1] = Keys.F1;
            map[(int)AvaloniaKey.F2] = Keys.F2;
            map[(int)AvaloniaKey.F3] = Keys.F3;
            map[(int)AvaloniaKey.F4] = Keys.F4;
            map[(int)AvaloniaKey.F5] = Keys.F5;
            map[(int)AvaloniaKey.F6] = Keys.F6;
            map[(int)AvaloniaKey.F7] = Keys.F7;
            map[(int)AvaloniaKey.F8] = Keys.F8;
            map[(int)AvaloniaKey.F9] = Keys.F9;
            map[(int)AvaloniaKey.F10] = Keys.F10;
            map[(int)AvaloniaKey.F11] = Keys.F11;
            map[(int)AvaloniaKey.F12] = Keys.F12;
            map[(int)AvaloniaKey.F13] = Keys.F13;
            map[(int)AvaloniaKey.F14] = Keys.F14;
            map[(int)AvaloniaKey.F15] = Keys.F15;
            map[(int)AvaloniaKey.F16] = Keys.F16;
            map[(int)AvaloniaKey.F17] = Keys.F17;
            map[(int)AvaloniaKey.F18] = Keys.F18;
            map[(int)AvaloniaKey.F19] = Keys.F19;
            map[(int)AvaloniaKey.F20] = Keys.F20;
            map[(int)AvaloniaKey.F21] = Keys.F21;
            map[(int)AvaloniaKey.F22] = Keys.F22;
            map[(int)AvaloniaKey.F23] = Keys.F23;
            map[(int)AvaloniaKey.F24] = Keys.F24;
            map[(int)AvaloniaKey.NumPad0] = Keys.Keypad0;
            map[(int)AvaloniaKey.NumPad1] = Keys.Keypad1;
            map[(int)AvaloniaKey.NumPad2] = Keys.Keypad2;
            map[(int)AvaloniaKey.NumPad3] = Keys.Keypad3;
            map[(int)AvaloniaKey.NumPad4] = Keys.Keypad4;
            map[(int)AvaloniaKey.NumPad5] = Keys.Keypad5;
            map[(int)AvaloniaKey.NumPad6] = Keys.Keypad6;
            map[(int)AvaloniaKey.NumPad7] = Keys.Keypad7;
            map[(int)AvaloniaKey.NumPad8] = Keys.Keypad8;
            map[(int)AvaloniaKey.NumPad9] = Keys.Keypad9;
            map[(int)AvaloniaKey.Decimal] = Keys.KeypadDecimal;
            map[(int)AvaloniaKey.Divide] = Keys.KeypadDivide;
            map[(int)AvaloniaKey.Multiply] = Keys.KeypadMultiply;
            map[(int)AvaloniaKey.Subtract] = Keys.KeypadSubtract;
            map[(int)AvaloniaKey.Add] = Keys.KeypadAdd;
            map[(int)AvaloniaKey.Enter] = Keys.KeypadEnter;
            //map[(int)AvaloniaKey.KeyPadEqual] = Key.KeyPadEqual;
            map[(int)AvaloniaKey.LeftShift] = Keys.LeftShift;
            //map[(int)AvaloniaKey.LeftControl] = Key.LeftCtrl;
            map[(int)AvaloniaKey.LeftAlt] = Keys.LeftAlt;
            map[(int)AvaloniaKey.LWin] = Keys.WinLeft;
            map[(int)AvaloniaKey.RightShift] = Keys.RightShift;
            //map[(int)AvaloniaKey.RightControl] = Key.RightCtrl;
            map[(int)AvaloniaKey.RightAlt] = Keys.RightAlt;
            map[(int)AvaloniaKey.RWin] = Keys.WinRight;
            return map;
        }
    }
}