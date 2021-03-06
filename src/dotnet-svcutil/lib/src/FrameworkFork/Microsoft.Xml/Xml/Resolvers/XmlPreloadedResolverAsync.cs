// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Xml;
using System.Threading.Tasks;

namespace Microsoft.Xml.Resolvers
{
    // 
    // XmlPreloadedResolver is an XmlResolver that which can be pre-loaded with data.
    // By default it contains well-known DTDs for XHTML 1.0 and RSS 0.91. 
    // Custom mappings of URIs to data can be added with the Add method.
    //
    public partial class XmlPreloadedResolver : XmlResolver
    {
        public override Task<Object> GetEntityAsync(Uri absoluteUri,
                                             string role,
                                             Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }

            PreloadedData data;
            if (!_mappings.TryGetValue(absoluteUri, out data))
            {
                if (_fallbackResolver != null)
                {
                    return _fallbackResolver.GetEntityAsync(absoluteUri, role, ofObjectToReturn);
                }
                throw new XmlException(string.Format(ResXml.Xml_CannotResolveUrl, absoluteUri.ToString()));
            }

            if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream) || ofObjectToReturn == typeof(Object))
            {
                return Task.FromResult<Object>(data.AsStream());
            }
            else if (ofObjectToReturn == typeof(TextReader))
            {
                return Task.FromResult<Object>(data.AsTextReader());
            }
            else
            {
                throw new XmlException(ResXml.Xml_UnsupportedClass);
            }
        }
    }
}
