/*
 * These are the keys used to access the endpoints on iexcloud.io
 * Make sure this file is in our .gitignore. If the tokenSecret is pushed at any 
 * point, generate a new token at https://iexcloud.io/console/tokens
 */

namespace StockTracker
{
    public static partial class APIKeys
    {
        static APIKeys()
        {
            tokenPublishable = "";
            tokenSecret = "";
        }
    }
}
