
using System;
using Avalonia.Input;
using AvaloniaKey = global::Avalonia.Input.Key;
using AvaloniaModifiers = global::Avalonia.Input.KeyModifiers;
using Key = OpenToolkit.Windowing.Common.Input.Key;
using KeyModifiers = OpenToolkit.Windowing.Common.Input.KeyModifiers;
using MouseButton = OpenToolkit.Windowing.Common.Input.MouseButton;

namespace engenious.Avalonia
{
    public static class AvaloniaKeyMap
    {
        public static readonly Key[] Map;

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
                return KeyModifiers.Command;
            return 0;
        }
        private static Key[] GenerateAvaloniaKeyMapping()
        {
            var map = new Key[(int)global::Avalonia.Input.Key.FnDownArrow + 1];
            map[(int)AvaloniaKey.Space] = Key.Space;
            //map[(int)AvaloniaKey.Apostrophe] = Key.Quote;
            map[(int)AvaloniaKey.OemComma] = Key.Comma;
            map[(int)AvaloniaKey.OemMinus] = Key.Minus;
            map[(int)AvaloniaKey.OemPeriod] = Key.Period;
            //map[(int)AvaloniaKey.Slash] = Key.Slash;
            map[(int)AvaloniaKey.D0] = Key.Number0;
            map[(int)AvaloniaKey.D1] = Key.Number1;
            map[(int)AvaloniaKey.D2] = Key.Number2;
            map[(int)AvaloniaKey.D3] = Key.Number3;
            map[(int)AvaloniaKey.D4] = Key.Number4;
            map[(int)AvaloniaKey.D5] = Key.Number5;
            map[(int)AvaloniaKey.D6] = Key.Number6;
            map[(int)AvaloniaKey.D7] = Key.Number7;
            map[(int)AvaloniaKey.D8] = Key.Number8;
            map[(int)AvaloniaKey.D9] = Key.Number9;
            map[(int)AvaloniaKey.OemSemicolon] = Key.Semicolon;
            map[(int)AvaloniaKey.OemPlus] = Key.Plus;
            map[(int)AvaloniaKey.A] = Key.A;
            map[(int)AvaloniaKey.B] = Key.B;
            map[(int)AvaloniaKey.C] = Key.C;
            map[(int)AvaloniaKey.D] = Key.D;
            map[(int)AvaloniaKey.E] = Key.E;
            map[(int)AvaloniaKey.F] = Key.F;
            map[(int)AvaloniaKey.G] = Key.G;
            map[(int)AvaloniaKey.H] = Key.H;
            map[(int)AvaloniaKey.I] = Key.I;
            map[(int)AvaloniaKey.J] = Key.J;
            map[(int)AvaloniaKey.K] = Key.K;
            map[(int)AvaloniaKey.L] = Key.L;
            map[(int)AvaloniaKey.M] = Key.M;
            map[(int)AvaloniaKey.N] = Key.N;
            map[(int)AvaloniaKey.O] = Key.O;
            map[(int)AvaloniaKey.P] = Key.P;
            map[(int)AvaloniaKey.Q] = Key.Q;
            map[(int)AvaloniaKey.R] = Key.R;
            map[(int)AvaloniaKey.S] = Key.S;
            map[(int)AvaloniaKey.T] = Key.T;
            map[(int)AvaloniaKey.U] = Key.U;
            map[(int)AvaloniaKey.V] = Key.V;
            map[(int)AvaloniaKey.W] = Key.W;
            map[(int)AvaloniaKey.X] = Key.X;
            map[(int)AvaloniaKey.Y] = Key.Y;
            map[(int)AvaloniaKey.Z] = Key.Z;
            map[(int)AvaloniaKey.OemOpenBrackets] = Key.BracketLeft;
            map[(int)AvaloniaKey.OemBackslash] = Key.BackSlash;
            map[(int)AvaloniaKey.OemCloseBrackets] = Key.BracketRight;
            //map[(int)AvaloniaKey.GraveAccent] = Key.Grave;

            // TODO: What are these world keys and how do I handle them.
            // map[(int)Keys.World1] = Key.Z;
            // map[(int)Keys.World2] = Key.Z;
            map[(int)AvaloniaKey.Escape] = Key.Escape;
            map[(int)AvaloniaKey.Enter] = Key.Enter;
            map[(int)AvaloniaKey.Tab] = Key.Tab;
            map[(int)AvaloniaKey.Back] = Key.BackSpace;
            map[(int)AvaloniaKey.Insert] = Key.Insert;
            map[(int)AvaloniaKey.Delete] = Key.Delete;
            map[(int)AvaloniaKey.Right] = Key.Right;
            map[(int)AvaloniaKey.Left] = Key.Left;
            map[(int)AvaloniaKey.Down] = Key.Down;
            map[(int)AvaloniaKey.Up] = Key.Up;
            map[(int)AvaloniaKey.PageUp] = Key.PageUp;
            map[(int)AvaloniaKey.PageDown] = Key.PageDown;
            map[(int)AvaloniaKey.Home] = Key.Home;
            map[(int)AvaloniaKey.End] = Key.End;
            map[(int)AvaloniaKey.CapsLock] = Key.CapsLock;
            map[(int)AvaloniaKey.Scroll] = Key.ScrollLock;
            map[(int)AvaloniaKey.NumLock] = Key.NumLock;
            map[(int)AvaloniaKey.PrintScreen] = Key.PrintScreen;
            map[(int)AvaloniaKey.Pause] = Key.Pause;
            map[(int)AvaloniaKey.F1] = Key.F1;
            map[(int)AvaloniaKey.F2] = Key.F2;
            map[(int)AvaloniaKey.F3] = Key.F3;
            map[(int)AvaloniaKey.F4] = Key.F4;
            map[(int)AvaloniaKey.F5] = Key.F5;
            map[(int)AvaloniaKey.F6] = Key.F6;
            map[(int)AvaloniaKey.F7] = Key.F7;
            map[(int)AvaloniaKey.F8] = Key.F8;
            map[(int)AvaloniaKey.F9] = Key.F9;
            map[(int)AvaloniaKey.F10] = Key.F10;
            map[(int)AvaloniaKey.F11] = Key.F11;
            map[(int)AvaloniaKey.F12] = Key.F12;
            map[(int)AvaloniaKey.F13] = Key.F13;
            map[(int)AvaloniaKey.F14] = Key.F14;
            map[(int)AvaloniaKey.F15] = Key.F15;
            map[(int)AvaloniaKey.F16] = Key.F16;
            map[(int)AvaloniaKey.F17] = Key.F17;
            map[(int)AvaloniaKey.F18] = Key.F18;
            map[(int)AvaloniaKey.F19] = Key.F19;
            map[(int)AvaloniaKey.F20] = Key.F20;
            map[(int)AvaloniaKey.F21] = Key.F21;
            map[(int)AvaloniaKey.F22] = Key.F22;
            map[(int)AvaloniaKey.F23] = Key.F23;
            map[(int)AvaloniaKey.F24] = Key.F24;
            map[(int)AvaloniaKey.NumPad0] = Key.Keypad0;
            map[(int)AvaloniaKey.NumPad1] = Key.Keypad1;
            map[(int)AvaloniaKey.NumPad2] = Key.Keypad2;
            map[(int)AvaloniaKey.NumPad3] = Key.Keypad3;
            map[(int)AvaloniaKey.NumPad4] = Key.Keypad4;
            map[(int)AvaloniaKey.NumPad5] = Key.Keypad5;
            map[(int)AvaloniaKey.NumPad6] = Key.Keypad6;
            map[(int)AvaloniaKey.NumPad7] = Key.Keypad7;
            map[(int)AvaloniaKey.NumPad8] = Key.Keypad8;
            map[(int)AvaloniaKey.NumPad9] = Key.Keypad9;
            map[(int)AvaloniaKey.Decimal] = Key.KeypadDecimal;
            map[(int)AvaloniaKey.Divide] = Key.KeypadDivide;
            map[(int)AvaloniaKey.Multiply] = Key.KeypadMultiply;
            map[(int)AvaloniaKey.Subtract] = Key.KeypadSubtract;
            map[(int)AvaloniaKey.Add] = Key.KeypadAdd;
            map[(int)AvaloniaKey.Enter] = Key.KeypadEnter;
            //map[(int)AvaloniaKey.KeyPadEqual] = Key.KeypadEqual;
            map[(int)AvaloniaKey.LeftShift] = Key.ShiftLeft;
            //map[(int)AvaloniaKey.LeftControl] = Key.ControlLeft;
            map[(int)AvaloniaKey.LeftAlt] = Key.AltLeft;
            map[(int)AvaloniaKey.LWin] = Key.WinLeft;
            map[(int)AvaloniaKey.RightShift] = Key.ShiftRight;
            //map[(int)AvaloniaKey.RightControl] = Key.ControlRight;
            map[(int)AvaloniaKey.RightAlt] = Key.AltRight;
            map[(int)AvaloniaKey.RWin] = Key.WinRight;
            return map;
        }
    }
}