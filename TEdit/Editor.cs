using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TEdit
{
	public class Editor
	{
		#region Data
		public static string path = "";
		public static char[,] content = new char[4096, 4096];
		public static ConsoleColor Main = ConsoleColor.Black;
		public static ConsoleColor Secoundary = ConsoleColor.DarkGray;
		public static ConsoleColor Text = ConsoleColor.White;
		public static ConsoleColor Active = ConsoleColor.Gray;
		public static ConsoleColor ActiveText = ConsoleColor.Black;
		private static int _OldCurX = 1;
		private static int _OldCurY = 1;
		private static int _CursorX = 0;
		private static int _CursorY = 0;
		#endregion
		public static void Run()
		{
			Console.WindowHeight = 25;
			Console.WindowWidth = 80;
			DrawWindow();
			Load();
			Console.CursorVisible = false;
			bool needsredraw = true;
			while (true)
			{
				if (needsredraw)
				{
                    WriteText((_CursorX / 23) * 23, (_CursorY / 77) * 77);
                    SetCursor(0, 0);
                }
                DrawCursor();
                if ((_CursorX % 23 == 0 && _CursorX != 0) || (_CursorY % 77 == 0 && _CursorY != 0))
                {
                    DrawWindow();
                }
				needsredraw = false;
				_OldCurX = _CursorX;
				_OldCurY = _CursorY;
                ConsoleKeyInfo keyi = Console.ReadKey(true);
				ConsoleKey key = keyi.Key;
                #region Cursor
                if (key == ConsoleKey.UpArrow && _CursorX != 0)
				{
					_CursorX--;
				}
				if (key == ConsoleKey.DownArrow && _CursorX != 4095)
				{
					_CursorX++;
				}
                if (key == ConsoleKey.LeftArrow && _CursorY != 0)
                {
                    _CursorY--;
                }
                if (key == ConsoleKey.RightArrow && _CursorY != 4095)
                {
                    _CursorY++;
                }
				#endregion
				needsredraw = IsOnEdge(_CursorX,_CursorY,_OldCurX,_OldCurY);
                if (key == ConsoleKey.Spacebar && _CursorY != 4095)
				{
                    content[_CursorX, _CursorY] = ' ';
					_CursorY++;
				}
                if (key == ConsoleKey.Backspace && _CursorY != 0)
                {
                    _CursorY--;
                    content[_CursorX, _CursorY] = ' ';
                }
                if (key == ConsoleKey.Enter && _CursorX != 4096)
                {
                    _CursorY = 0;
					_CursorX++;
                }
				if (key == ConsoleKey.Home)
				{
					_CursorY = 0;
					_CursorX = 0;
				}
				if (key == ConsoleKey.Escape)
				{

				}
                if (key == ConsoleKey.F5)
				{
					needsredraw = true;
				}
				if (char.IsNumber(keyi.KeyChar) || char.IsLetter(keyi.KeyChar))
				{
					content[_CursorX,_CursorY] = keyi.KeyChar;
					_CursorY++;
					//ResetWindow();
				}
			}
		}

		#region Aditional Functions
		private static void DrawStatusBar()
		{
			Console.ForegroundColor = Text;
			Console.BackgroundColor = Secoundary;
			SetCursor(24, 0);
			Console.Write("                                                                                ");
			SetCursor(24, 1);
			Console.Write("Line: " + _CursorX + " Col: " + _CursorY);
			SetCursor(0, 0);
		}
		private static void Load()
		{
			if (!File.Exists(path) || (path == ""))
			{
				return;
			}
			string[] temp = File.ReadAllLines(path);
			for (int line = 0; line < temp.Length; line++)
			{
				char[] chrline = temp[line].ToCharArray();
				for (int index = 0; index < chrline.Length; index++)
				{
					content[line,index] = chrline[index];
				}
			}
		}
		private static void DrawWindow()
		{
			SetCursor(0, 0);
			Console.ForegroundColor = Text;
			Console.BackgroundColor = Main;
			Console.Clear();
			Console.BackgroundColor = Secoundary;
			Console.WriteLine("                                                                                ");
			SetCursor(0, 40 - path.Length / 2);
			Console.BackgroundColor = Active;
			Console.ForegroundColor = ActiveText;
			Console.Write(path);
			Console.ForegroundColor = Text;
			Console.BackgroundColor = Secoundary;
			for (int i = 1; i < 24; i++)
			{
				SetCursor(i,0);
				Console.Write(" ");
				SetCursor(i, 79);
				Console.Write(" ");
			}
			DrawStatusBar();
		}
		private static void WriteText(int x, int y)
		{
			Console.BackgroundColor = Main;
			Console.ForegroundColor = Text;
			for (int i = 0; i < 23; i++)
			{
				for (int j = 0; j < 78; j++)
				{
					SetCursor(i + 1, j + 1);
					string t = content[i + x, j + y].ToString();
					if (t != null)
					{
						Console.Write(content[i + x, j + y]);
					}
				}
			}
			DrawStatusBar();
		}
		private static void SetCursor(int x, int y)
		{
			Console.CursorTop = x;
			Console.CursorLeft = y;
		}
		private static void DrawCursor()
		{
            SetCursor((_CursorX + 1) % 23, (_CursorY + 1) % 77);
            Console.BackgroundColor = Active;
            Console.ForegroundColor = ActiveText;
            Console.Write(content[_CursorX, _CursorY]);
            Console.BackgroundColor = Main;
            Console.ForegroundColor = Text;
            SetCursor((_OldCurX + 1) % 23, (_OldCurY + 1) % 77);
            Console.Write(content[_OldCurX, _OldCurY]);
            SetCursor(0, 0);
        }
		private static void ResetWindow()
		{
			Console.WindowHeight = 25;
			Console.WindowWidth = 80;
			DrawWindow();
			WriteText(_CursorX, 0);
			DrawCursor();
		}
		private static bool IsOnEdge(int x, int y, int ox, int oy)
		{
			if (x == 0 && ox == 23 || y == 0 && oy == 23)
            {
				return true;
			}
            return false;
		}
		#endregion
	}
}
