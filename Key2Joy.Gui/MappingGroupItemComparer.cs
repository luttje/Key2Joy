using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Key2Joy.Contracts.Mapping;
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
        var mappedOptionX = x.RowObject as AbstractMappedOption;
        var mappedOptionY = y.RowObject as AbstractMappedOption;

        var sortDirection = this.primarySortOrder == SortOrder.Ascending ? 1 : -1;

        // Check for parent-child relationships
        if (mappedOptionX.IsChildOf(mappedOptionY))
        {
            return 1 * sortDirection;
        }
        else if (mappedOptionY.IsChildOf(mappedOptionX))
        {
            return -1 * sortDirection;
        }

        // If both mapped options are children of different parents, sort based on their parents
        if (mappedOptionX.IsChild
            && mappedOptionY.IsChild
            && mappedOptionX.Parent != mappedOptionY.Parent)
        {
            return this.DefaultSorting(mappedOptionX.Parent, mappedOptionY.Parent) * sortDirection;
        }

        // If only mappedOptionX is a child, sort it relative to mappedOptionY's position
        if (mappedOptionX.IsChild && !mappedOptionY.IsChild)
        {
            return this.DefaultSorting(mappedOptionX.Parent, mappedOptionY) * sortDirection;
        }

        // If only mappedOptionY is a child, sort it relative to mappedOptionX's position
        if (mappedOptionY.IsChild && !mappedOptionX.IsChild)
        {
            return this.DefaultSorting(mappedOptionX, mappedOptionY.Parent) * sortDirection;
        }

        // If no parent-child relationship or siblings, sort based on triggers/actions
        return this.DefaultSorting(mappedOptionX, mappedOptionY) * sortDirection;
    }

    private int DefaultSorting(AbstractMappedOption mappedOptionX, AbstractMappedOption mappedOptionY)
    {
        if (typeof(CoreAction).IsAssignableFrom(this.primarySort.DataType))
        {
            return mappedOptionX.Action.CompareTo(mappedOptionY.Action);
        }

        if (mappedOptionX.Trigger != null)
        {
            if (mappedOptionY.Trigger == null)
            {
                return 1;
            }

            return mappedOptionX.Trigger.CompareTo(mappedOptionY.Trigger);
        }

        return mappedOptionY.Trigger == null ? 0 : -1;
    }
}
