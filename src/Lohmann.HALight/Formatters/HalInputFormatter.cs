using System;
using System.Reflection;
using Lohmann.HALight.Formatters.Internal;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Lohmann.HALight.Formatters
{
    public class HalInputFormatter : JsonInputFormatter
    {
        public HalInputFormatter()
            :this(HalJsonSerializerSettingsProvider.CreateSerializerSettings())
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(HalJsonSerializerSettingsProvider.HalMediaType));
        }

        public HalInputFormatter(JsonSerializerSettings serializerSettings)
            : base(HalJsonSerializerSettingsProvider.AppendHalConverters(serializerSettings))
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(HalJsonSerializerSettingsProvider.HalMediaType));
        }

        protected override bool CanReadType(Type type)
        {
            return typeof (IResource).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }
    }
}