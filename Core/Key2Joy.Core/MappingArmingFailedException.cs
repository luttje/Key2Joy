using System;

namespace Key2Joy;

public class MappingArmingFailedException : Exception
{
    public MappingArmingFailedException(string message) : base(message)
    {
    }
}
