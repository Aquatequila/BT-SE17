namespace Svg.Wrapper
{
    public class SvgCommandFactory
    {
        public SvgCommandFactory() {}

        private void InitPoint(SvgCommand point)
        {
            point.x = 0;
            point.y = 0;
            point.xAxisRotation = 0;
            point.largeArc = 0;
            point.sweep = 0;
            point.rx = 0;
            point.ry = 0;
            point.x1 = 0;
            point.y1 = 0;
    }

        public SvgCommand MCmd(double x, double y)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.M;
            returnValue.x           = x;
            returnValue.y           = y;

            return returnValue;
        }
        public SvgCommand mCmd(double x, double y)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.m;
            returnValue.x           = x;
            returnValue.y           = y;

            return returnValue;
        }
        public SvgCommand LCmd(double x, double y)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.L;
            returnValue.x           = x;
            returnValue.y           = y;

            return returnValue;
        }
        public SvgCommand lCmd(double x, double y)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.l;
            returnValue.x           = x;
            returnValue.y           = y;

            return returnValue;
        }

        public SvgCommand QCmd(double x, double y, double x1, double y1)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.Q;
            returnValue.x           = x;
            returnValue.y           = y;
            returnValue.x1          = x1;
            returnValue.y1          = y1;

            return returnValue;
        }

        public SvgCommand qCmd(double x, double y, double x1, double y1)
        {
            SvgCommand returnValue   = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type        = PointType.q;
            returnValue.x           = x;
            returnValue.y           = y;
            returnValue.x1          = x1;
            returnValue.y1          = y1;

            return returnValue;
        }

        public SvgCommand ACmd(double x, double y, double rx, double ry, double xAxisRotation, double largeArc, double sweep)
        {
            SvgCommand returnValue       = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type            = PointType.A;
            returnValue.x               = x;
            returnValue.y               = y;
            returnValue.rx              = rx;
            returnValue.ry              = ry;
            returnValue.xAxisRotation   = xAxisRotation;
            returnValue.largeArc        = largeArc;
            returnValue.sweep           = sweep;

            return returnValue;
        }
        public SvgCommand aCmd(double x, double y, double rx, double ry, double xAxisRotation, double largeArc, double sweep)
        {
            SvgCommand returnValue       = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type            = PointType.a;
            returnValue.x               = x;
            returnValue.y               = y;
            returnValue.rx              = rx;
            returnValue.ry              = ry;
            returnValue.xAxisRotation   = xAxisRotation;
            returnValue.largeArc        = largeArc;
            returnValue.sweep           = sweep;

            return returnValue;
        }

        public SvgCommand DeepCopyPoint(ref SvgCommand toCopy)
        {
            SvgCommand returnValue       = new SvgCommand();
            InitPoint(returnValue);

            returnValue.type            = toCopy.type;
            returnValue.x               = toCopy.x;
            returnValue.y               = toCopy.y;
            returnValue.xAxisRotation   = toCopy.xAxisRotation;
            returnValue.largeArc        = toCopy.largeArc;
            returnValue.sweep           = toCopy.sweep;
            returnValue.rx              = toCopy.rx;
            returnValue.ry              = toCopy.ry;
            returnValue.x1              = toCopy.x1;
            returnValue.y1              = toCopy.y1;

            return returnValue;
        }
    }
    
}

