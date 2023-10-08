namespace Key2Joy.Contracts.Mapping
{
    public enum MappingMenuVisibility
    {
        /// <summary>
        /// This action isn't visibile anywhere by default.
        /// 
        /// You should provide access to the action somewhere in a UserControl.
        /// </summary>
        Never = 0,

        /// <summary>
        /// This action is visible in any menu.
        /// </summary>
        Always = 1,

        /// <summary>
        /// This action is visible only in top level menu's.
        /// 
        /// You should provide access to the action somewhere in a UserControl.
        /// </summary>
        OnlyTopLevel = 2,

        /// <summary>
        /// This action isn't visibile in top level menu's.
        /// </summary>
        UnlessTopLevel = 3,
    }
}
