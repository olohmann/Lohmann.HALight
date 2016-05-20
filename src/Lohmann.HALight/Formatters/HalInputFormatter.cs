using System;
using System.Buffers;
using System.Reflection;
using Lohmann.HALight.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Lohmann.HALight.Formatters
{
    public class HalInputFormatter : JsonInputFormatter
    {
        public HalInputFormatter(
            ILogger logger)
            : this(
                  logger, 
                  HalJsonSerializerSettingsProvider.CreateSerializerSettings(), 
                  ArrayPool<char>.Shared,
                  new DefaultObjectPoolProvider())
        {
        }
            
        public HalInputFormatter(
            ILogger logger, 
            JsonSerializerSettings serializerSettings, 
            ArrayPool<char> charPool, 
            ObjectPoolProvider objectPoolProvider)
            : base(
                  logger, 
                  HalJsonSerializerSettingsProvider.AppendHalConverters(serializerSettings), 
                  charPool, 
                  objectPoolProvider)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(HalJsonSerializerSettingsProvider.HalMediaType));
        }

        protected override bool CanReadType(Type type)
        {
            return typeof (IResource).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }
    }
}