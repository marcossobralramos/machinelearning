﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using Microsoft.Data.DataView;
using Microsoft.ML.Data;
using Microsoft.ML.Data.Conversion;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.ML.RunTests
{
    public class DataTypesTest : TestDataViewBase
    {
        public DataTypesTest(ITestOutputHelper helper)
           : base(helper)
        {
        }

        private readonly static Conversions _conv = Conversions.Instance;

        [Fact]
        public void R4ToSBtoR4()
        {
            var r4ToSB = Conversions.Instance.GetStringConversion<float>(NumberType.R4);

            var txToR4 = Conversions.Instance.GetStandardConversion< ReadOnlyMemory<char>, float>(
                TextType.Instance, NumberType.R4, out bool identity2);

            Assert.NotNull(r4ToSB);
            Assert.NotNull(txToR4);

            float fVal = float.NaN;
            StringBuilder textFVal = default;
            r4ToSB(in fVal, ref textFVal);

            Assert.True("?" == textFVal.ToString());

            fVal = 0;
            var fValTX = textFVal.ToString().AsMemory();
            txToR4(in fValTX, ref fVal);

            Assert.Equal(fVal, float.NaN);
        }

        [Fact]
        public void R8ToSBtoR8()
        {
            var r8ToSB = Conversions.Instance.GetStringConversion<double>(NumberType.R8);

            var txToR8 = Conversions.Instance.GetStandardConversion<ReadOnlyMemory<char>, double>(
                TextType.Instance, NumberType.R8, out bool identity2);

            Assert.NotNull(r8ToSB);
            Assert.NotNull(txToR8);

            double dVal = double.NaN;
            StringBuilder textDVal = default;
            r8ToSB(in dVal, ref textDVal);

            Assert.True("?" == textDVal.ToString());

            dVal = 0;
            var dValTX = textDVal.ToString().AsMemory();
            txToR8(in dValTX, ref dVal);

            Assert.Equal(dVal, double.NaN);
        }

        [Fact]
        public void TXToSByte()
        {
            var mapper = GetMapper<ReadOnlyMemory<char>, sbyte>(NumberType.I1);

            Assert.NotNull(mapper);

            //1. sbyte.MinValue in text to sbyte.
            sbyte minValue = sbyte.MinValue;
            sbyte maxValue = sbyte.MaxValue;
            ReadOnlyMemory<char> src = minValue.ToString().AsMemory();
            sbyte dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, minValue);

            //2. sbyte.MaxValue in text to sbyte.
            src = maxValue.ToString().AsMemory();
            dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, maxValue);

            //3. ERROR condition: sbyte.MinValue - 1 in text to sbyte.
            src = (sbyte.MinValue - 1).ToString().AsMemory();
            dst = 0;
            var ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to sbyte.", ex.Message);

            //4. ERROR condition: sbyte.MaxValue + 1 in text to sbyte.
            src = (sbyte.MaxValue + 1).ToString().AsMemory();
            dst = 0;
            ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to sbyte.", ex.Message);

            //5. Empty string in text to sbyte.
            src = default;
            dst = -1;
            mapper(in src, ref dst);
            Assert.Equal(default, dst);
        }

        [Fact]
        public void TXToShort()
        {
            var mapper = GetMapper<ReadOnlyMemory<char>, short>(NumberType.I2);

            Assert.NotNull(mapper);

            //1. short.MinValue in text to short.
            short minValue = short.MinValue;
            short maxValue = short.MaxValue;
            ReadOnlyMemory<char> src = minValue.ToString().AsMemory();
            short dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, minValue);

            //2. short.MaxValue in text to short.
            src = maxValue.ToString().AsMemory();
            dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, maxValue);

            //3. ERROR condition: short.MinValue - 1 in text to short.
            src = (minValue - 1).ToString().AsMemory();
            dst = 0;
            var ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to short.", ex.Message);

            //4. ERROR condition: short.MaxValue + 1 in text to short.
            src = (maxValue + 1).ToString().AsMemory();
            dst = 0;
            ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to short.", ex.Message);

            //5. Empty value in text to short.
            src = default;
            dst = -1;
            mapper(in src, ref dst);
            Assert.Equal(default, dst);
        }

        [Fact]
        public void TXToInt()
        {
            var mapper = GetMapper<ReadOnlyMemory<char>, int>(NumberType.I4);

            Assert.NotNull(mapper);

            //1. int.MinValue in text to int.
            int minValue = int.MinValue;
            int maxValue = int.MaxValue;
            ReadOnlyMemory<char> src = minValue.ToString().AsMemory();
            int dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, minValue);

            //2. int.MaxValue in text to int.
            src = maxValue.ToString().AsMemory();
            dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, maxValue);

            //3. ERROR condition: int.MinValue - 1 in text to int.
            src = ((long)minValue - 1).ToString().AsMemory();
            dst = 0;
            var ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to int.", ex.Message);

            //4. ERROR condition: int.MaxValue + 1 in text to int.
            src = ((long)maxValue + 1).ToString().AsMemory();
            dst = 0;
            ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to int.", ex.Message);

            //5. Empty value in text to int.
            src = default;
            dst = -1;
            mapper(in src, ref dst);
            Assert.Equal(default, dst);
        }

        [Fact]
        public void TXToLong()
        {
            var mapper = GetMapper<ReadOnlyMemory<char>, long>(NumberType.I8);

            Assert.NotNull(mapper);

            //1. long.MinValue in text to long.
            var minValue = long.MinValue;
            var maxValue = long.MaxValue;
            ReadOnlyMemory<char> src = minValue.ToString().AsMemory();
            var dst = default(long);
            mapper(in src, ref dst);
            Assert.Equal(dst, minValue);

            //2. long.MaxValue in text to long.
            src = maxValue.ToString().AsMemory();
            dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, maxValue);

            //3. long.MinValue - 1 in text to long.
            src = (minValue - 1).ToString().AsMemory();
            dst = 0;
            mapper(in src, ref dst);
            Assert.Equal(dst, (long)minValue - 1);

            //4. ERROR condition: long.MaxValue + 1 in text to long.
            src = ((ulong)maxValue + 1).ToString().AsMemory();
            dst = 0;
            var ex = Assert.ThrowsAny<Exception>(() => mapper(in src, ref dst));
            Assert.Equal("Value could not be parsed from text to long.", ex.Message);

            //5. Empty value in text to long.
            src = default;
            dst = -1;
            mapper(in src, ref dst);
            Assert.Equal(default, dst);
        }

        private static ValueMapper<TSrc, TDst> GetMapper<TSrc, TDst>(ColumnType dstType)
        {
            Assert.True(typeof(TDst) == dstType.RawType);

            return Conversions.Instance.GetStandardConversion<TSrc, TDst>(
                TextType.Instance, dstType, out bool identity);
        }
    }
}
