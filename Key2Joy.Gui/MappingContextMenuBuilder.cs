using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using BrightIdeasSoftware;
using Key2Joy.Mapping;
using System.Windows.Forms;
using System.Collections;
using CommandLine;
using Key2Joy.Gui.Properties;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Mapping;
using System.Collections.Generic;

namespace Key2Joy.Gui;

internal class SelectEditMappingEventArgs : EventArgs
{
    public MappedOption MappedOption { get; }

    public SelectEditMappingEventArgs(MappedOption mappedOption)
        => this.MappedOption = mappedOption;
}

internal class SelectRemoveMappingsEventArgs : EventArgs
{
    public SelectRemoveMappingsEventArgs()
    { }
}

internal class SelectMakeMappingParentlessEventArgs : EventArgs
{
    public MappedOption MappedOption { get; }

    public SelectMakeMappingParentlessEventArgs(MappedOption mappedOption)
        => this.MappedOption = mappedOption;
}

internal class SelectChooseNewParentEventArgs : EventArgs
{
    public MappedOption MappedOption { get; }
    public MappedOption NewParent { get; }

    public SelectChooseNewParentEventArgs(MappedOption mappedOption, MappedOption newParent)
    {
        this.MappedOption = mappedOption;
        this.NewParent = newParent;
    }
}

internal class SelectMultiEditMappingEventArgs : EventArgs
{
    /// <summary>
    /// The property to be editted
    /// </summary>
    public PropertyInfo Property { get; }

    /// <summary>
    /// The trigger or actions to edit
    /// </summary>
    public IList<AbstractMappingAspect> MappingAspects { get; }

    /// <summary>
    /// The value entered by the user
    /// </summary>
    public object Value { get; }

    public SelectMultiEditMappingEventArgs(PropertyInfo property, IList<AbstractMappingAspect> mappingAspects, object value)
    {
        this.Property = property;
        this.MappingAspects = mappingAspects;
        this.Value = value;
    }
}

internal class MappingContextMenuBuilder
{
    public event EventHandler<SelectEditMappingEventArgs> SelectEditMapping;

    public event EventHandler<SelectRemoveMappingsEventArgs> SelectRemoveMappings;

    public event EventHandler<SelectMakeMappingParentlessEventArgs> SelectMakeMappingParentless;

    public event EventHandler<SelectChooseNewParentEventArgs> SelectChooseNewParent;

    public event EventHandler<SelectMultiEditMappingEventArgs> SelectMultiEditMapping;

    private readonly ContextMenuStrip menu;
    private readonly IList selectedItems;
    private MappedOption currentChildChoosingParent = null;

    internal MappingContextMenuBuilder(IList selectedItems)
    {
        this.selectedItems = selectedItems;
        this.menu = new();
    }

    /// <summary>
    /// Sets up the multi-selection context menu for mapping options.
    /// </summary>
    /// <param name="menu">The context menu to set up.</param>
    private void SetupMultiSelectionContextMenu(ContextMenuStrip menu)
    {
        var selectedCount = this.selectedItems.Count;

        // Create main edit item
        var editItems = new ToolStripMenuItem { Text = "Edit Multiple Mappings" };
        menu.Items.Add(editItems);

        // Create sub-items for triggers and actions
        var editTriggerItems = this.AddDropDownItem(editItems, "Triggers");
        var editActionsItems = this.AddDropDownItem(editItems, "Actions");

        // Get properties of the first selected item
        var firstItem = (MappedOption)this.selectedItems[0].Cast<OLVListItem>().RowObject;
        this.PopulateDropDownItems(editTriggerItems, firstItem.Trigger);
        this.PopulateDropDownItems(editActionsItems, firstItem.Action);

        // Add item for removing selected mappings
        var removeItems = menu.Items.Add($"Remove {selectedCount} Mappings");
        removeItems.Click += (s, _) => this.SelectRemoveMappings?.Invoke(this, new());
    }

    /// <summary>
    /// Adds a dropdown item to the specified menu item.
    /// </summary>
    /// <param name="menuItem">The parent menu item.</param>
    /// <param name="text">The text of the dropdown item to add.</param>
    /// <returns>The created dropdown item.</returns>
    private ToolStripMenuItem AddDropDownItem(ToolStripMenuItem menuItem, string text)
    {
        var dropDownItem = new ToolStripMenuItem(text);
        menuItem.DropDownItems.Add(dropDownItem);
        return dropDownItem;
    }

    /// <summary>
    /// Populates dropdown items based on the properties of the specified action or trigger.
    /// </summary>
    /// <param name="dropdownItem">The parent dropdown item.</param>
    /// <param name="mappingAspect">The trigger or action whose properties are used to populate the dropdown items.</param>
    private void PopulateDropDownItems<TAspect>(
        ToolStripMenuItem dropdownItem,
        TAspect mappingAspect
    )
    {
        var aspectType = mappingAspect.GetType();
        var properties = mappingAspect
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
            {
                continue;
            }

            var item = new ToolStripMenuItem(property.Name);
            var aspectsToEdit = new List<AbstractMappingAspect>();
            dropdownItem.DropDownItems.Add(item);

            foreach (var olvItem in this.selectedItems.Cast<OLVListItem>())
            {
                var mappedOption = (MappedOption)olvItem.RowObject;
                var otherMappingAspect = (AbstractMappingAspect)
                    (
                        typeof(TAspect) == typeof(AbstractTrigger)
                        ? mappedOption.Trigger
                        : mappedOption.Action
                    );

                // Disable if any selected item is of a different type of aspect
                if (!aspectType.Equals(otherMappingAspect.GetType()))
                {
                    item.Enabled = false;
                    break;
                }

                aspectsToEdit.Add(otherMappingAspect);
            }

            if (item.Enabled)
            {
                item.Click += (s, e) =>
                {
                    var dialog = new MappingPropertyEditorForm(property, aspectsToEdit);
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        this.SelectMultiEditMapping?.Invoke(
                            this,
                            new(
                                property,
                                aspectsToEdit,
                                dialog.Value
                            )
                        );
                    }
                };
            }
        }

        if (dropdownItem.DropDownItems.Count == 0)
        {
            dropdownItem.DropDownItems.Add(new ToolStripMenuItem("No common properties"));
        }
    }

    internal ContextMenuStrip Build()
    {
        var addItem = this.menu.Items.Add("Add New Mapping");
        addItem.Click += (s, _) => this.SelectEditMapping?.Invoke(this, new(null));

        var selectedCount = this.selectedItems.Count;

        if (selectedCount > 1)
        {
            this.SetupMultiSelectionContextMenu(this.menu);
        }
        else
        {
            var selected = (MappedOption)this.selectedItems[0].Cast<OLVListItem>().RowObject;

            if (selected is MappedOption mappedOption)
            {
                var editItem = this.menu.Items.Add("Edit Mapping");
                editItem.Click += (s, _) => this.SelectEditMapping?.Invoke(this, new(mappedOption));

                var removeItem = this.menu.Items.Add("Remove Mapping");
                removeItem.Click += (s, _) => this.SelectRemoveMappings?.Invoke(this, new());

                this.menu.Items.Add(new ToolStripSeparator());

                if (mappedOption.IsChild)
                {
                    var removeParentItem = this.menu.Items.Add("Disconnect Mapping from Parent");
                    removeParentItem.Click += (s, _) => this.SelectMakeMappingParentless?.Invoke(this, new(mappedOption));
                }

                if (this.currentChildChoosingParent == null)
                {
                    var chooseNewParentItem = this.menu.Items.Add("Choose New Parent for this Mapping...");
                    chooseNewParentItem.Click += (s, _) => this.currentChildChoosingParent = mappedOption;
                    chooseNewParentItem.Enabled = !mappedOption.Children.Any();
                }
                else
                {
                    var chooseParentItem = this.menu.Items.Add("Choose as Parent");
                    chooseParentItem.Image = Resources.tick;
                    chooseParentItem.Click += (s, _) =>
                    {
                        this.SelectChooseNewParent?.Invoke(this, new(this.currentChildChoosingParent, mappedOption));
                        this.currentChildChoosingParent = null;
                    };
                    chooseParentItem.Enabled = !mappedOption.IsChild;

                    var cancelItem = this.menu.Items.Add("Cancel Choosing Parent");
                    cancelItem.Image = Resources.cross;
                    cancelItem.Click += (s, _) => this.currentChildChoosingParent = null;
                }
            }
        }

        return this.menu;
    }
}
