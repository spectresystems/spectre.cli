using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Cli.Testing.Fakes
{
    public sealed class TestableConsoleInput : IAnsiConsoleInput
    {
        private readonly Queue<ConsoleKeyInfo> _input;

        public TestableConsoleInput()
        {
            _input = new Queue<ConsoleKeyInfo>();
        }

        public void PushText(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            foreach (var character in input)
            {
                PushCharacter(character);
            }

            PushKey(ConsoleKey.Enter);
        }

        public void PushCharacter(char character)
        {
            var control = char.IsUpper(character);
            _input.Enqueue(new ConsoleKeyInfo(character, (ConsoleKey)character, false, false, control));
        }

        public void PushKey(ConsoleKey key)
        {
            _input.Enqueue(new ConsoleKeyInfo((char)key, key, false, false, false));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (_input.Count == 0)
            {
                throw new InvalidOperationException("No input available.");
            }

            return _input.Dequeue();
        }
    }

    public sealed class TestableCursor : IAnsiConsoleCursor
    {
        public void Move(CursorDirection direction, int steps)
        {
        }

        public void SetPosition(int column, int line)
        {
        }

        public void Show(bool show)
        {
        }
    }

    public sealed class FakeConsole : IAnsiConsole, IDisposable
    {
        public Capabilities Capabilities { get; }
        public Encoding Encoding { get; }
        public IAnsiConsoleCursor Cursor => new TestableCursor();
        public TestableConsoleInput Input { get; }

        public int Width { get; }
        public int Height { get; }

        IAnsiConsoleInput IAnsiConsole.Input => Input;
        public RenderPipeline Pipeline { get; }

        public Decoration Decoration { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public string Link { get; set; }

        public StringWriter Writer { get; }
        public string Output => Writer.ToString();
        public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

        public FakeConsole(
            int width = int.MaxValue, int height = 9000, Encoding encoding = null,
            bool supportsAnsi = true, ColorSystem colorSystem = ColorSystem.Standard,
            bool legacyConsole = false, bool interactive = true)
        {
            Capabilities = new Capabilities(supportsAnsi, colorSystem, legacyConsole, interactive);
            Encoding = encoding ?? Encoding.UTF8;
            Width = width;
            Height = height;
            Writer = new StringWriter();
            Input = new TestableConsoleInput();
            Pipeline = new RenderPipeline();
        }

        public void Dispose()
        {
            Writer.Dispose();
        }

        public void Clear(bool home)
        {
        }

        public void Write(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                return;
            }

            foreach (var segment in segments)
            {
                Writer.Write(segment.Text);
            }
        }
    }
}
