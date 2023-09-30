using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 25;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!AppDomainHelper.GetIsOwnDomain()) 
                return;
            
            if (e.Index < 0)
                return;

            var item = Items[e.Index] as ImageComboBoxItem;

            if (item == null)
                return;

            var text = item.ToString();
            var image = item.Image;

            e.DrawBackground();

            Brush textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? SystemBrushes.HighlightText
                : SystemBrushes.WindowText;

            if (image != null)
            {
                var rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
                e.Graphics.DrawImage(image, Padding.Left + rect.X, Padding.Top + rect.Y, rect.Width - Padding.Left - Padding.Right, rect.Height - Padding.Top - Padding.Bottom);
                e.Graphics.DrawString(text, e.Font, textColor, Padding.Left + Spacing + rect.Right, Padding.Top + rect.Y);
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
            Image = image;
            ItemValue = value;
            DisplayMember = displayMember;
        }

        public override string ToString()
        {
            if (DisplayMember == null)
                return ItemValue.ToString();

            var property = ItemValue.GetType().GetProperty(DisplayMember);

            if (property == null)
                return ItemValue.ToString();

            var propertyValue = property.GetValue(ItemValue);
            return propertyValue.ToString();
        }
    }

    internal class ImageComboBoxItem<T> : ImageComboBoxItem
    {
        public new T ItemValue { get; set; }

        public ImageComboBoxItem(T value, Image image, string displayMember = null)
            : base(value, image, displayMember)
        {
            ItemValue = value;
        }
    }
}
