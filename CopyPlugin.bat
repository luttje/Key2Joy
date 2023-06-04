set PluginProjectName=%1
set PluginFileName=%3

set PluginPath=%2
set PluginOutputPath=%2
call set PluginOutputPath=%%PluginOutputPath:%PluginProjectName%=Key2Joy.Gui%%

echo Copying Plugin (%1) to %PluginOutputPath%
echo f | xcopy /f /y %PluginPath%%PluginFileName% %PluginOutputPath%Plugins\%PluginFileName%