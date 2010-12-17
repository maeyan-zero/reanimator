using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Revival.Common
{
    public class ObjectDelegator
    {
        public delegate Object FieldGetValueDelegate(Object target);
        private readonly FieldGetValueDelegate[] _fieldGetValueDelegates;

        private delegate void FieldSetValueDelegate(Object target, Object value);
        private readonly FieldSetValueDelegate[] _fieldSetValueDelegates;

        public ObjectDelegator(Type type, String field)
        {
            FieldInfo[] fieldInfos = type.GetFields();
            switch (field)
            {
                case "GetValue":
                    _fieldGetValueDelegates = new FieldGetValueDelegate[fieldInfos.Length];
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        _fieldGetValueDelegates[i] = _CreateGetField(fieldInfos[i]);
                    }
                    break;

                case "SetValue":
                    _fieldSetValueDelegates = new FieldSetValueDelegate[fieldInfos.Length];
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        _fieldSetValueDelegates[i] = _CreateSetField(fieldInfos[i]);
                    }
                    break;

                default:
                    throw new NotSupportedException("The field " + field + "is not supported!");
            }
        }

        private static FieldSetValueDelegate _CreateSetField(FieldInfo field)
        {
            // create our custom delegate method
            DynamicMethod setMethod = new DynamicMethod("SetValue", typeof(void), new[] { typeof(Object), typeof(Object) }, field.DeclaringType);
            ILGenerator ilGenerator = setMethod.GetILGenerator();

            // push the object to read, cast to our type, then push the value to set to
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, field.DeclaringType);
            ilGenerator.Emit(OpCodes.Ldarg_1);

            // if the field is a primitive then we need to unbox it
            if (field.FieldType.IsValueType) ilGenerator.Emit(OpCodes.Unbox_Any, field.FieldType);

            // set the field
            ilGenerator.Emit(OpCodes.Stfld, field);

            // return the value
            ilGenerator.Emit(OpCodes.Ret);

            return (FieldSetValueDelegate)setMethod.CreateDelegate(typeof(FieldSetValueDelegate));
        }

        private static FieldGetValueDelegate _CreateGetField(FieldInfo field)
        {
            // create our custom delegate method
            DynamicMethod getMethod = new DynamicMethod("GetValue", typeof(Object), new[] { typeof(Object) }, field.DeclaringType);
            ILGenerator ilGenerator = getMethod.GetILGenerator();

            // push the object to read, cast to our type, and get the field
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, field.DeclaringType);
            ilGenerator.Emit(OpCodes.Ldfld, field);

            // if the field is a primitive then we need to box it
            if (field.FieldType.IsValueType) ilGenerator.Emit(OpCodes.Box, field.FieldType);

            // return the value
            ilGenerator.Emit(OpCodes.Ret);

            return (FieldGetValueDelegate)getMethod.CreateDelegate(typeof(FieldGetValueDelegate));
        }

        public FieldGetValueDelegate this[int index]
        {
            get
            {
                if (index < 0 || index > _fieldGetValueDelegates.Length) return null;

                return _fieldGetValueDelegates[index];
            }
        }

        public Object this[int index, Object target]
        {
            set
            {
                if (index < 0 || index > _fieldSetValueDelegates.Length) return;

                _fieldSetValueDelegates[index](target, value);
            }
        }
    }
}