using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping;

public interface IProvideReverseAspect
{
    /// <summary>
    /// Makes the given aspect the reverse of the current instance
    /// </summary>
    /// <param name="aspect"></param>
    /// <returns></returns>
    void MakeReverse(AbstractMappingAspect aspect);
}

public static class CommonReverseAspect
{
    public static void MakeReversePressState(AbstractMappingAspect current, AbstractMappingAspect aspect)
        => (aspect as IPressState).PressState = (current as IPressState).PressState == PressState.Press ? PressState.Release : PressState.Press;
}
