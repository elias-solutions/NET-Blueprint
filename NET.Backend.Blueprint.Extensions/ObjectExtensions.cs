﻿using System.Text.Json;

namespace NET.Backend.Blueprint.Extensions;

public static class ObjectExtensions
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
    public static string ToJson(this object value)
    {
        if (value == null)
        {
            throw new ArgumentException($"Argument '{nameof(value)}' null");
        }

        return JsonSerializer.Serialize(value, Options);
    }
}