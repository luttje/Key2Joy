using System.Drawing;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    internal class ImageComboBox : ComboBox
    {
        /// <summary>
        /// Padding around the image
        /// </summary>
        public new Rectangle Padding { get; set; } = Rectangle.FromLTRB(5, 5, 5, 5);

        /// <summary>
        /// Spacing between text and image
        /// </summary>
        public float Spacing { get; set; } = 0;

        public ImageComboBox()
            : base()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 25;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (this.Items[e.Index] is not ImageComboBoxItem item)
            {
                return;
            }

            var text = item.ToString();
            var image = item.Image;

            e.DrawBackground();

            var textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? SystemBrushes.HighlightText
                : SystemBrushes.WindowText;

            if (image != null)
            {
                Rectangle rect = new(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
                e.Graphics.DrawImage(image, this.Padding.Left + rect.X, this.Padding.Top + rect.Y, rect.Width - this.Padding.Left - this.Padding.Right, rect.Height - this.Padding.Top - this.Padding.Bottom);
                e.Graphics.DrawString(text, e.Font, textColor, this.Padding.Left + this.Spacing + rect.Right, this.Padding.Top + rect.Y);
            }
            else
            {
                e.Graphics.DrawString(text, e.Font, textColor, e.Bounds.X, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }
    }

    internal class ImageComboBoxItem
    {
        public Image Image { get; set; }
        public object ItemValue { get; set; }
        public string DisplayMember { get; set; }

        public ImageComboBoxItem(object value, Image image, string displayMember = null)
        {
            this.Image = image;
            this.ItemValue = value;
            this.DisplayMember = displayMember;
        }

        public override string ToString()
        {
            if (this.DisplayMember == null)
            {
                return this.ItemValue.ToString();
            }

            var property = this.ItemValue.GetType().GetProperty(this.DisplayMember);

            if (property == null)
            {
                return this.ItemValue.ToString();
            }

            var propertyValue = property.GetValue(this.ItemValue);
            return propertyValue.ToString();
        }
    }

    internal class ImageComboBoxItem<T> : ImageComboBoxItem
    {
        public new T ItemValue { get; set; }

        public ImageComboBoxItem(T value, Image image, string displayMember = null)
            : base(value, image, displayMember)
        {
            this.ItemValue = value;
        }
    }
}
