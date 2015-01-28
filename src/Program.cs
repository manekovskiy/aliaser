using System;
using System.IO;
using System.Security;
using CommandLine;
using CommandLine.Text;

namespace Aliaser
{
	internal class Options
	{
		[Option('f', "file", Required = true, HelpText = "Alias file name.")]
		public string AliasFile { get; set; }

		[Option('v', "verbose", Required = false, DefaultValue = true, HelpText = "Prints all messages to the standard output.")]
		public bool Verbose { get; set; }

		[Option('e', "executable", Required = false, DefaultValue = "CMD.exe", HelpText = "The name of the executable to assign aliases to.")]
		public string ExecutableName { get; set; }

		[ParserState]
		public IParserState ParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}

	class Program
	{
		private static Options Options { get; set; }

		static void Main(string[] args)
		{
			if (!TryParseOptions(args)) return;

			string[] lines;
			if (!TryReadAliasFileLines(out lines)) return;

			for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
			{
				var line = lines[lineNumber].Trim();

				Tuple<string, string> parsedLine;
				if (!TryParseLine(line, lineNumber, out parsedLine)) continue;

				var alias = parsedLine.Item1;
				var command = parsedLine.Item2;
				if (!WinApi.AddConsoleAlias(alias, command, Options.ExecutableName))
				{
					WriteLine("Failed to add alias '{0}' to the command '{1}'.", alias, command);
				}
				else
				{
					WriteLine("Successfully added alias '{0}' to the command '{1}'.", alias, command);
				}
			}
		}

		private static bool TryParseOptions(string[] args)
		{
			Options = new Options();
			if (!CommandLine.Parser.Default.ParseArguments(args, Options)) return false;

			if (!File.Exists(Options.AliasFile))
			{
				Console.WriteLine("The aliases file '{0}' could not be found.", Options.AliasFile);
				return false;
			}

			return true;
		}

		private static bool TryReadAliasFileLines(out string[] lines)
		{
			lines = null;
			try
			{
				lines = File.ReadAllLines(Options.AliasFile);
			}
			catch (Exception e)
			{
				if (e is UnauthorizedAccessException || e is SecurityException)
				{
					Console.WriteLine("Cannot read alias file. Application has no permissions required. Error message: {0}", e.Message);
					return false;
				}

				Console.WriteLine("Unhandled exception occured while reading alias file. Error message: {0}", e.Message);
				return false;
			}

			return true;
		}

		private static bool TryParseLine(string line, int lineNumber, out Tuple<string, string> parsedLine)
		{
			parsedLine = null;

			// If line starts with '#' character then it is treated as a comment, i.e. not processed.
			// If line is empty or whitespace string it is not processed.
			if (line.StartsWith("#") || string.IsNullOrEmpty(line)) return false;

			var firstSpaceCharacterIndex = line.IndexOf(' ');
			if (firstSpaceCharacterIndex < 0)
			{
				WriteLine("Invalid value at line {0}. Alias and associated command must be delimited with a one or more space characters. Line omitted.", lineNumber);
				return false;
			}

			var alias = line.Substring(0, firstSpaceCharacterIndex).Trim();
			var command = line.Substring(firstSpaceCharacterIndex).Trim();

			if (string.IsNullOrEmpty(command))
			{
				WriteLine("Invalid command at line {0}. Alias is present but command could not be an empty string. Line omitted.", lineNumber);
				return false;
			}

			parsedLine = new Tuple<string, string>(alias, command);

			return true;
		}

		private static void WriteLine(string format, params object[] arguments)
		{
			if (Options.Verbose)
			{
				Console.WriteLine(format, arguments);
			}
		}
	}
}