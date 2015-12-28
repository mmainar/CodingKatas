using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RomanNumeralsKata
{
    [TestFixture]
    public class RomanNumeralsTests
    {
        private RomanNumeralsConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new RomanNumeralsConverter();
        }

        [TestCase]
        public void RomansHadNoZero()
        {
            Assert.AreEqual("", converter.Convert(0));
        }

        [TestCase]
        public void Romans()
        {
            Assert.AreEqual("I", converter.Convert(1));
            Assert.AreEqual("II", converter.Convert(2));
            Assert.AreEqual("III", converter.Convert(3));

            Assert.AreEqual("V", converter.Convert(5));
            Assert.AreEqual("VI", converter.Convert(6));
            Assert.AreEqual("VII", converter.Convert(7));
            Assert.AreEqual("VIII", converter.Convert(8)); 
            Assert.AreEqual("IX", converter.Convert(9));
            Assert.AreEqual("X", converter.Convert(10));

            Assert.AreEqual("XVIII", converter.Convert(18));

            Assert.AreEqual("XXXIII", converter.Convert(33));

            Assert.AreEqual("L", converter.Convert(50));

            Assert.AreEqual("LX", converter.Convert(60));

            Assert.AreEqual("XL", converter.Convert(40));

            Assert.AreEqual("C", converter.Convert(100));
            Assert.AreEqual("CCC", converter.Convert(300));
            Assert.AreEqual("CL", converter.Convert(150));

            Assert.AreEqual("DCCC", converter.Convert(800));
        }
    }

    public class RomanNumeralsConverter
    {
        private Dictionary<int, string> conversionDictionary = new Dictionary<int, string>();

        public RomanNumeralsConverter()
        {
            conversionDictionary.Add(1000, "M");
            conversionDictionary.Add(900, "CM");
            conversionDictionary.Add(500, "D");
            conversionDictionary.Add(400, "CD");
            conversionDictionary.Add(100, "C");
            conversionDictionary.Add(50, "L");
            conversionDictionary.Add(40, "XL");
            conversionDictionary.Add(10, "X");
            conversionDictionary.Add(9, "IX");
            conversionDictionary.Add(5, "V");
            conversionDictionary.Add(4, "IV");
            conversionDictionary.Add(1, "I");
            conversionDictionary.Add(0, "");
        }

        public String Convert(int n)
        {        
            if (n < 0)
                throw new ArgumentOutOfRangeException("n");

            var tuple = GetConversionFactor(n);
            if (n > 0)
              return tuple.Value + Convert(n - tuple.Key);

            return "";
        }

        private KeyValuePair<int, string> GetConversionFactor(int n)
        {
            return conversionDictionary.FirstOrDefault(cd => cd.Key <= n);
        }
    }
}
