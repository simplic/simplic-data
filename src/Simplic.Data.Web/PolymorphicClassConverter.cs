using Newtonsoft.Json;
using System;

namespace Simplic.Data.Web
{
    // The original solotion is from: https://stackoverflow.com/questions/32314638/signalr-typenamehandling/64890938#64890938
    // with the following answer used https://stackoverflow.com/a/40279480
    // Also some things where removed since they where unused.

    /// <summary>
    /// Json converter for polymorphic classes. 
    /// The converter is meant to be used in attributes only.
    /// </summary>
    public class PolymorphicClassConverter : JsonConverter
    {
        [ThreadStatic]
        static bool disabled;

        // Disables the converter in a thread-safe manner.
        private bool Disabled { get { return disabled; } set { disabled = value; } }

        public override bool CanWrite { get { return !Disabled; } }

        public override bool CanRead { get { return !Disabled; } }

        public override bool CanConvert(Type objectType)
        {
            // This can throw an exception since it is not meant to be called when the converter is used as an 
            // attribute
            throw new Exception();
        }

        /// <summary>
        /// Deserializes a json object.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            // Prevent infinite recursion of converters
            using (new PushValue<bool>(true, () => Disabled, val => Disabled = val))

            using (new PushValue<TypeNameHandling>(TypeNameHandling.Auto,
                                                   () => serializer.TypeNameHandling,
                                                   val => serializer.TypeNameHandling = val))
            {
                return serializer.Deserialize(reader, objectType);
            }
        }

        /// <summary>
        /// Serializes a json object.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Prevent infinite recursion of converters
            using (new PushValue<bool>(true, () => Disabled, val => Disabled = val))

            using (new PushValue<TypeNameHandling>(TypeNameHandling.Auto,
                                                   () => serializer.TypeNameHandling,
                                                   val => serializer.TypeNameHandling = val))
            {
                // Force the $type to be written unconditionally by passing typeof(object) as the type being serialized.
                serializer.Serialize(writer, value, typeof(object));
            }
        }

        /// <summary>
        /// Struct used just in <see cref="PolymorphicClassConverter"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private struct PushValue<T> : IDisposable
        {
            // Also this struct comes from the stackoverflow question mentioned in the top of this document.

            Action<T> setValue;
            T oldValue;

            public PushValue(T value, Func<T> getValue, Action<T> setValue)
            {
                if (getValue == null || setValue == null)
                    throw new ArgumentNullException();
                this.setValue = setValue;
                this.oldValue = getValue();
                setValue(value);
            }

            // By using a disposable struct we avoid the overhead of allocating and freeing an instance
            // of a finalizable class.
            public void Dispose()
            {
                if (setValue != null)
                    setValue(oldValue);
            }
        }
    }
}
