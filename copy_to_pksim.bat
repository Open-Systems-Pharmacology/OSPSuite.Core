@echo off

set SolutionDir=%~d0%~p0
set SrcDir=%SolutionDir%src\
set AppDebugDir=%SolutionDir%..\PK-Sim\src\PKSim\bin\Debug

copy "%SrcDir%OSPSuite.UI\bin\debug\OSPSuite*.dll" "%AppDebugDir%"
copy "%SrcDir%OSPSuite.UI\bin\debug\OSPSuite*.pdb" "%AppDebugDir%"
pause