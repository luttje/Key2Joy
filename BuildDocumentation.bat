@echo off

IF "%CI%"=="true" GOTO :EOF

SET LOCAL=%~dp0
set TARGET=bin\Release
%LOCAL%Support\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%bin\Release\Key2Joy\Key2Joy.xml %LOCAL%Docs --filter markdown-doc
