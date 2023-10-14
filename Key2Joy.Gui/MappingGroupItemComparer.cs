using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;

namespace Key2Joy.Gui;

public class MappingGroupItemComparer : IComparer<OLVListItem>
{
    private readonly OLVColumn primarySort;
    private readonly SortOrder primarySortOrder;

    public MappingGroupItemComparer(OLVColumn primarySort, SortOrder primarySortOrder)
    {
        this.primarySort = primarySort;
        this.primarySortOrder = primarySortOrder;
    }

    public int Compare(OLVListItem x, OLVListItem y)
    {
        var mappedOptionX = x.RowObject as MappedOption;
        var mappedOptionY = y.RowObject as MappedOption;

        var sortDirection = this.primarySortOrder == SortOrder.Ascending ? 1 : -1;

        if (typeof(CoreAction).IsAssignableFrom(this.primarySort.DataType))
        {
            return mappedOptionX.Action.CompareTo(mappedOptionY.Action) * sortDirection;
        }

        if (mappedOptionX.Trigger != null)
        {
            if (mappedOptionY.Trigger == null)
            {
                return 1 * sortDirection;
            }

            return mappedOptionX.Trigger.CompareTo(mappedOptionY.Trigger) * sortDirection;
        }

        return mappedOptionY.Trigger == null ? 0 : -1 * sortDirection;
    }
}