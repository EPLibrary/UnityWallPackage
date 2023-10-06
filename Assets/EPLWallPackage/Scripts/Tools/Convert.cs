
using System;

namespace EPL
{
    public static class Convert
    {
        public const double km_m = 1000;
        public const double m_Km = 1.0 / 1000;
        public const double km_AU = 1.0 / 149600000;
        public const double au_km = 149600000;
        public const double m3_km3 = 1.0 / 1e9;

        public const double sec_min = 1.0 / 60;
        public const double min_sec = 60.0;
        public const double sec_day = 1.0 / 86400;
        public const double day_year = 1.0 / 365.2425;
        public const double day_century = 1.0 / 36524.25;
        public const double hour_sec = 3600.0;
        public const double hour_day = 1.0 / 24.0;
        public const double year_day = 365.2425;
        public const double year_month = 12;
        public const double year_hour = 8760;
        public const double year_century = 1.0 / 100.0;
        public const double month_day = 30.4167;
        public const double milsec_sec = 0.001;

        public const double rad_degree = 180.0 / Math.PI;
        public const double degree_rad = Math.PI / 180.0;
    }
}