@echo off

IF "%CI%"=="true" GOTO :EOF

SET LOCAL=%~dp0
set TARGET=Release
echo "%LOCAL%Support\BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\net48\BuildMarkdownDocs.exe %LOCAL%bin\Release\Key2Joy\Key2Joy.xml %LOCAL%Docs --filter markdown-doc"
%LOCAL%Support\BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\net48\BuildMarkdownDocs.exe %LOCAL%bin\Release\Key2Joy\Key2Joy.xml %LOCAL%Docs --filter markdown-doc
