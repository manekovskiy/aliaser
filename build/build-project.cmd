:: Calling msbuild depending on platform. 
:: Using Rebuild target of project.
:: %1 is build configuration name. Either Debug or Release
:: %2 is full path to *.csproj file. Parameter should be properly escaped
:: %3 is output directory where built binaries should be placed
:: %4 is optional parameter that should be equal to CLEANOUTPUT. If this parameter is set then output directory will be deleted before build

:: Get path for MSBUILD
if defined PROGRAMFILES(x86) (
	set _MSBUILDDIR=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319
) else (
	set _MSBUILDDIR=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
)

set _CONFIGURATION=%1
set _PROJECT=%2
set _OUTPUTPATH=%3
set _OUTPUTPATH_TMP=%3_tmp

if [%4]==[CLEANOUTPUT] (
	if exist %_OUTPUTPATH%	@rmdir /s /q %_OUTPUTPATH%
)

:: Build project
%_MSBUILDDIR%\msbuild.exe %_PROJECT% /property:Configuration=%_CONFIGURATION%;Platform=x86;OutputPath=%_OUTPUTPATH_TMP%;Warn=0 /t:ReBuild /verbosity:quiet

:: Create output folder if it does not exist
if not exist %_OUTPUTPATH% mkdir %_OUTPUTPATH%

:: Get the name for the merged assembly
for %%a in ("%_PROJECT%") do set _MERGED_ASSEMBLY_NAME=%%~na

:: Combine assemblies
call "%TOOLS_ROOT%\ILMerge.exe" /out:"%_OUTPUTPATH%\%_MERGED_ASSEMBLY_NAME%.exe" "%_OUTPUTPATH_TMP%\*.exe" "%_OUTPUTPATH_TMP%\*.dll" /wildcards

:: Remove temporary output folder
rmdir /S /Q %_OUTPUTPATH_TMP%