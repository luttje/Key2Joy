using System.Reflection;
using Key2Joy.Contracts.Mapping.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Contracts.Mappings.Actions;

[TestClass]
public class ActionOptionsControl
{
    private void CheckMembers<T1, T2>(MemberInfo[] membersFromT1, MemberInfo[] membersFromT2)
    {
        Assert.AreEqual(membersFromT1.Length, membersFromT2.Length, $"Different count for members between {typeof(T1).Name} and {typeof(T2).Name}.");

        for (var i = 0; i < membersFromT1.Length; i++)
        {
            var memberFromT1 = membersFromT1[i];
            var memberFromT2 = membersFromT2[i];

            Assert.AreEqual(memberFromT1.Name, memberFromT2.Name, $"Different member names for members at index {i} between {typeof(T1).Name} and {typeof(T2).Name}.");

            if (memberFromT1 is MethodInfo methodFromT1 && memberFromT2 is MethodInfo methodFromT2)
            {
                Assert.AreEqual(
                    methodFromT1.GetParameters().Length,
                    methodFromT2.GetParameters().Length,
                    $"Different parameter count for methods named {methodFromT1.Name} between {typeof(T1).Name} and {typeof(T2).Name}."
                );
            }
        }
    }
    
    /// <summary>
    /// Test that IPluginActionOptionsControl and IActionOptionsControl have the same methods with the same
    /// parameter counts only difference is that IPluginActionOptionsControl has PluginAction as the type
    /// where IActionOptionsControl has AbstractAction
    /// </summary>
    [TestMethod]
    public void IPluginActionOptionsControl_And_IActionOptionsControl_HaveSameMembers()
    {
        // Check Methods
        this.CheckMembers<IPluginActionOptionsControl, IActionOptionsControl>(
            typeof(IPluginActionOptionsControl).GetMethods(),
            typeof(IActionOptionsControl).GetMethods()
        );

        // Check Properties
        this.CheckMembers<IPluginActionOptionsControl, IActionOptionsControl>(
            typeof(IPluginActionOptionsControl).GetProperties(),
            typeof(IActionOptionsControl).GetProperties()
        );

        // Check Events
        this.CheckMembers<IPluginActionOptionsControl, IActionOptionsControl>(
            typeof(IPluginActionOptionsControl).GetEvents(),
            typeof(IActionOptionsControl).GetEvents()
        );
    }
}
