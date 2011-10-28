using System.Drawing;

namespace Aga.Controls.Tree.NodeControls
{
	public class NodeIcon : BindableControl
	{
		public NodeIcon()
		{
			LeftMargin = 1;
		}

		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			Image image = GetIcon(node);
			
            if (image != null)
				return image.Size;
			
            return Size.Empty;
		}

		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			Image image = GetIcon(node);

			if (image != null)
			{
				Rectangle r = GetBounds(node, context);

				context.Graphics.DrawImage(image, r.Location);
			}
		}

		protected virtual Image GetIcon(TreeNodeAdv node)
		{
			return GetValue(node) as Image;
		}
	}
}
