using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
	class Cradle
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
	}
}
