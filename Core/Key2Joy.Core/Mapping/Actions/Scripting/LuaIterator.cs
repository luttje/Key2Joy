using System.Collections;

namespace Key2Joy.Mapping.Actions.Scripting
{
    public class LuaIterator
    {
        public const int INDEX_START = 1; // Lua is 1-based

        private readonly int index;
        private readonly IEnumerator enumerator;

        public LuaIterator(ICollection data)
        {
            this.index = INDEX_START;
            this.enumerator = data.GetEnumerator();
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
            if (this.enumerator.MoveNext())
            {
                value = this.enumerator.Current;
                return this.index;
            }

            value = null;
            return null;
        }
    }
}
