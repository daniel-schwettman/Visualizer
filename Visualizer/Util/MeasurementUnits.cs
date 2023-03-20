using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.Util
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public static class MeasurementUnits
    {
        public static bool IsMetric { get; set; }
        public static MeasurementUnit CurrentUnit = MeasurementUnit.Inches;

        //100 pixels per meter
        public const double WorldToDisplayPixels = 150d; //device independent pixels per world unit (meters)

        public static double ConvertWorldUnitsToDisplayPixels(double worldUnits)
        {
            return worldUnits * WorldToDisplayPixels;
        }

        public static double ConvertDisplayPixelsToWorldUnits(double displayPixels)
        {
            return displayPixels / WorldToDisplayPixels;
        }

        public static double ConvertMetersPerSecondToFeetPerMinute(double metersPerSecond)
        {
            return metersPerSecond * 3.2808399d * 60;
        }

        public static double ConvertFeetPerMinuteToMetersPerSecond(double feetPerMinute)
        {
            return feetPerMinute / 3.2808399d / 60;
        }

        public static double ConvertFeetPerMinuteToMetersPerMinute(double feetPerMinute)
        {
            return feetPerMinute / 3.2808399d;
        }

        public static double ConvertMetersToInches(double meters)
        {
            return meters * 39.3700787d;
        }

        public static double ConvertMeterToMillimeters(double meters)
        {
            return meters * 1000;
        }

        public static double ConvertMillimetersToMeters(double millimeters)
        {
            return millimeters / 1000;
        }

        public static double ConvertInchesToMeters(double inches)
        {
            return inches * 0.0254d;
        }

        public static double ConvertInchesToMillimeters(double inches)
        {
            return inches * 25.4d;
        }

        public static double ConvertMillimetersToInches(double millimeters)
        {
            return millimeters / 25.4d;
        }

        public const double DipsPerInch = 96d;
        public const double PointsPerInch = 72d;
        public const double PointsPerDip = PointsPerInch / DipsPerInch;
        public const double DipsPerPoint = DipsPerInch / PointsPerInch;
        public const double DeviceIndependentPixelsPerInch = 96d;
        public const double DeviceIndependentPixelsPerMeter = DeviceIndependentPixelsPerInch * InchesPerMeter;
        public const double InchesPerMeter = 39.3700787d;
        public const double InchesPerFoot = 12d;
        public const double CentimetersPerMeter = 100d;
        public const double MetersPerDeviceIndependentPixel = 1d / DeviceIndependentPixelsPerMeter;
        public const double InchesPerDeviceIndependentPixel = 1d / DeviceIndependentPixelsPerInch;
        public const double FeetPerInch = 1d / InchesPerFoot;
        public const double MetersPerCentimeter = 1d / CentimetersPerMeter;

        public static double Convert(double value, MeasurementUnit from, MeasurementUnit to)
        {
            double pixelUnitValue = value;
            switch (from)
            {
                case MeasurementUnit.Pixel:
                    break;
                case MeasurementUnit.Point:
                    pixelUnitValue = value * DipsPerPoint;
                    break;
                case MeasurementUnit.Metric: //meters
                    pixelUnitValue = value * DeviceIndependentPixelsPerMeter;
                    break;
                case MeasurementUnit.English: //feet
                    pixelUnitValue = value * DeviceIndependentPixelsPerInch * InchesPerFoot;
                    break;
                case MeasurementUnit.Centimeters:
                    pixelUnitValue = value * DeviceIndependentPixelsPerMeter * MetersPerCentimeter;
                    break;
                case MeasurementUnit.Inches:
                    pixelUnitValue = value * DeviceIndependentPixelsPerInch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("from", from, "Unsupported MeasurementUnit");
            }

            switch (to)
            {
                case MeasurementUnit.Pixel:
                    return pixelUnitValue;
                case MeasurementUnit.Point:
                    return pixelUnitValue * PointsPerDip;
                case MeasurementUnit.Metric: //meters
                    return pixelUnitValue * MetersPerDeviceIndependentPixel;
                case MeasurementUnit.English: //feet
                    return pixelUnitValue * InchesPerDeviceIndependentPixel * FeetPerInch;
                case MeasurementUnit.Centimeters:
                    return pixelUnitValue * MetersPerDeviceIndependentPixel * CentimetersPerMeter;
                case MeasurementUnit.Inches:
                    return pixelUnitValue * InchesPerDeviceIndependentPixel;
                default:
                    throw new ArgumentOutOfRangeException("to", to, "Unsupported MeasurementUnit");
            }
        }
    }

    [System.Reflection.Obfuscation(Exclude = true)]
    public enum MeasurementUnit
    {
        Pixel,
        Metric,
        English,
        Centimeters,
        Inches,
        Point,
    }
}
