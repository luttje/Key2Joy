set PluginProjectName=%1

set PluginPath=%2
set PluginOutputPath=%2
call set PluginOutputPath=%%PluginOutputPath:%PluginProjectName%=Key2Joy.Gui%%

echo Copying Plugin (%1) to %PluginOutputPath%
echo d | xcopy /d /y %PluginPath% %PluginOutputPath%Plugins\%PluginProjectName%