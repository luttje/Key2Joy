@echo off

IF "%CI%"=="true" GOTO :EOF

SET LOCAL=%~dp0
set TARGET=%1
set BUILD_DIR=%2

IF "%TARGET%" == "Debug" goto skip

echo "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %BUILD_DIR% %LOCAL%..\bin\DocumentationFiles %LOCAL%..\Docs --filter markdown-doc"
IF EXIST "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe" %LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %BUILD_DIR% %LOCAL%..\bin\DocumentationFiles %LOCAL%..\Docs --filter markdown-doc

:skip
