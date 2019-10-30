using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
	class Cradle4
	{
		/// <summary>
		/// Lookahead character
		/// </summary>
		public char Look;
		/// <summary>
		/// Read new char from the input stream
		/// </summary>
		/// <returns></returns>
		private void GetChar()
		{
			Look = Console.ReadKey().KeyChar;
		}
		private void Error(string s)
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(s);
			Console.ResetColor();
		}
		private void Abort(string s)
		{
			Error(s);
			Environment.Exit(-1);
		}
		private void Expected(string s)
		{
			Abort(s + " Expected");
		}
		private void Match(char x)
		{
			if (Look == x) GetChar();
			else Expected("'" + x + "'");
		}
		private bool IsAlpha(char c)
		{
			return Regex.IsMatch(c.ToString(), "[a-zA-Z]");
		}
		private bool IsDigit(char c)
		{
			return Regex.IsMatch(c.ToString(), "[0-9]");
		}
		private bool IsAlNum(char c)
		{
			return IsAlpha(c) || IsDigit(c);
		}
		private bool IsAddop(char c)
		{
			return Regex.IsMatch(c.ToString(), "[+,-]");
		}
		private char GetName()
		{
			if (!IsAlpha(Look)) Expected("'Name'");
			char c = Look;
			GetChar();
			return c;
		}
		private int GetNum()
		{
			int value = 0;
			if (!IsDigit(Look)) Expected("'Integer'");
			while(IsDigit(Look))
			{
				value = 10 * value + Look - '0';
				GetChar();
			}
			return value;
		}
		private void Emit(string s)
		{
			Console.Write("\t" + s);
		}
		private void EmitLn(string s)
		{
			Console.WriteLine();
			Console.WriteLine("\t" + s);
		}
		public void NewLine()
		{
			if (Look == '\r')
			{
				GetChar();
				//if (Look == '\n')
				//	GetChar();
			}
		}
		public void Init()
		{
			GetChar();
		}
		public void Assignment()
		{
			char name = GetName();
			Match('=');
			Table[name - 'a'] = Expression();
		}
		private int Factor()
		{
			int value = 0;
			if(Look == '(')
			{
				Match('(');
				value = Expression();
				Match(')');
			}
			else
			{
				if (IsAlpha(Look))
					value = Table[GetName() - 'a'];// e.g. Table['x']
				else  value = GetNum();
			}
			return value;
		}
		private int Term()
		{
			int value = Factor();
			while(Regex.IsMatch(Look.ToString(), "[*,/]"))
			{
				switch(Look)
				{
					case '*': Match('*');
						value *= Factor();
						break;
					case '/': Match('/');
						value /= Factor();
						break;
				}
			}
			return value;
		}
		private int Expression()
		{
			int value = 0;
			if(IsAddop(Look))
			{
				value = 0;
			}
			else
			{
				value = Term();
			}
			while(IsAddop(Look))
			{
				switch(Look)
				{
					case '+': Match('+');
						value += Term();
						break;
					case '-': Match('-');
						value -= Term();
						break;
				}
			}
			return value;
		}
		public int[] Table = new int['z' - 'a'];// holds 26 variables of 1 char name
		public void Input()
		{
			Match('?');
			Table[GetName() - 'a'] = Convert.ToInt32(Console.ReadLine());
		}
		public void Output()
		{
			Match('!');
			Console.WriteLine(Table[GetName() - 'a']);
		}
	}
}
