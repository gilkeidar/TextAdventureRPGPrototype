using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Tools
{
	public static class ColorPrintouts //contains print functions in different colors depending on the usecase.
	{
		static StringBuilder sB = new StringBuilder();

		public static void StatePrint(string text, States state)
		{
			switch (state)
			{
				case States.Peaceful:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case States.Combat:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case States.Dead:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case States.GameMessage:
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					break;
				default:
					break;
			}
			Print(text);
			Console.ResetColor();
		}

		public static void ColorPrint(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Print(text);
			Console.ResetColor();
		}

		public static void Print(string text)
		{
			string[] words = text.Split(' ');
			sB.Clear();
			//solution inspired by Stack Overflow Answers: (in terms of using a string builder)
			/*foreach(string word in words)
			{
				if (word == words.Last())
				{
					sB.Append(word + " ");
					//Console.WriteLine("Printing Last Bit of Text!");
					Console.WriteLine(sB.ToString());
					break;
				}
				if (sB.Length + word.Length < Console.WindowWidth)
				{
					sB.Append(word + " ");
				}
				else
				{
					//Console.WriteLine("Printing!");
					Console.WriteLine(sB.ToString());
					sB.Clear();
					sB.Append(word + " ");
				}
			}*/

			foreach(string word in words)
			{
				if(sB.Length + word.Length + 1 < Console.WindowWidth) // + 1 accounts for the extra space between words
				{
					sB.Append(word + " ");
					if(word == words.Last())
					{
						Console.WriteLine(sB.ToString());
						break;
					}
				}
				else
				{
					Console.WriteLine(sB.ToString());
					sB.Clear();
					sB.Append(word + " ");
				}
			}

		}

	}
}
