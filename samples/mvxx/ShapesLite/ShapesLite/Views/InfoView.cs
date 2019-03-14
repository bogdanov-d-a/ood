﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLite.Views
{
    using RectangleSignallingValue = Common.SignallingValue<Common.Rectangle<double>>;

    public class InfoView
    {
        public RectangleSignallingValue DomainModelRect =
            new RectangleSignallingValue(new Common.RectangleDouble(0, 0, 0, 0));

        public RectangleSignallingValue AppModelRect =
            new RectangleSignallingValue(new Common.RectangleDouble(0, 0, 0, 0));

        public InfoView()
        {
            DomainModelRect.Event += (Common.Rectangle<double> rect) => FireTextChangedEvent();
            AppModelRect.Event += (Common.Rectangle<double> rect) => FireTextChangedEvent();
        }

        private static string RectToString(Common.Rectangle<double> rect)
        {
            return String.Format("{{ {0:F1}, {1:F1}, {2:F1}, {3:F1} }}",
                rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public string GetText()
        {
            return "DMR = " + RectToString(DomainModelRect.Value) + " AMR = " + RectToString(AppModelRect.Value);
        }

        public delegate void TextDelegate(string text);
        public event TextDelegate TextChangedEvent = delegate {};

        private void FireTextChangedEvent()
        {
            TextChangedEvent(GetText());
        }
    }
}
