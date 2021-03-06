using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace StardewModdingAPI.Framework.Content
{
    internal class AssetInfo : IAssetInfo
    {
        /*********
        ** Fields
        *********/
        /// <summary>Normalises an asset key to match the cache key.</summary>
        protected readonly Func<string, string> GetNormalisedPath;


        /*********
        ** Accessors
        *********/
        /// <summary>The content's locale code, if the content is localised.</summary>
        public string Locale { get; }

        /// <summary>The normalised asset name being read. The format may change between platforms; see <see cref="AssetNameEquals"/> to compare with a known path.</summary>
        public string AssetName { get; }

        /// <summary>The content data type.</summary>
        public Type DataType { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="locale">The content's locale code, if the content is localised.</param>
        /// <param name="assetName">The normalised asset name being read.</param>
        /// <param name="type">The content type being read.</param>
        /// <param name="getNormalisedPath">Normalises an asset key to match the cache key.</param>
        public AssetInfo(string locale, string assetName, Type type, Func<string, string> getNormalisedPath)
        {
            this.Locale = locale;
            this.AssetName = assetName;
            this.DataType = type;
            this.GetNormalisedPath = getNormalisedPath;
        }

        /// <summary>Get whether the asset name being loaded matches a given name after normalisation.</summary>
        /// <param name="path">The expected asset path, relative to the game's content folder and without the .xnb extension or locale suffix (like 'Data\ObjectInformation').</param>
        public bool AssetNameEquals(string path)
        {
            path = this.GetNormalisedPath(path);
            return this.AssetName.Equals(path, StringComparison.InvariantCultureIgnoreCase);
        }


        /*********
        ** Protected methods
        *********/
        /// <summary>Get a human-readable type name.</summary>
        /// <param name="type">The type to name.</param>
        protected string GetFriendlyTypeName(Type type)
        {
            // dictionary
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] genericArgs = type.GetGenericArguments();
                return $"Dictionary<{this.GetFriendlyTypeName(genericArgs[0])}, {this.GetFriendlyTypeName(genericArgs[1])}>";
            }

            // texture
            if (type == typeof(Texture2D))
                return type.Name;

            // native type
            if (type == typeof(int))
                return "int";
            if (type == typeof(string))
                return "string";

            // default
            return type.FullName;
        }
    }
}
