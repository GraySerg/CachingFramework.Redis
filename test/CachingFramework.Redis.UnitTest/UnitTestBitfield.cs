﻿using System;
using CachingFramework.Redis.Contracts;
using NUnit.Framework;using NUnit.Framework.Legacy;

namespace CachingFramework.Redis.UnitTest
{
    [TestFixture]
    public class UnitTestBitfield
    {
        [OneTimeSetUp]
        public void SetUpFixture()
        {
            if (Common.VersionInfo[0] < 3)
            {
                ClassicAssert.Ignore($"Bitfield tests ignored for version {string.Join(".", Common.VersionInfo)}\n");
            }
        }

        [Test, TestCaseSource(typeof(Common), "Raw")]
        public void UT_CacheBitmapBitField(RedisContext context)
        {
            var key = "UT_CacheBitmapBitField";
            context.Cache.Remove(key);
            var rb = context.Collections.GetRedisBitmap(key);
            var ex = rb.BitfieldSet(BitfieldType.u4, 0, 14, false, OverflowType.Fail);
            var n1 = rb.BitfieldGet<decimal>(BitfieldType.u4, 0);
            ClassicAssert.AreEqual(0, ex);
            ClassicAssert.AreEqual(14, n1);
            rb.BitfieldSet(BitfieldType.u16, 0, 0xb525);
            ClassicAssert.AreEqual(0xb5, rb.BitfieldGet<int>(BitfieldType.u8, 0));
            ClassicAssert.AreEqual(0x25, rb.BitfieldGet<uint>(BitfieldType.u8, 1, true));

            ClassicAssert.AreEqual(0x2, rb.BitfieldGet<byte>(BitfieldType.u3, 4));
            ClassicAssert.AreEqual(0x12, rb.BitfieldGet<sbyte>(BitfieldType.u5, 7));
            ClassicAssert.AreEqual(0x2, rb.BitfieldGet<long>(BitfieldType.u3, 2, true));
        }

        [Test, TestCaseSource(typeof(Common), "Raw")]
        public void UT_CacheBitmapBitField_Overflow(RedisContext context)
        {
            var key = "UT_CacheBitmapBitField_Overflow";
            context.Cache.Remove(key);
            var rb = context.Collections.GetRedisBitmap(key);
            ClassicAssert.Throws<OverflowException>(() => rb.BitfieldSet(BitfieldType.u1, 0, -2, false, OverflowType.Fail));
            ClassicAssert.DoesNotThrow(() => rb.BitfieldSet(BitfieldType.u1, 0, -2));
            ClassicAssert.DoesNotThrow(() => rb.BitfieldSet(BitfieldType.u1, 0, -2, false, OverflowType.Saturation));
        }

        [Test, TestCaseSource(typeof(Common), "Raw")]
        public void UT_CacheBitmapBitField_WrapSaturation(RedisContext context)
        {
            var key = "UT_CacheBitmapBitField_WrapSaturation";
            context.Cache.Remove(key);
            var rb = context.Collections.GetRedisBitmap(key);
            rb.BitfieldSet(BitfieldType.u13, 10, 8191, true);
            ClassicAssert.AreEqual(8191, rb.BitfieldGet<int>(BitfieldType.u13, 10, true));
            rb.BitfieldIncrementBy(BitfieldType.u13, 10, 4, true);
            ClassicAssert.AreEqual(3, rb.BitfieldGet<UInt16>(BitfieldType.u13, 10, true));
            rb.BitfieldSet(BitfieldType.u13, 4, 8191);
            rb.BitfieldIncrementBy(BitfieldType.u13, 4, 999, false, OverflowType.Saturation);
            ClassicAssert.AreEqual(8191, rb.BitfieldGet<UInt32>(BitfieldType.u13, 4));
        }
    }
}
