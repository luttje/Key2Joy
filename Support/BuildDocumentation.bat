@echo off

if "%CI%"=="true" goto :EOF

set LOCAL=%~dp0
set TARGET=%1
set BUILD_DIR=%2

if "%TARGET%" == "Debug" goto skip

echo "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %BUILD_DIR% %LOCAL%..\bin\DocumentationFiles %LOCAL%..\Docs --filter markdown-doc"
if exist "%LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe" %LOCAL%BuildMarkdownDocs\bin\BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %BUILD_DIR% %LOCAL%..\bin\DocumentationFiles %LOCAL%..\Docs --filter markdown-doc

:skip
