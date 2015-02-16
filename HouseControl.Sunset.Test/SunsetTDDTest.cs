﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HouseControl.Sunset.Test
{
    [TestClass]
    public class SunsetTDDTest
    {
        string sampleData = "{\"results\":{\"sunrise\":\"2:35:18 PM\",\"sunset\":\"1:35:58 AM\",\"solar_noon\":\"8:05:38 PM\",\"day_length\":\"11:00:40\",\"civil_twilight_begin\":\"2:09:53 PM\",\"civil_twilight_end\":\"2:01:23 AM\",\"nautical_twilight_begin\":\"1:40:38 PM\",\"nautical_twilight_end\":\"2:30:38 AM\",\"astronomical_twilight_begin\":\"1:11:37 PM\",\"astronomical_twilight_end\":\"2:59:39 AM\"},\"status\":\"OK\"}";
        string errorData = "{\"status\":\"ERROR\"}";

        [TestMethod]
        public void CheckStatus_WithGoodData_ReturnsTrue()
        {
            bool result = SunsetTDD.CheckStatus(sampleData);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckStatus_WithErrorData_ReturnsFalse()
        {
            bool result = SunsetTDD.CheckStatus(errorData);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParseSunset_WithGoodData_ReturnsString()
        {
            string result = SunsetTDD.ParseSunset(sampleData);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseSunset_WithErrorData_ReturnsNull()
        {
            string result = SunsetTDD.ParseSunset(errorData);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseSunset_WithGoodData_ReturnsExpectedString()
        {
            string expected = "1:35:58 AM";
            string result = SunsetTDD.ParseSunset(sampleData);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetLocalTime_UTCTimeString_ReturnsLocalTime()
        {
            string utcTimeString = "1:35:58 AM";
            DateTime currentDate = new DateTime(2015, 02, 15);
            DateTime expectedLocalTime = new DateTime(2015, 02, 15, 17, 35, 58);
            DateTime result = SunsetTDD.GetLocalTime(utcTimeString, currentDate);
            Assert.AreEqual(expectedLocalTime, result);
        }

        [TestMethod]
        public void GetLocalTime_NullTimeString_ReturnsDateOnly()
        {
            string utcTimeString = null;
            DateTime currentDate = new DateTime(2015, 02, 15);
            DateTime expectedLocalTime = new DateTime(2015, 02, 15, 0, 0, 0);
            DateTime result = SunsetTDD.GetLocalTime(utcTimeString, currentDate);
            Assert.AreEqual(expectedLocalTime, result);
        }

        [TestMethod]
        public void ParseSunrise_WithGoodData_ReturnsString()
        {
            string result = SunsetTDD.ParseSunrise(sampleData);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseSunrise_WithErrorData_ReturnsNull()
        {
            string result = SunsetTDD.ParseSunrise(errorData);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseSunrise_WithGoodData_ReturnsExpectedString()
        {
            string expected = "2:35:18 PM";
            string result = SunsetTDD.ParseSunrise(sampleData);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetSunset_WithDate_ReturnsSunsetForDate()
        {
            DateTime currentDate = new DateTime(2015, 02, 15);
            DateTime expected = new DateTime(2015, 02, 15, 17, 35, 58);

            Mock<ISunsetService> serviceMock = new Mock<ISunsetService>();
            serviceMock.Setup(s => s.GetServiceData(It.IsAny<DateTime>()))
                .Returns(sampleData);

            SunsetTDD provider = new SunsetTDD();
            provider.SunsetService = serviceMock.Object;
            DateTime result = provider.GetSunset(currentDate);

            Assert.AreEqual(expected, result);
        }
    }
}