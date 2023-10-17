set PluginProjectName=%1

set PluginPath=%2
set PluginOutputPath=%3

:: If the PluginOutputPath is empty
if "%PluginOutputPath%"=="" (
    call set PluginOutputPath=%%PluginPath:%PluginProjectName%=Key2Joy.Gui%%
)

echo Copying Plugin (%1) to %PluginOutputPath%
echo d | xcopy /s /e /d /y %PluginPath% %PluginOutputPath%Plugins\%PluginProjectName%
