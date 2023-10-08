using System.Collections.Generic;
using BrightIdeasSoftware;

namespace Key2Joy.Gui;

public class MappingGroupComparer : IComparer<OLVGroup>
{
    public int Compare(OLVGroup x, OLVGroup y) => x.GroupId.CompareTo(y.GroupId);
}