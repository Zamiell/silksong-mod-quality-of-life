using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace RacingMod;

[HarmonyPatch(typeof(GameManager), "LoadedFromMenu")]
public class GameManager_LoadedFromMenu_Patch
{
    static void Postfix()
    {
        if (PlayerData.instance == null)
        {
            return;
        }

        /*
        Debug.Log("PlayerData dump:");
        PrintObject(PlayerData.instance, "", new HashSet<object>(), 0, 3);
        */
    }

    private static void PrintObject(
        object obj,
        string indent,
        HashSet<object> visited,
        int depth,
        int maxDepth
    )
    {
        if (obj == null || depth >= maxDepth)
        {
            return;
        }

        // Prevent infinite loops with circular references.
        if (!obj.GetType().IsValueType && visited.Contains(obj))
        {
            return;
        }

        if (!obj.GetType().IsValueType)
        {
            visited.Add(obj);
        }

        Type type = obj.GetType();

        // Print all fields.
        foreach (
            var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
        )
        {
            try
            {
                object value = field.GetValue(obj);
                PrintValue(field.Name, field.FieldType, value, indent, visited, depth, maxDepth);
            }
            catch (Exception ex)
            {
                Debug.Log($"{indent}{field.Name} ({field.FieldType.Name}) = <Error: {ex.Message}>");
            }
        }

        // Print all properties.
        foreach (
            var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
        )
        {
            try
            {
                object value = property.GetValue(obj);
                PrintValue(
                    property.Name,
                    property.PropertyType,
                    value,
                    indent,
                    visited,
                    depth,
                    maxDepth
                );
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $"{indent}{property.Name} ({property.PropertyType.Name}) = <Error: {ex.Message}>"
                );
            }
        }
    }

    private static void PrintValue(
        string name,
        Type type,
        object value,
        string indent,
        HashSet<object> visited,
        int depth,
        int maxDepth
    )
    {
        if (value == null)
        {
            Debug.Log($"{indent}{name} ({type.Name}) = null");
            return;
        }

        // Handle primitive types and strings.
        if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal))
        {
            Debug.Log($"{indent}{name} ({type.Name}) = {value}");
            return;
        }

        // Handle Unity Vector types.
        if (
            type == typeof(Vector2)
            || type == typeof(Vector3)
            || type == typeof(Vector4)
            || type == typeof(Color)
        )
        {
            Debug.Log($"{indent}{name} ({type.Name}) = {value}");
            return;
        }

        // Handle collections.
        if (value is IEnumerable enumerable && !(value is string))
        {
            IEnumerator enumerator = enumerable.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext() && count < 10)
            {
                count++;
            }

            Debug.Log($"{indent}{name} ({type.Name}) = [Collection with items]");

            if (depth + 1 < maxDepth)
            {
                enumerator = enumerable.GetEnumerator();
                int index = 0;
                while (enumerator.MoveNext() && index < 10)
                {
                    object item = enumerator.Current;
                    if (item != null)
                    {
                        Type itemType = item.GetType();
                        if (
                            itemType.IsPrimitive
                            || itemType.IsEnum
                            || itemType == typeof(string)
                            || itemType == typeof(decimal)
                        )
                        {
                            Debug.Log($"{indent}  [{index}] = {item}");
                        }
                        else
                        {
                            Debug.Log($"{indent}  [{index}] ({itemType.Name}):");
                            PrintObject(item, indent + "    ", visited, depth + 1, maxDepth);
                        }
                    }
                    else
                    {
                        Debug.Log($"{indent}  [{index}] = null");
                    }
                    index++;
                }
            }
            return;
        }

        // Handle complex objects.
        Debug.Log($"{indent}{name} ({type.Name}):");
        PrintObject(value, indent + "  ", visited, depth + 1, maxDepth);
    }
}
