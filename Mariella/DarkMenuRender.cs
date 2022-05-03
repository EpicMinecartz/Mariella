using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mariella {
    class DarkMenuRender : ToolStripSystemRenderer {

        public Color DarkTheme_UI_MenuBack = Color.FromArgb(40, 40, 40);
        public Color DarkTheme_UI_Text = Color.FromArgb(240, 240, 240);
        public Color DarkTheme_UI_MenuSel = Color.FromArgb(50, 50, 50);

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
            e.ToolStrip.BackColor = DarkTheme_UI_MenuBack;//realistically this should only be done once lmao
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            if (!e.Item.Selected) {
                e.Graphics.FillRectangle(new SolidBrush(DarkTheme_UI_MenuBack), rectangle);
            } else {
                if (e.Item.Enabled) {
                    this.RenderSelectedButtonFill(e.Graphics, rectangle);
                } else {
                    e.Graphics.FillRectangle(new SolidBrush(DarkTheme_UI_MenuBack), rectangle);
                }
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
            if (e.Item is ToolStripMenuItem) {
                e.TextColor = DarkTheme_UI_Text;
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
            e.ToolStrip.BackColor = DarkTheme_UI_MenuBack;
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);

            e.Graphics.FillRectangle(new SolidBrush(DarkTheme_UI_MenuBack), rectangle);
            this.RenderSeparatorInternal(e.Graphics, e.Item, new Rectangle(Point.Empty, e.Item.Size), e.Vertical);
        }

        public void ExtendedToolStripSeparator_Paint(object sender, PaintEventArgs e) {
            ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, toolStripSeparator.Width + 15, toolStripSeparator.Height + 15);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
            if (e.Item is ToolStripDropDownItem) {
                e.ArrowColor = DarkTheme_UI_Text;
            }
            base.OnRenderArrow(e);
        }

        private void RenderSeparatorInternal(Graphics g, ToolStripItem item, Rectangle bounds, bool vertical) {
            Pen foreColorPen = SystemPens.ControlDark;
            Pen highlightColorPen = SystemPens.ButtonHighlight;
            bool isASeparator = item is ToolStripSeparator;
            bool isAHorizontalSeparatorNotOnDropDownMenu = false;

            if (isASeparator) {
                if (vertical) {
                    if (!item.IsOnDropDown) {
                        bounds.Y += 3;
                        bounds.Height = Math.Max(0, bounds.Height - 6);
                    }
                } else {
                    ToolStripDropDownMenu dropDownMenu = item.GetCurrentParent() as ToolStripDropDownMenu;
                    if (dropDownMenu != null) {
                        if (dropDownMenu.RightToLeft == RightToLeft.No) {
                            bounds.X += dropDownMenu.Padding.Left - 2;
                            bounds.Width = dropDownMenu.Width - bounds.X;
                        } else {
                            bounds.X += 2;
                            bounds.Width = dropDownMenu.Width - bounds.X - dropDownMenu.Padding.Right;

                        }
                    } else {
                        isAHorizontalSeparatorNotOnDropDownMenu = true;
                    }
                }
            }
            try {
                if (vertical) {
                    if (bounds.Height >= 4) {
                        bounds.Inflate(0, -2);
                    }

                    bool rightToLeft = (item.RightToLeft == RightToLeft.Yes);
                    Pen leftPen = rightToLeft ? highlightColorPen : foreColorPen;
                    Pen rightPen = rightToLeft ? foreColorPen : highlightColorPen;

                    int startX = bounds.Width / 2;
                    g.DrawLine(leftPen, startX, bounds.Top, startX, bounds.Bottom - 1);
                    startX += 1;
                    g.DrawLine(rightPen, startX, bounds.Top + 1, startX, bounds.Bottom);
                } else {
                    if (isAHorizontalSeparatorNotOnDropDownMenu && bounds.Width >= 4) {
                        bounds.Inflate(-2, 0);
                    }
                    int startY = bounds.Height / 2;

                    g.DrawLine(foreColorPen, bounds.Left, startY, bounds.Right - 1, startY);

                    if ((!isASeparator) || isAHorizontalSeparatorNotOnDropDownMenu) {
                        startY += 1;
                        g.DrawLine(highlightColorPen, bounds.Left + 1, startY, bounds.Right - 1, startY);
                    }
                }
            } finally {

            }
        }

        private static bool GetPen(Color color, ref Pen pen) {
            if (color.IsSystemColor) {
                pen = SystemPens.FromSystemColor(color);
                return false;
            } else {
                pen = new Pen(color);
                return true;
            }
        }

        private void RenderSelectedButtonFill(Graphics g, Rectangle bounds) {
            if (bounds.Width == 0 || bounds.Height == 0) {
                return;
            }
            g.FillRectangle(new System.Drawing.SolidBrush(DarkTheme_UI_MenuSel), bounds);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e) {
            base.OnRenderSplitButtonBackground(e);
            DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item as ToolStripSplitButton, ((ToolStripSplitButton)e.Item).DropDownButtonBounds, Color.FromArgb(240, 240, 240), ArrowDirection.Down));
        }

        public void DrawArrow(ToolStripArrowRenderEventArgs e) {
            OnRenderArrow(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {

        }
    }
}
