﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CameraControl.Core.Interfaces;
using CameraControl.Devices;

namespace CameraControl.Core.Classes
{
    public class FilenameTemplate : IFilenameTemplate
    {
        public string Pharse(string template, PhotoSession session, ICameraDevice device, string fileName)
        {
            CameraProperty property = device.LoadProperties();
            switch (template)
            {
                case "[Counter 3 digit]":
                case "[Counter 4 digit]":
                case "[Counter 5 digit]":
                case "[Counter 6 digit]":
                case "[Counter 7 digit]":
                case "[Counter 8 digit]":
                case "[Counter 9 digit]":
                    return session.Counter.ToString(new string('0', Convert.ToInt16(template.Substring(9, 1))));
                case "[Series 4 digit]":
                    return session.Series.ToString(new string('0', 4));
                case "[Camera Counter 3 digit]":
                case "[Camera Counter 4 digit]":
                case "[Camera Counter 5 digit]":
                case "[Camera Counter 6 digit]":
                case "[Camera Counter 7 digit]":
                case "[Camera Counter 8 digit]":
                case "[Camera Counter 9 digit]":
                    return property.Counter.ToString(new string('0', Convert.ToInt16(template.Substring(16, 1))));
                case "[Session Name]":
                    return session.Name;
                case "[Capture Name]":
                    return session.CaptureName;
                case "[Exposure Compensation]":
                    if (device!=null && device.ExposureCompensation != null)
                        return device.ExposureCompensation.Value != "0" ? device.ExposureCompensation.Value : "";
                    return "";
                case "[FNumber]":
                    if (device != null && device.FNumber != null)
                        return device.FNumber.Value ?? "";
                    return "";
                case "[Date yyyy-MM-dd]":
                    return DateTime.Now.ToString("yyyy-MM-dd");
                case "[Date yyyy]":
                    return DateTime.Now.ToString("yyyy");
                case "[Date yyyy-MM]":
                    return DateTime.Now.ToString("yyyy-MM");
                case "[Date MMM]":
                    return DateTime.Now.ToString("MMM");
                case "[Barcode]":
                    return session.Barcode;
                case "[File format]":
                    return GetType(fileName);
                case "[Original Filename]":
                    return Path.GetFileNameWithoutExtension(fileName);
                case "[Camera Name]":
                    return property.DeviceName.Replace(":", "_").Replace("?", "_").Replace("*", "_");
                case "[Selected Tag1]":
                    return session.SelectedTag1 != null ? session.SelectedTag1.Value.Trim() : "";
                case "[Selected Tag2]":
                    return session.SelectedTag2 != null ? session.SelectedTag2.Value.Trim() : "";
                case "[Selected Tag3]":
                    return session.SelectedTag3 != null ? session.SelectedTag3.Value.Trim() : "";
                case "[Selected Tag4]":
                    return session.SelectedTag4 != null ? session.SelectedTag4.Value.Trim() : "";
                case "[Unix Time]":
                    var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTime.Now.Kind);
                    var unixTimestamp = System.Convert.ToInt64((DateTime.Now - date).TotalSeconds);
                    return unixTimestamp.ToString();

            }
            return "";
        }

        private string GetType(string file)
        {
            var ext = Path.GetExtension(file);
            if (ext.StartsWith("."))
                ext = ext.Substring(1);
            switch (ext.ToLower())
            {
                case "jpg":
                    return "Jpg";
                case "nef":
                    return "Raw";
                case "cr2":
                    return "Raw";
                case "tif":
                    return "Tif";
            }
            return ext;
        }

    }
}
