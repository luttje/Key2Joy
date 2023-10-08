using System.Collections;

namespace Key2Joy.Mapping
{
    public class LuaIterator
    {
        public const int INDEX_START = 1; // Lua is 1-based

        private int index;
        private IEnumerator enumerator;

        public LuaIterator(ICollection data)
        {
            index = INDEX_START;
            enumerator = data.GetEnumerator();
        }

        /// <summary>
        /// Lua iterator 'Next' implementation. Gets the collection and current index
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// local devices = Midi.InputDeviceGetAll() -- Requires Key2Joy.Plugin.Midi
        /// for k, v in collection(devices) do
        ///     print(v)
        /// end
        /// ]]>
        /// </example>
        /// <param name="collection">Collection to be iterated (always null? :/)</param>
        /// <param name="currentIndex">Null if first call, the current index otherwise</param>
        /// <param name="value"></param>
        /// <returns>The key/index of the collection</returns>
        public object Next(object collection, int? currentIndex, out object value)
        {
            if (enumerator.MoveNext())
            {
                value = enumerator.Current;
                return index;
            }

            value = null;
            return null;
        }
    }
}
