using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Casper.Network.SDK.Types;
using NUnit.Framework;
using Org.BouncyCastle.Utilities.Encoders;

namespace NetCasperTest
{
    public class CLValueToTypeTest
    {
        [Test]
        public void CLValueToBoolean()
        {
            var clValue = CLValue.Bool(false);
            Assert.AreEqual(false, clValue.ToBoolean());
            Assert.AreEqual(false, (bool) clValue);

            clValue = CLValue.Bool(true);
            Assert.AreEqual(true, clValue.ToBoolean());
            Assert.AreEqual(true, (bool) clValue);

            clValue = CLValue.Ok(CLValue.Bool(false), new CLTypeInfo(CLType.String));
            Assert.AreEqual(false, clValue.ToBoolean());
            Assert.AreEqual(false, (bool) clValue);

            clValue = CLValue.Ok(CLValue.Bool(true), new CLTypeInfo(CLType.String));
            Assert.AreEqual(true, clValue.ToBoolean());
            Assert.AreEqual(true, (bool) clValue);

            clValue = CLValue.I32(0);
            var ex = Assert.Catch<FormatException>(() => clValue.ToBoolean());
            Assert.IsTrue(ex?.Message.Contains("I32"));

            clValue = CLValue.Ok(CLValue.U32(0), new CLTypeInfo(CLType.String));
            ex = Assert.Catch<FormatException>(() => clValue.ToBoolean());
            Assert.IsTrue(ex?.Message.Contains("U32"));
        }

        [Test]
        public void CLValueToInt32()
        {
            var clValue = CLValue.I32(int.MinValue);
            Assert.AreEqual(int.MinValue, clValue.ToInt32());
            Assert.AreEqual(int.MinValue, (int) clValue);

            clValue = CLValue.I32(0);
            Assert.AreEqual(0, clValue.ToInt32());
            Assert.AreEqual(0, (int) clValue);

            clValue = CLValue.I32(int.MaxValue);
            Assert.AreEqual(int.MaxValue, clValue.ToInt32());
            Assert.AreEqual(int.MaxValue, (int) clValue);

            clValue = CLValue.U8(255);
            Assert.AreEqual(255, clValue.ToInt32());
            Assert.AreEqual(255, (int) clValue);

            clValue = CLValue.Ok(CLValue.I32(int.MaxValue), new CLTypeInfo(CLType.String));
            Assert.AreEqual(int.MaxValue, clValue.ToInt32());
            Assert.AreEqual(int.MaxValue, (int) clValue);

            clValue = CLValue.Ok(CLValue.U8(255), new CLTypeInfo(CLType.String));
            Assert.AreEqual(255, clValue.ToInt32());
            Assert.AreEqual(255, (int) clValue);

            clValue = CLValue.I64(0);
            var ex = Assert.Catch<FormatException>(() => clValue.ToBoolean());
            Assert.IsTrue(ex?.Message.Contains("I64"));
        }

        [Test]
        public void CLValueToInt64()
        {
            var clValue = CLValue.I64(long.MinValue);
            Assert.AreEqual(long.MinValue, clValue.ToInt64());
            Assert.AreEqual(long.MinValue, (long) clValue);

            clValue = CLValue.I64(0);
            Assert.AreEqual(0, clValue.ToInt64());
            Assert.AreEqual(0, (long) clValue);

            clValue = CLValue.I64(long.MaxValue);
            Assert.AreEqual(long.MaxValue, clValue.ToInt64());
            Assert.AreEqual(long.MaxValue, (long) clValue);

            clValue = CLValue.I32(int.MinValue);
            Assert.AreEqual(int.MinValue, clValue.ToInt64());
            Assert.AreEqual(int.MinValue, (long) clValue);

            clValue = CLValue.U32(uint.MaxValue);
            Assert.AreEqual(uint.MaxValue, clValue.ToInt64());
            Assert.AreEqual(uint.MaxValue, (long) clValue);

            clValue = CLValue.U8(255);
            Assert.AreEqual(255, clValue.ToInt64());
            Assert.AreEqual(255, (long) clValue);

            clValue = CLValue.Ok(CLValue.I64(long.MaxValue), new CLTypeInfo(CLType.String));
            Assert.AreEqual(long.MaxValue, clValue.ToInt64());
            Assert.AreEqual(long.MaxValue, (long) clValue);
        }

        [Test]
        public void CLValueToByte()
        {
            var clValue = CLValue.U8(byte.MinValue);
            Assert.AreEqual(byte.MinValue, clValue.ToByte());
            Assert.AreEqual(byte.MinValue, (byte) clValue);

            clValue = CLValue.U8(byte.MaxValue);
            Assert.AreEqual(byte.MaxValue, clValue.ToByte());
            Assert.AreEqual(byte.MaxValue, (byte) clValue);

            clValue = CLValue.Ok(CLValue.U8(byte.MaxValue), new CLTypeInfo(CLType.String));
            Assert.AreEqual(byte.MaxValue, clValue.ToByte());
            Assert.AreEqual(byte.MaxValue, (byte) clValue);
        }

        [Test]
        public void CLValueToUInt32()
        {
            var clValue = CLValue.U32(uint.MinValue);
            Assert.AreEqual(uint.MinValue, clValue.ToUInt32());
            Assert.AreEqual(uint.MinValue, (uint) clValue);

            clValue = CLValue.U32(uint.MaxValue);
            Assert.AreEqual(uint.MaxValue, clValue.ToUInt32());
            Assert.AreEqual(uint.MaxValue, (uint) clValue);

            clValue = CLValue.U8(byte.MaxValue);
            Assert.AreEqual(byte.MaxValue, clValue.ToUInt32());
            Assert.AreEqual(byte.MaxValue, (uint) clValue);

            clValue = CLValue.Ok(CLValue.U32(uint.MaxValue), new CLTypeInfo(CLType.String));
            Assert.AreEqual(uint.MaxValue, clValue.ToUInt32());
            Assert.AreEqual(uint.MaxValue, (uint) clValue);
        }

        [Test]
        public void CLValueToUInt64()
        {
            var clValue = CLValue.U64(ulong.MinValue);
            Assert.AreEqual(ulong.MinValue, clValue.ToUInt64());
            Assert.AreEqual(ulong.MinValue, (ulong) clValue);

            clValue = CLValue.U64(ulong.MaxValue);
            Assert.AreEqual(ulong.MaxValue, clValue.ToUInt64());
            Assert.AreEqual(ulong.MaxValue, (ulong) clValue);

            clValue = CLValue.U32(uint.MaxValue);
            Assert.AreEqual(uint.MaxValue, clValue.ToUInt64());
            Assert.AreEqual(uint.MaxValue, (ulong) clValue);

            clValue = CLValue.U8(byte.MaxValue);
            Assert.AreEqual(byte.MaxValue, clValue.ToUInt64());
            Assert.AreEqual(byte.MaxValue, (ulong) clValue);

            clValue = CLValue.Ok(CLValue.U64(ulong.MaxValue), new CLTypeInfo(CLType.String));
            Assert.AreEqual(ulong.MaxValue, clValue.ToUInt64());
            Assert.AreEqual(ulong.MaxValue, (ulong) clValue);
        }

        [Test]
        public void CLValueToBigInteger()
        {
            var clValue = CLValue.U512(5123456789012);
            Assert.AreEqual(new BigInteger(5123456789012), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(5123456789012), (BigInteger) clValue);

            clValue = CLValue.U256(5123456789012);
            Assert.AreEqual(new BigInteger(5123456789012), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(5123456789012), (BigInteger) clValue);

            clValue = CLValue.U128(5123456789012);
            Assert.AreEqual(new BigInteger(5123456789012), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(5123456789012), (BigInteger) clValue);

            var bigInt = new BigInteger(Hex.Decode("F0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF"), true);
            clValue = CLValue.U128(bigInt);
            Assert.AreEqual(bigInt, clValue.ToBigInteger());
            Assert.AreEqual(bigInt, (BigInteger) clValue);

            clValue = CLValue.U64(5123456789012);
            Assert.AreEqual(new BigInteger(5123456789012), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(5123456789012), (BigInteger) clValue);

            clValue = CLValue.U32(uint.MaxValue);
            Assert.AreEqual(new BigInteger(uint.MaxValue), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(uint.MaxValue), (BigInteger) clValue);

            clValue = CLValue.U8(0xFF);
            Assert.AreEqual(new BigInteger(0xFF), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(0xFF), (BigInteger) clValue);

            clValue = CLValue.I64(long.MaxValue);
            Assert.AreEqual(new BigInteger(long.MaxValue), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(long.MaxValue), (BigInteger) clValue);

            clValue = CLValue.I32(int.MaxValue);
            Assert.AreEqual(new BigInteger(int.MaxValue), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(int.MaxValue), (BigInteger) clValue);

            clValue = CLValue.Ok(CLValue.U512(5123456789012), new CLTypeInfo(CLType.String));
            Assert.AreEqual(new BigInteger(5123456789012), clValue.ToBigInteger());
            Assert.AreEqual(new BigInteger(5123456789012), (BigInteger) clValue);
        }

        [Test]
        public void CValueToUnit()
        {
            var clValue = CLValue.Unit();
            
            Assert.IsNotNull(clValue);
            Assert.AreEqual(Unit.Default, clValue.ToUnit());
            Assert.AreEqual(Unit.Default, (Unit)clValue);

            clValue = CLValue.Ok(CLValue.Unit(), CLType.String);
            
            Assert.IsNotNull(clValue);
            Assert.AreEqual(Unit.Default, clValue.ToUnit());
            Assert.AreEqual(Unit.Default, (Unit)clValue);
        }
        
        [Test]
        public void CLValueToString()
        {
            var clValue = CLValue.String("Casper Network!");
            Assert.AreEqual("Casper Network!", clValue.ToString());
            Assert.AreEqual("Casper Network!", (string) clValue);

            clValue = CLValue.Bool(true);
            Assert.AreEqual(true.ToString(), (string) clValue);

            clValue = CLValue.I32(Int32.MinValue);
            Assert.AreEqual(Int32.MinValue.ToString(), (string) clValue);

            clValue = CLValue.I64(Int64.MinValue);
            Assert.AreEqual(Int64.MinValue.ToString(), (string) clValue);

            clValue = CLValue.U8(byte.MaxValue);
            Assert.AreEqual(byte.MaxValue.ToString(), (string) clValue);

            clValue = CLValue.U32(uint.MaxValue);
            Assert.AreEqual(uint.MaxValue.ToString(), (string) clValue);

            clValue = CLValue.U64(ulong.MaxValue);
            Assert.AreEqual(ulong.MaxValue.ToString(), (string) clValue);

            clValue = CLValue.U128(BigInteger.One);
            Assert.AreEqual(BigInteger.One.ToString(), (string) clValue);

            var uref = new URef("uref-82e4a481ec5d015ad519d0d7800b4484aa2c830ba24dd166aec81ecde890dc74-007");
            clValue = CLValue.URef(uref);
            Assert.AreEqual(uref.ToString(), (string) clValue);

            var pk = PublicKey.FromHexString("01b7c7c545dfa3fb853a97fb3581ce10eb4f67a5861abed6e70e5e3312fdde402c");
            clValue = CLValue.PublicKey(pk);
            Assert.AreEqual(pk.ToAccountHex(), (string) clValue);

            var gs1 = GlobalStateKey.FromString(
                "account-hash-989ca079a5e446071866331468ab949483162588d57ec13ba6bb051f1e15f8b7");
            clValue = CLValue.Key(gs1);
            Assert.AreEqual(gs1.ToString(), (string) clValue);

            clValue = CLValue.Key(uref);
            Assert.AreEqual(uref.ToString(), (string) clValue);

            clValue = CLValue.Ok(CLValue.String("Casper Network!"), new CLTypeInfo(CLType.String));
            Assert.AreEqual("Casper Network!", clValue.ToString());
            Assert.AreEqual("Casper Network!", (string) clValue);

            clValue = CLValue.Ok(CLValue.Key(uref), new CLTypeInfo(CLType.String));
            Assert.AreEqual(uref.ToString(), (string) clValue);
        }

        [Test]
        public void CLValueToURef()
        {
            var uref = new URef("uref-82e4a481ec5d015ad519d0d7800b4484aa2c830ba24dd166aec81ecde890dc74-007");
            var clValue = CLValue.URef(uref);
            Assert.IsTrue(uref.RawBytes.SequenceEqual(clValue.ToURef().RawBytes));
            Assert.AreEqual(uref.AccessRights, ((URef) clValue).AccessRights);

            clValue = CLValue.Ok(CLValue.Key(uref), new CLTypeInfo(CLType.String));
            Assert.IsTrue(uref.RawBytes.SequenceEqual(clValue.ToURef().RawBytes));
            Assert.AreEqual(uref.AccessRights, ((URef) clValue).AccessRights);
        }

        [Test]
        public void CLValueToPublicKey()
        {
            var pk = PublicKey.FromHexString("01b7c7c545dfa3fb853a97fb3581ce10eb4f67a5861abed6e70e5e3312fdde402c");
            var clValue = CLValue.PublicKey(pk);
            Assert.AreEqual(pk.ToAccountHex(), clValue.ToPublicKey().ToAccountHex());
            Assert.AreEqual(pk.ToAccountHex(), ((PublicKey) clValue).ToAccountHex());

            pk = PublicKey.FromHexString("02037292af42f13f1f49507c44afe216b37013e79a062d7e62890f77b8adad60501e");
            clValue = CLValue.PublicKey(pk);
            Assert.AreEqual(pk.ToAccountHex(), clValue.ToPublicKey().ToAccountHex());
            Assert.AreEqual(pk.ToAccountHex(), ((PublicKey) clValue).ToAccountHex());

            clValue = CLValue.Ok(CLValue.PublicKey(pk), new CLTypeInfo(CLType.String));
            Assert.AreEqual(pk.ToAccountHex(), clValue.ToPublicKey().ToAccountHex());
            Assert.AreEqual(pk.ToAccountHex(), ((PublicKey) clValue).ToAccountHex());
        }

        [Test]
        public void CLValueToGlobalStateKey()
        {
            var gs1 = GlobalStateKey.FromString(
                "account-hash-989ca079a5e446071866331468ab949483162588d57ec13ba6bb051f1e15f8b7");
            var clValue = CLValue.Key(gs1);
            Assert.AreEqual(gs1.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs1.ToString(), ((GlobalStateKey) clValue).ToString());

            var gs2 = GlobalStateKey.FromString(
                "uref-a465baf86b29c9d12b643b32beef2b9acf55dd4a820d59281ba5a1cf131ee796-000");
            clValue = CLValue.Key(gs2);
            Assert.AreEqual(gs2.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs2.ToString(), ((GlobalStateKey) clValue).ToString());

            var gs3 = GlobalStateKey.FromString("era-3");
            clValue = CLValue.Key(gs3);
            Assert.AreEqual(gs3.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs3.ToString(), ((GlobalStateKey) clValue).ToString());

            clValue = CLValue.Ok(CLValue.Key(gs1), new CLTypeInfo(CLType.String));
            Assert.AreEqual(gs1.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs1.ToString(), ((GlobalStateKey) clValue).ToString());

            clValue = CLValue.Ok(CLValue.Key(gs2), new CLTypeInfo(CLType.String));
            Assert.AreEqual(gs2.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs2.ToString(), ((GlobalStateKey) clValue).ToString());

            clValue = CLValue.Ok(CLValue.Key(gs3), new CLTypeInfo(CLType.String));
            Assert.AreEqual(gs3.ToString(), clValue.ToGlobalStateKey().ToString());
            Assert.AreEqual(gs3.ToString(), ((GlobalStateKey) clValue).ToString());
        }

        [Test]
        public void CLValueToList()
        {
            var clValue = CLValue.List(new CLValue[]
                {CLValue.U8(0x10), CLValue.U8(0x20), CLValue.U8(0x30), CLValue.U8(0x40)});
            var list0 = clValue.ToList<byte>();
            Assert.AreEqual(0x10, list0[0]);
            Assert.AreEqual(0x20, list0[1]);
            Assert.AreEqual(0x30, list0[2]);
            Assert.AreEqual(0x40, list0[3]);

            var pk1 = PublicKey.FromHexString("01b7c7c545dfa3fb853a97fb3581ce10eb4f67a5861abed6e70e5e3312fdde402c");
            var pk2 = PublicKey.FromHexString("02037292af42f13f1f49507c44afe216b37013e79a062d7e62890f77b8adad60501e");
            var pk3 = PublicKey.FromHexString("011970eb3d16d0ef7f3d381c0e5fbe980a03eaea7fa40fad710c66657fcfbc9677");
            clValue = CLValue.List(new CLValue[]
            {
                CLValue.PublicKey(pk1),
                CLValue.PublicKey(pk2),
                CLValue.PublicKey(pk3)
            });
            var list = clValue.ToList();
            Assert.AreEqual(pk1.ToAccountHex(), ((PublicKey) list[0]).ToAccountHex());
            Assert.AreEqual(pk2.ToAccountHex(), ((PublicKey) list[1]).ToAccountHex());
            Assert.AreEqual(pk3.ToAccountHex(), ((PublicKey) list[2]).ToAccountHex());

            var gs1 = new AccountHashKey(
                "account-hash-989ca079a5e446071866331468ab949483162588d57ec13ba6bb051f1e15f8b7");
            var gs2 = new URef("uref-a465baf86b29c9d12b643b32beef2b9acf55dd4a820d59281ba5a1cf131ee796-000");
            var gs3 = new EraInfoKey("era-3");
            clValue = CLValue.List(new CLValue[]
            {
                CLValue.Key(gs1),
                CLValue.Key(gs2),
                CLValue.Key(gs3)
            });
            list = clValue.ToList();
            Assert.AreEqual(gs1.ToString(), ((GlobalStateKey) list[0]).ToString());
            Assert.AreEqual(gs2.ToString(), ((GlobalStateKey) list[1]).ToString());
            Assert.AreEqual(gs3.ToString(), ((GlobalStateKey) list[2]).ToString());

            clValue = CLValue.Ok(clValue, new CLTypeInfo(CLType.String));
            list = clValue.ToList();
            Assert.AreEqual(gs1.ToString(), ((GlobalStateKey) list[0]).ToString());
            Assert.AreEqual(gs2.ToString(), ((GlobalStateKey) list[1]).ToString());
            Assert.AreEqual(gs3.ToString(), ((GlobalStateKey) list[2]).ToString());
        }

        [Test]
        public void CLValueToByteArray()
        {
            var bytes = Hex.Decode("00010203040506070809");
            var clValue = CLValue.ByteArray(bytes);
            Assert.IsTrue(bytes.SequenceEqual(clValue.ToByteArray()));
            Assert.IsTrue(bytes.SequenceEqual((byte[]) clValue));

            clValue = CLValue.Ok(clValue, new CLTypeInfo(CLType.String));
            Assert.IsTrue(bytes.SequenceEqual(clValue.ToByteArray()));
            Assert.IsTrue(bytes.SequenceEqual((byte[]) clValue));
        }

        [Test]
        public void CLValueToDictionary()
        {
            var dict = new Dictionary<CLValue, CLValue>()
            {
                {CLValue.String("fourteen"), CLValue.U8(14)},
                {CLValue.String("fifteen"), CLValue.U8(15)},
                {CLValue.String("sixteen"), CLValue.U8(16)},
            };
            var clValue = CLValue.Map(dict);

            var map = clValue.ToDictionary();
            Assert.AreEqual(14, map["fourteen"]);
            Assert.AreEqual(15, map["fifteen"]);
            Assert.AreEqual(16, map["sixteen"]);

            clValue = CLValue.Ok(clValue, new CLTypeInfo(CLType.String));
            map = clValue.ToDictionary();
            Assert.AreEqual(14, map["fourteen"]);
            Assert.AreEqual(15, map["fifteen"]);
            Assert.AreEqual(16, map["sixteen"]);
        }

        [Test]
        public void CLResultWithErrorTest()
        {
            var clValue = CLValue.Err(CLValue.String("Error 1"), new CLTypeInfo(CLType.U8));

            var ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(U8,String) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals("Error 1"));

            clValue = CLValue.Err(CLValue.I32(-1), new CLTypeInfo(CLType.String));
            ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(String,I32) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals(-1));

            clValue = CLValue.Err(CLValue.I32(int.MinValue), new CLKeyTypeInfo(KeyIdentifier.Account));
            ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(Key(Account),I32) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals(int.MinValue));

            clValue = CLValue.Err(CLValue.I32(int.MaxValue), new CLOptionTypeInfo(new CLTypeInfo(CLType.String)));
            ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(Option(String),I32) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals(int.MaxValue));

            clValue = CLValue.Err(CLValue.I32(int.MaxValue), new CLListTypeInfo(new CLTypeInfo(CLType.String)));
            ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(List(String),I32) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals(int.MaxValue));

            clValue = CLValue.Err(CLValue.I32(int.MaxValue),
                new CLMapTypeInfo(new CLTypeInfo(CLType.String), new CLOptionTypeInfo(new CLTypeInfo(CLType.U512))));
            ex = Assert.Catch<CLValueException>(() => clValue.ToByte());
            Assert.IsTrue(ex?.Message.Equals("Result(Map(String,Option(U512)),I32) variable contains an error."));
            Assert.IsTrue(ex?.ErrorValue.Equals(int.MaxValue));
        }

        [Test]
        public void CLValueToResultTest()
        {
            var cl1 = CLValue.I64(long.MaxValue);
            var clValue = CLValue.Ok(cl1, CLType.String);

            var result = clValue.ToResult<long, string>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.IsFailure);
            Assert.IsNull(result.Error);
            Assert.AreEqual(long.MaxValue, result.Value);

            var dict = new Dictionary<CLValue, CLValue>()
            {
                {CLValue.String("fourteen"), CLValue.U8(14)},
                {CLValue.String("fifteen"), CLValue.U8(15)},
                {CLValue.String("sixteen"), CLValue.U8(16)},
            };
            clValue = CLValue.Ok(CLValue.Map(dict), CLType.String);

            var res2 = clValue.ToResult<IDictionary, string>();
            Assert.IsNotNull(res2);

            var gs1 = new AccountHashKey(
                "account-hash-989ca079a5e446071866331468ab949483162588d57ec13ba6bb051f1e15f8b7");
            var gs2 = new URef("uref-a465baf86b29c9d12b643b32beef2b9acf55dd4a820d59281ba5a1cf131ee796-000");
            var gs3 = new EraInfoKey("era-3");
            clValue = CLValue.Ok(CLValue.List(new CLValue[]
            {
                CLValue.Key(gs1),
                CLValue.Key(gs2),
                CLValue.Key(gs3)
            }), CLType.String);

            var res3 = clValue.ToResult<IList, string>();
            Assert.IsNotNull(res3);

            clValue = CLValue.Err(CLValue.List(new CLValue[]
            {
                CLValue.Key(gs1),
                CLValue.Key(gs2),
                CLValue.Key(gs3)
            }), CLType.String);

            var res4 = clValue.ToResult<string, List<GlobalStateKey>>();
            Assert.IsNotNull(res4);
            Assert.IsFalse(res4.Success);
            Assert.IsTrue(res4.IsFailure);
            Assert.AreEqual(default, res4.Value);
            Assert.AreEqual(3, res4.Error.Count);
            Assert.AreEqual(gs1.ToHexString(), res4.Error[0].ToHexString());
            Assert.AreEqual(gs2.ToHexString(), res4.Error[1].ToHexString());
            Assert.AreEqual(gs3.ToHexString(), res4.Error[2].ToHexString());
        }

        [Test]
        public void CLValueToTuple1Test()
        {
            var cl1 = CLValue.Map(new Dictionary<CLValue, CLValue>()
            {
                {CLValue.String("fourteen"), CLValue.U8(14)},
                {CLValue.String("fifteen"), CLValue.U8(15)},
                {CLValue.String("sixteen"), CLValue.U8(16)},
            });
            var clValue = CLValue.Tuple1(cl1);

            var tuple = clValue.ToTuple1<Dictionary<string, byte>>();
            Assert.IsNotNull(tuple);

            var dict = tuple.Item1;
            Assert.AreEqual(14, dict["fourteen"]);
            Assert.AreEqual(15, dict["fifteen"]);
            Assert.AreEqual(16, dict["sixteen"]);
        }

        [Test]
        public void CLValueToTuple2Test()
        {
            var cl1 = CLValue.U32(uint.MaxValue);
            var cl2 = CLValue.List(new[]
            {
                CLValue.U8(0x010), CLValue.U8(0x020)
            });
            var clValue = CLValue.Tuple2(cl1, cl2);

            var tuple = clValue.ToTuple2<uint, List<byte>>();
            Assert.IsNotNull(tuple);
            Assert.AreEqual(uint.MaxValue, tuple.Item1);

            var list = tuple.Item2;
            Assert.IsNotNull(list);
            Assert.AreEqual(0x10, list[0]);
            Assert.AreEqual(0x20, list[1]);
        }


        [Test]
        public void CLValueToTuple3Test()
        {
            var cl1 = CLValue.Map(new Dictionary<CLValue, CLValue>()
            {
                {CLValue.String("fourteen"), CLValue.U8(14)},
                {CLValue.String("fifteen"), CLValue.U8(15)},
                {CLValue.String("sixteen"), CLValue.U8(16)},
            });
            var cl2 = CLValue.U32(uint.MaxValue);
            var cl3 = CLValue.List(new[]
            {
                CLValue.U8(0x010), CLValue.U8(0x020)
            });
            var clValue = CLValue.Tuple3(cl1, cl2, cl3);

            var tuple = clValue.ToTuple3<Dictionary<string, byte>, uint, List<byte>>();
            Assert.IsNotNull(tuple);

            var dict = tuple.Item1;
            Assert.AreEqual(14, dict["fourteen"]);
            Assert.AreEqual(15, dict["fifteen"]);
            Assert.AreEqual(16, dict["sixteen"]);
            
            Assert.AreEqual(uint.MaxValue, tuple.Item2);

            var list = tuple.Item3;
            Assert.IsNotNull(list);
            Assert.AreEqual(0x10, list[0]);
            Assert.AreEqual(0x20, list[1]);
        }
    }
}