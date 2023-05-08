/* 
 * Define a partial class here to maintain anonymity of our API keys.
 * Make sure APIKeysLocal.cs is added to .gitignore
 */

namespace StockTracker
{
    public static partial class APIKeys
    {
        /// <summary>
        /// API token that can be pushed/published
        /// </summary>
        public static readonly string tokenPublishable = "";
        /// <summary>
        /// Secret API token, should not be shared
        /// </summary>
        public static readonly string tokenSecret = "";
    }
}
