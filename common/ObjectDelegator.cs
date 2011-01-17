using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Revival.Common
{
    public class ObjectDelegator : IEnumerable
    {
        public delegate Object FieldGetValueDelegate(Object target);
        public delegate void FieldSetValueDelegate(Object target, Object value);

        public class FieldDelegate
        {
            public String Name;
            public FieldGetValueDelegate GetValue;
            public FieldSetValueDelegate SetValue;
            public FieldInfo Info;

            public Type FieldType { get { return Info.FieldType; } }
            public bool IsPublic { get { return Info.IsPublic; } }
            public bool IsPrivate { get { return Info.IsPrivate; } }
        }

        public class DelegatorEnumerator : IEnumerator
        {
            private readonly Dictionary<String, FieldDelegate> _delegatesDict;
            private Dictionary<String, FieldDelegate>.Enumerator _delegatesEnumerator;

            public DelegatorEnumerator(Dictionary<String, FieldDelegate> delegatesDict)
            {
                _delegatesDict = delegatesDict;
                Reset();
            }

            public bool MoveNext()
            {
                return _delegatesEnumerator.MoveNext();
            }

            public void Reset()
            {
                _delegatesEnumerator = _delegatesDict.GetEnumerator();
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public FieldDelegate Current
            {
                get
                {
                    try
                    {
                        return _delegatesEnumerator.Current.Value;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        private readonly Dictionary<String, FieldDelegate> _fieldDelegatesDict = new Dictionary<String, FieldDelegate>();

        public ObjectDelegator(IEnumerable<FieldInfo> fieldInfos)
        {
            if (fieldInfos == null) throw new ArgumentNullException("fieldInfos", "Cannot be null!");

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                AddField(fieldInfo);
            }
        }

        public void AddField(FieldInfo fieldInfo)
        {
            if (_fieldDelegatesDict.ContainsKey(fieldInfo.Name)) return;

            FieldDelegate fieldDelegate = new FieldDelegate
            {
                Name = fieldInfo.Name,
                GetValue = _CreateGetField(fieldInfo),
                SetValue = _CreateSetField(fieldInfo),
                Info = fieldInfo
            };

            _fieldDelegatesDict.Add(fieldInfo.Name, fieldDelegate);
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

        public int FieldCount
        {
            get { return _fieldDelegatesDict.Count; }
        }

        // "getter"
        public FieldGetValueDelegate this[String fieldName]
        {
            get
            {
                return GetFieldGetDelegate(fieldName);
            }
        }

        // "setter"
        public Object this[String fieldName, Object target]
        {
            set
            {
                FieldSetValueDelegate fieldSetDelegate = GetFieldSetDelegate(fieldName);
                if (fieldSetDelegate != null) fieldSetDelegate(target, value);
            }
        }

        public FieldDelegate GetFieldDelegate(String fieldName)
        {
            FieldDelegate fieldDelegate;
            return _fieldDelegatesDict.TryGetValue(fieldName, out fieldDelegate) ? fieldDelegate : null;
        }

        public FieldGetValueDelegate GetFieldGetDelegate(String fieldName)
        {
            FieldDelegate fieldDelegate;
            return _fieldDelegatesDict.TryGetValue(fieldName, out fieldDelegate) ? fieldDelegate.GetValue : null;
        }

        public FieldSetValueDelegate GetFieldSetDelegate(String fieldName)
        {
            FieldDelegate fieldDelegate;
            return _fieldDelegatesDict.TryGetValue(fieldName, out fieldDelegate) ? fieldDelegate.SetValue : null;
        }

        public bool ContainsGetFieldDelegate(String fieldName)
        {
            return _fieldDelegatesDict.ContainsKey(fieldName);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DelegatorEnumerator GetEnumerator()
        {
            return new DelegatorEnumerator(_fieldDelegatesDict);
        }
    }
}