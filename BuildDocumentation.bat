@echo off

SET LOCAL=%~dp0
set TARGET=bin\Release
%LOCAL%BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%KeyToJoy\%TARGET%\KeyToJoy.xml %LOCAL%Docs --filter markdown-doc