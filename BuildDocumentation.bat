@echo off

SET LOCAL=%~dp0
set TARGET=bin\Release
echo %LOCAL%bin\Release\Key2Joy\Key2Joy.xml
if exist %LOCAL%bin\Release\Key2Joy\Key2Joy.xml echo Key2Joy.xml exists
if not exist %LOCAL%bin\Release\Key2Joy\Key2Joy.xml echo Key2Joy.xml does not exist
%LOCAL%BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%bin\Release\Key2Joy\Key2Joy.xml %LOCAL%Docs --filter markdown-doc