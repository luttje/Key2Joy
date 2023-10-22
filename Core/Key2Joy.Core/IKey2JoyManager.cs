using System;
using Key2Joy.Mapping;

namespace Key2Joy;

public interface IKey2JoyManager
{
    void CallOnUiThread(Action action);

    /// <summary>
    /// Arms the mapping options so the triggers cause the actions to be executed.
    /// </summary>
    /// <param name="profile"></param>
    /// <exception cref="MappingArmingFailedException">Occurs when an illegal configuration can't be started</exception>
    void ArmMappings(MappingProfile profile);

    bool GetIsArmed(MappingProfile profile = null);

    void DisarmMappings();
}
