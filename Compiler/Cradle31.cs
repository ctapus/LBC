using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
	class Cradle31
	{
		/// <summary>
		/// Lookahead character
		/// </summary>
		private char Look;
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
		private char GetNum()
		{
			if (!IsDigit(Look)) Expected("'Integer'");
			char c = Look;
			GetChar();
			return c;
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
		public void Init()
		{
			GetChar();
		}
		private void Ident()
		{
			char name = GetName();
			if(Look == '(')
			{
				Match('(');
				Match(')');
				EmitLn("bsr " + name);
			}
			else
			{
				EmitLn("mov " + name + ", eax");
			}
		}
		private void Factor()
		{
			if (Look == '(')
			{
				Match('(');
				Expression();
				Match(')');
			}
			else
			{
				if(IsAlpha(Look))
				{
					EmitLn("mov eax " + GetName());
				}
				else
				{
					EmitLn("mov eax " + GetNum());
				}
			}
		}
		private void Multiply()
		{
			Match('*');
			Factor();
			EmitLn("pop ebx");
			EmitLn("mul eax, ebx");
		}
		private void Divide()
		{
			Match('/');
			Factor();
			EmitLn("pop ebx");
			EmitLn("div eax, ebx");
		}
		private void Term()
		{
			Factor();
			while (Array.IndexOf(new[] { '*', '/' }, Look) > -1)
			{
				EmitLn("push eax");
				switch (Look)
				{
					case '*':
						Multiply();
						break;
					case '/':
						Divide();
						break;
				}
			}
		}
		private void Add()
		{
			Match('+');
			Term();
			EmitLn("pop ebx");
			EmitLn("add eax, ebx");
		}
		private void Substract()
		{
			Match('-');
			Term();
			EmitLn("pop ebx");
			EmitLn("sub eax, ebx");
			EmitLn("neg eax");
		}
		private void Expression()
		{
			if (IsAddop(Look))
			{
				EmitLn("mov eax, 0");
			}
			else
			{
				Term();
			}
			while (IsAddop(Look))
			{
				EmitLn("push eax");
				switch (Look)
				{
					case '+':
						Add();
						break;
					case '-':
						Substract();
						break;
				}
			}
		}
		public void Assignment()
		{
			char name = GetName();
			Match('=');
			Expression();
			EmitLn("lea " + name + ", eax");
		}
	}
}
//< factor >::=< number > |(< expression >)| < variable >
//< Ident >=< Expression >