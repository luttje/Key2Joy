namespace Key2Joy.Contracts.Mapping
{
    public class ActionAttribute : MappingAttribute
    {
        /// <summary>
        /// Group the actions should be categorized under.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Image for the group the actions should be categorized under.
        /// </summary>
        public string GroupImage { get; set; }
    }
}
