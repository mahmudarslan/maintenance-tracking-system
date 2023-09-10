using System;
using System.Collections.Generic;
using System.Text;

namespace Arslan.Vms.VehicleService
{  

    public class VehicleTypeConsts
    {
        public static int NameMaxLength { get; set; } = 200;
    }

    public class VehicleConsts
    {
        public static int PlateMaxLength { get; set; } = 100;
        public static int ChassisMaxLength { get; set; } = 100;
        public static int ColorMaxLength { get; set; } = 50;
        public static int MotorMaxLength { get; set; } = 100;
    }     
    public class LocationConsts
    {
        public static int NameMaxLength { get; set; } = 100;
    }

    public class DocNoFormatConsts
    {
        public static int PrefixMaxLength { get; set; } = 30;
        public static int SuffixMaxLength { get; set; } = 30;
    } 

}
