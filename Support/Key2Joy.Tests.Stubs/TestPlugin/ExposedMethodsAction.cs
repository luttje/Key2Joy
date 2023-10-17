using System;
using System.Text;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Tests.Stubs.TestPlugin;

[Action(
    Description = "ExposedMethodAction",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "ExposedMethodAction"
)]
public class ExposedMethodsAction : PluginAction
{
    [ExposesScriptingMethod("simpleConcat")]
    public string DoScriptAction_Concat(string input, int[] numbers)
        => Concat(input, numbers);

    public static string Concat(string input, int[] numbers)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(input);

        foreach (var number in numbers)
        {
            stringBuilder.AppendLine(number.ToString());
        }

        return stringBuilder.ToString();
    }

    [ExposesScriptingMethod("isUnlessTopLevel")]
    public bool DoScriptAction_IsEnum(MappingMenuVisibility visibilityEnum)
        => visibilityEnum == MappingMenuVisibility.UnlessTopLevel;

    [ExposesScriptingMethod("returnsNumbersInput")]
    public int[] DoScriptAction_ReturnsNumbers(int[] numbers)
        => numbers;
}
