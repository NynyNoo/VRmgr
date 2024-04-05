using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

public class DataConverter : MonoBehaviour
{
    /*public (Vector3, Vector3, DateTime) ParseData(string dataString)
    {
        Vector3 Accelerometer;
        Vector3 Gyroscope;
        DateTime Timestamp;

        dataString = ReplaceCommasWithDots(dataString);

        Regex regex = new Regex(@"X:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Y:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Z:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?);\s*X:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Y:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Z:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?);\s*(\d+:\d+:\d+\.\d+)");

        Match match = regex.Match(dataString);

        if (match.Success)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            Accelerometer = new Vector3(
                float.Parse(match.Groups[1].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[2].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[3].Value, NumberStyles.Float, cultureInfo)
            );

            Gyroscope = new Vector3(
                float.Parse(match.Groups[4].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[5].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[6].Value, NumberStyles.Float, cultureInfo)
            );
            Timestamp = DateTime.Parse(match.Groups[7].Value);

            return (Accelerometer, Gyroscope, Timestamp);
        }
        else
        {
            throw new FormatException("Nieprawid³owy format danych.");
        }
    }*/

    public (Vector3, Vector3, Quaternion, DateTime) ParseData(string dataString)
    {
        Vector3 accelerometer;
        Vector3 gyroscope;
        Quaternion attitude;
        DateTime timestamp;

        dataString = ReplaceCommasWithDots(dataString);

        Regex regex = new Regex(@"X:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Y:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Z:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?);\s*X:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Y:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Z:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?);\s*X:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Y:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*Z:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?),\s*W:\s*(-?\d+[\.,]?\d*(?:E[-+]?\d+)?);\s*(\d+:\d+:\d+\.\d+)");

        Match match = regex.Match(dataString);

        if (match.Success)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            accelerometer = new Vector3(
                float.Parse(match.Groups[1].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[2].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[3].Value, NumberStyles.Float, cultureInfo)
            );

            gyroscope = new Vector3(
                float.Parse(match.Groups[4].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[5].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[6].Value, NumberStyles.Float, cultureInfo)
            );

            attitude = new Quaternion(
                float.Parse(match.Groups[7].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[8].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[9].Value, NumberStyles.Float, cultureInfo),
                float.Parse(match.Groups[10].Value, NumberStyles.Float, cultureInfo)
            );

            timestamp = DateTime.Parse(match.Groups[11].Value);

            // Wywo³ujemy delegata z przetworzonymi danymi
            return (accelerometer, gyroscope, attitude, timestamp);
        }
        else
        {
            Debug.LogError(dataString);
            throw new FormatException("Nieprawid³owy format danych.");
        }
    }



    public string ReplaceCommasWithDots(string input)
    {
        // Zamieniamy przecinki na kropki tylko w przypadku, gdy wystêpuj¹ po cyfrze (separator dziesiêtny)
        input = Regex.Replace(input, @"(\d),(\d)", "$1.$2");
        return input.Replace(" ", "");
    }



}
