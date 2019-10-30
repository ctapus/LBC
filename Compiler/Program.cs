using System;

namespace Compiler
{
	class Program
	{
		static void Main(string[] args)
		{
			Cradle4 cradle = new Cradle4();
			cradle.Init();
			do
			{
				switch(cradle.Look)
				{
					case '?': cradle.Input();
						break;
					case '!': cradle.Output();
						break;
					default:
						cradle.Assignment();
						break;
				}
				cradle.NewLine();
			} while (cradle.Look != '.');
		}
	}
}
