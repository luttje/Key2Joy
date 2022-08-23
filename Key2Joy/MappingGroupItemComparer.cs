using BrightIdeasSoftware;
using Key2Joy.Mapping;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Key2Joy
{
    internal class MappingGroupItemComparer : IComparer<OLVListItem>
    {
        private OLVColumn primarySort;
        private SortOrder primarySortOrder;

        public MappingGroupItemComparer(OLVColumn primarySort, SortOrder primarySortOrder)
        {
            this.primarySort = primarySort;
            this.primarySortOrder = primarySortOrder;
        }

        public int Compare(OLVListItem x, OLVListItem y)
        {
            var mappedOptionX = x.RowObject as MappedOption;
            var mappedOptionY = y.RowObject as MappedOption;

            var sortDirection = primarySortOrder == SortOrder.Ascending ? 1 : -1;

            if (typeof(BaseAction).IsAssignableFrom(primarySort.DataType))
            {
                return mappedOptionX.Action.CompareTo(mappedOptionY.Action) * sortDirection;
            }

            if(mappedOptionX.Trigger != null)
            {
                return mappedOptionX.Trigger.CompareTo(mappedOptionY.Trigger) * sortDirection;
            }

            return 1 * sortDirection;
        }
    }
}