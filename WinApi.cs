using System.Runtime.InteropServices;
using System.Text;

namespace Aliaser
{
	internal class WinApi
	{
		/// <summary>
		/// Retrieves the text for the specified console alias and executable.
		/// </summary>
		/// <param name="lpSource">The console alias whose text is to be retrieved.</param>
		/// <param name="lpTargetBuffer">A pointer to a buffer that receives the text associated with the console alias.</param>
		/// <param name="targetBufferLength">The size of the buffer pointed to by <paramref name="lpTargetBuffer"/>, in bytes.</param>
		/// <param name="lpExeName">The name of the executable file.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.
		/// </returns>
		[DllImport("kernel32", SetLastError = true)]
		internal static extern bool GetConsoleAlias(
		string lpSource,
		out StringBuilder lpTargetBuffer,
		uint targetBufferLength,
		string lpExeName);

		/// <summary>
		/// Retrieves the required size for the buffer used by the GetConsoleAliasExes function.
		/// </summary>
		/// <returns>The size of the buffer required to store the names of all executable files that have console aliases defined, in bytes.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern uint GetConsoleAliasExesLength();

		/// <summary>
		/// Retrieves the names of all executable files with console aliases defined
		/// </summary>
		/// <param name="lpExeNameBuffer">A pointer to a buffer that receives the names of the executable files.</param>
		/// <param name="exeNameBufferLength">The size of the buffer pointed to by <paramref name="lpExeNameBuffer"/>, in bytes.</param>
		/// <remarks>
		/// To determine the required size for the lpExeNameBuffer buffer, use the GetConsoleAliasExesLength function.
		/// </remarks>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32", SetLastError = true)]
		internal static extern uint GetConsoleAliasExes(
		out StringBuilder lpExeNameBuffer,
		uint exeNameBufferLength);

		/// <summary>
		/// Retrieves the required size for the buffer used by the GetConsoleAliases function.
		/// </summary>
		/// <param name="lpExeName">The name of the executable file whose console aliases are to be retrieved.</param>
		/// <returns>The size of the buffer required to store all console aliases defined for this executable file, in bytes.</returns>
		[DllImport("kernel32", SetLastError = true)]
		internal static extern uint GetConsoleAliasesLength(string lpExeName);

		/// <summary>
		/// Retrieves all defined console aliases for the specified executable.
		/// </summary>
		/// <param name="lpAliasBuffer">
		/// A pointer to a buffer that receives the aliases.
		/// The format of the data is as follows: Source1=Target1\0Source2=Target2\0... SourceN=TargetN\0, where N is the number of console aliases defined.
		/// </param>
		/// <param name="aliasBufferLength">The size of the buffer pointed to by <paramref name="lpAliasBuffer"/>, in bytes.</param>
		/// <param name="lpExeName">The executable file whose aliases are to be retrieved.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("Kernel32.dll")]
		internal static extern uint GetConsoleAliases(
		StringBuilder[] lpAliasBuffer,
		uint aliasBufferLength,
		string lpExeName);

		/// <summary>
		/// Defines a console alias for the specified executable.
		/// </summary>
		/// <param name="source">The console alias to be mapped to the text specified by <paramref name="target"/>.</param>
		/// <param name="target">The text to be substituted for <paramref name="source"/>. If this parameter is NULL, then the console alias is removed.</param>
		/// <param name="exeName">The name of the executable file for which the console alias is to be defined.</param>
		/// <returns>
		/// If the function succeeds, the return value is TRUE.
		/// If the function fails, the return value is FALSE. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32", SetLastError = true)]
		internal static extern bool AddConsoleAlias(string source, string target, string exeName);
	}
}