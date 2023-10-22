using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Util;

namespace Key2Joy.Gui;

public partial class MappingPropertyEditorForm : Form
{
    private const string TEXT_LABEL = "New value for '{0}'";
    public object Value { get; private set; }

    private PropertyInfo property;
    private IList<AbstractMappingAspect> mappingAspects;
    private Control ctrlValueInput;

    private readonly Type[] decimalLikeTypes = new[] {
        typeof(decimal),
        typeof(decimal?),
        typeof(double),
        typeof(double?),
        typeof(float),
        typeof(float?),
    };

    private readonly Type[] numberLikeTypes = new[] {
        typeof(int),
        typeof(int?),
        typeof(uint),
        typeof(uint?),
        typeof(long),
        typeof(long?),
        typeof(ulong),
        typeof(ulong?),
        typeof(short),
        typeof(short?),
        typeof(ushort),
        typeof(ushort?),
        typeof(byte),
        typeof(byte?),
        typeof(sbyte),
        typeof(sbyte?),
    };

    public MappingPropertyEditorForm(PropertyInfo property, IList<AbstractMappingAspect> mappingAspects)
    {
        this.InitializeComponent();

        this.property = property;
        this.mappingAspects = mappingAspects;

        this.Text = $"Editting {property.Name} on {mappingAspects.Count} items";
        this.grpValueEditor.Text = string.Format(TEXT_LABEL, property.Name);

        this.Load += this.MappingPropertyEditorForm_Load;
    }

    private void MappingPropertyEditorForm_Load(object sender, EventArgs e)
        => this.CreateValueInput();

    /// <summary>
    /// Generates a value input based on the property type
    /// </summary>
    // We cant simplify this since the minimum and maximum need to be set first.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
    private void CreateValueInput()
    {
        var propertyType = this.property.PropertyType;
        object commonValue = null;

        foreach (var mappingAspect in this.mappingAspects)
        {
            var value = this.property.GetValue(mappingAspect);

            if (commonValue == null)
            {
                commonValue = value;
            }
            else if (!commonValue.Equals(value))
            {
                commonValue = null;
                break;
            }
        }

        if (propertyType == typeof(string))
        {
            this.ctrlValueInput = new TextBox()
            {
                Text = commonValue as string ?? string.Empty
            };
        }
        else if (this.numberLikeTypes.Contains(propertyType))
        {
            var minValue = propertyType.GetNumericMinValue();
            var maxValue = propertyType.GetNumericMaxValue();
            var nud = new NumericUpDown
            {
                DecimalPlaces = 0,
                Minimum = TypeExtensions.ToDecimalSafe(minValue),
                Maximum = TypeExtensions.ToDecimalSafe(maxValue)
            };

            nud.Value = commonValue == null ? 0 : TypeExtensions.ToDecimalSafe(commonValue);

            this.ctrlValueInput = nud;
        }
        else if (this.decimalLikeTypes.Contains(propertyType))
        {
            var minValue = propertyType.GetNumericMinValue();
            var maxValue = propertyType.GetNumericMaxValue();
            var nud = new NumericUpDown
            {
                DecimalPlaces = 2,  // Assuming 2 decimal places
                Minimum = TypeExtensions.ToDecimalSafe(minValue),
                Maximum = TypeExtensions.ToDecimalSafe(maxValue)
            };

            nud.Value = commonValue == null ? 0 : TypeExtensions.ToDecimalSafe(commonValue);

            this.ctrlValueInput = nud;
        }
        else if (propertyType.IsEnum)
        {
            this.ctrlValueInput = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = Enum.GetValues(propertyType)
            };
        }
        else
        {
            // For all other types, error and close
            MessageBox.Show(
                $"Cannot (yet) edit this type of property: {propertyType.Name}",
                "Property type not (yet) supported",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }

        this.ctrlValueInput.Dock = DockStyle.Fill;
        this.pnlValueInputParent.Controls.Add(this.ctrlValueInput);
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void BtnApplyChanges_Click(object sender, EventArgs e)
    {
        var propertyType = this.property.PropertyType;
        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (this.ctrlValueInput is TextBox textBox)
        {
            this.Value = textBox.Text;
        }
        else if (this.ctrlValueInput is NumericUpDown numericUpDown)
        {
            this.Value = Convert.ChangeType(numericUpDown.Value, propertyType);
        }
        else if (this.ctrlValueInput is ComboBox comboBox)
        {
            this.Value = comboBox.SelectedItem;
        }
        else
        {
            // TODO: Handle other types
            this.Value = this.ctrlValueInput.Text;
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}
