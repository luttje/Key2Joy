using System;
using Key2Joy.Mapping;

namespace Key2Joy;

public interface IKey2JoyManager
{
    void CallOnUiThread(Action action);

    void ArmMappings(MappingProfile profile);

    bool GetIsArmed(MappingProfile profile = null);

    void DisarmMappings();
}
