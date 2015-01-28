Aliaser
=======

This tool is built to bypass the restriction of the [DOSKEY](http://ss64.com/nt/doskey.html) utility and allows to set up a list of console aliases (macros) from the batch files. It calls the same WinAPI function as DOSKEY - [`AddConsoleAlias`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681935(v=vs.85).aspx). It means that alias (macros) definition rules are the same as in DOSKEY.

Command line parameters
-----------------------

Utility can accept a number of parameters:


    -f, --file          Required. Alias file name.

    -v, --verbose       (Default: True) Prints all messages to the standard
                        output.

    -e, --executable    (Default: CMD.exe) The name of the executable to assign
                        aliases to.

Alias file name format:
-----------------------

Please refer to the `alias-sample.txt` file in this repository.

Usage examples:
---------------

Add aliases to the current console from `myaliases.txt` file:

    aliaser -f myaliases.txt
    aliaser --file myaliases.txt --verbose False
