/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2017 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SoftProofing
{
    internal sealed class ColorButton : Button
    {
        private Color value;
        private bool isFocused;
        private bool isHovering;
        private bool isPressed;

        public ColorButton()
        {
            this.value = Color.Black;
            this.isFocused = false;
            this.isHovering = false;
            this.isPressed = false;
        }

        [DefaultValue(typeof(Color), "Black")]
        public Color Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    Invalidate();
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            this.isFocused = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            this.isFocused = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if ((mevent.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.isHovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.isHovering = false;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            if (this.isPressed)
            {
                this.isPressed = false;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            PushButtonState state = PushButtonState.Normal;

            if (this.isPressed && RectangleToScreen(ClientRectangle).Contains(MousePosition))
            {
                state = PushButtonState.Pressed;
            }
            else if (this.isHovering)
            {
                state = PushButtonState.Hot;
            }

            ButtonRenderer.DrawButton(e.Graphics, e.ClipRectangle, this.isFocused, state);

            Rectangle bounds = e.ClipRectangle;
            bounds.Inflate(-4, -4);
            bounds.Width -= 1;
            bounds.Height -= 1;

            using (SolidBrush brush = new SolidBrush(this.value))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            e.Graphics.DrawRectangle(SystemPens.GrayText, bounds);
        }
    }
}
