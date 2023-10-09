@echo off

IF "%CI%"=="true" GOTO :EOF

SET LOCAL=%~dp0
set TARGET=%1
echo "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%..\bin\Release\Key2Joy\Key2Joy.xml %LOCAL%..\Docs --filter markdown-doc"
IF EXIST "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe" %LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%..\bin\Release\Key2Joy\Key2Joy.xml %LOCAL%..\Docs --filter markdown-doc
