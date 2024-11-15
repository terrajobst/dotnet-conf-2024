﻿using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

using DotnetRelease;

List<ModelInfo> models = [
    new (typeof(MajorReleasesIndex), "dotnet-releases-index.json"),
    new (typeof(MajorReleaseOverview), "dotnet-releases.json"),
    new (typeof(PatchReleasesIndex), "dotnet-patch-releases-index.json"),
    new (typeof(PatchReleaseOverview), "dotnet-patch-release.json"),
    new (typeof(OSPackagesOverview), "dotnet-os-packages.json"),
    new (typeof(SupportedOSMatrix), "dotnet-supported-os-matrix.json"),
];

var serializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
{
    PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower
};

var exporterOptions = new JsonSchemaExporterOptions()
{

    TransformSchemaNode = (ctx, schema) =>
    {
        if (schema is not JsonObject schemaObj || schemaObj.ContainsKey("$ref"))
        {
            return schema;
        }

        DescriptionAttribute? descriptionAttribute =
            GetCustomAttribute<DescriptionAttribute>(ctx.PropertyInfo?.AttributeProvider) ??
            GetCustomAttribute<DescriptionAttribute>(ctx.PropertyInfo?.AssociatedParameter?.AttributeProvider) ??
            GetCustomAttribute<DescriptionAttribute>(ctx.TypeInfo.Type);

        if (descriptionAttribute != null)
        {
            schemaObj.Insert(0, "description", (JsonNode)descriptionAttribute.Description);
        }

        return schemaObj;
    }

};

foreach (var model in models)
{
    WriteSchema(model);
}

void WriteSchema(ModelInfo modelInfo)
{
    var (type, targetFile) = modelInfo;
    var schema = JsonSchemaExporter.GetJsonSchemaAsNode(serializerOptions, type, exporterOptions);
    File.WriteAllText(targetFile, schema.ToString());
}

static TAttribute? GetCustomAttribute<TAttribute>(ICustomAttributeProvider? provider, bool inherit = false) where TAttribute : Attribute
    => provider?.GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault() as TAttribute;

record ModelInfo(Type Type, string TargetFile);