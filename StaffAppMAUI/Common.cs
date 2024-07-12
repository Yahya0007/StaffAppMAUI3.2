using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.Common
{
    public class TitleViewFix : Grid
    {
        bool isMeasured;
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            isMeasured = true;
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }
        protected override Size ArrangeOverride(Rect bounds)
        {
            if (!isMeasured)
                Measure(bounds.Width, double.PositiveInfinity, MeasureFlags.None);
            if (bounds.Height == 0)
                bounds.Height = DesiredSize.Height + 12;
            return base.ArrangeOverride(bounds);
        }
    }
}
