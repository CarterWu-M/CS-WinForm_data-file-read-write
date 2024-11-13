using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INI_file_study.Utilities
{
    internal class jsonFile
    {
        public class Impedance
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }

        public class Voltage
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }
        public class Current
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }
        public class Power
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }
        public class TestPoint1
        {
            public Impedance Impedance { get; set; }
            public Voltage Voltage { get; set; }
            public Current Current { get; set; }
            public Power Power { get; set; }
        }
        public class TestPoint2
        {
            public Impedance Impedance { get; set; }
            public Voltage Voltage { get; set; }
        }

        public class Root
        {
            public TestPoint1 TestPoint1 { get; set; }
            public TestPoint2 TestPoint2 { get; set; }
        }
    }
}
