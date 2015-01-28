@echo OFF

call setup-env.cmd
call "%ROOT%\build\build-project.cmd" DEBUG "%SRC_ROOT%\aliaser.csproj" "%ROOT%\bin"