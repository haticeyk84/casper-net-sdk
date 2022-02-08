using System;

namespace Casper.Network.SDK.Types
{
    /// <summary>
    /// A `CLTypeInfo` specific for the Tuple1 CLType.
    /// </summary>
    public class CLTuple1TypeInfo : CLTypeInfo
    {
        public CLTypeInfo Type0 { get; }
        
        public CLTuple1TypeInfo(CLTypeInfo type0)
            : base(CLType.Tuple1)
        {
            Type0 = type0;
        }
        
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return base.Equals(obj) && this.Type0.Equals(((CLTuple1TypeInfo) obj).Type0);
        }
        
        public override int GetHashCode()
        {
            return (int)Type^(int)1;
        }

        public override string ToString()
        {
            return $"Tuple1({Type0})";
        }

        public override Type GetFrameworkType()
        {
            Type t0Type = this.Type0.GetFrameworkType();

            var resultType = typeof(Tuple<>).MakeGenericType(new[] {t0Type});
            
            return resultType;
        }
    }
    
    /// <summary>
    /// A `CLTypeInfo` specific for the Tuple2 CLType.
    /// </summary>
    public class CLTuple2TypeInfo : CLTypeInfo
    {
        public CLTypeInfo Type0 { get; }
        public CLTypeInfo Type1 { get; }

        public CLTuple2TypeInfo(CLTypeInfo type0, CLTypeInfo type1)
            : base(CLType.Tuple2)
        {
            Type0 = type0;
            Type1 = type1;
        }
        
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return base.Equals(obj) && this.Type0.Equals(((CLTuple2TypeInfo) obj).Type0)
                                    && this.Type1.Equals(((CLTuple2TypeInfo) obj).Type1);
        }
        
        public override int GetHashCode()
        {
            return (int)Type^(int)2;
        }

        public override string ToString()
        {
            return $"Tuple2({Type0},{Type1})";
        }

        public override Type GetFrameworkType()
        {
            Type t0Type = this.Type0.GetFrameworkType();
            Type t1Type = this.Type1.GetFrameworkType();

            var resultType = typeof(Tuple<,>).MakeGenericType(new[] {t0Type, t1Type});
            
            return resultType;
        }
    }
    
    /// <summary>
    /// A `CLTypeInfo` specific for the Tuple3 CLType.
    /// </summary>
    public class CLTuple3TypeInfo : CLTypeInfo
    {
        public CLTypeInfo Type0 { get; }
        public CLTypeInfo Type1 { get; }
        public CLTypeInfo Type2 { get; }

        public CLTuple3TypeInfo(CLTypeInfo type0, CLTypeInfo type1, CLTypeInfo type2)
            : base(CLType.Tuple3)
        {
            Type0 = type0;
            Type1 = type1;
            Type2 = type2;
        }
        
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return base.Equals(obj) && this.Type0.Equals(((CLTuple3TypeInfo) obj).Type0)
                                    && this.Type1.Equals(((CLTuple3TypeInfo) obj).Type1)
                                    && this.Type2.Equals(((CLTuple3TypeInfo) obj).Type2);
        }
        
        public override int GetHashCode()
        {
            return (int)Type^(int)3;
        }

        public override string ToString()
        {
            return $"Tuple3({Type0},{Type1},{Type2})";
        }

        public override Type GetFrameworkType()
        {
            Type t0Type = this.Type0.GetFrameworkType();
            Type t1Type = this.Type1.GetFrameworkType();
            Type t2Type = this.Type2.GetFrameworkType();

            var resultType = typeof(Tuple<,,>).MakeGenericType(new[] {t0Type, t1Type,t2Type});
            
            return resultType;
        }
    }
}
