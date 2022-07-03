@echo off

SET LOCAL=%~dp0
set TARGET=bin\Release
%LOCAL%BuildMarkdownDocs\%TARGET%\BuildMarkdownDocs.exe %LOCAL%Key2Joy\%TARGET%\Key2Joy.xml %LOCAL%Docs --filter markdown-doc