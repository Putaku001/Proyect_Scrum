using System.Diagnostics.CodeAnalysis;
using Proyecto_A.Models;

namespace Proyecto_A.Utilities;

/// <summary>
/// Project Model Extentions
/// </summary>
public static class ProjectExtentions
{
    /// <summary>
    /// Check if the project is null or new.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this ContenidoModel? contenido)
    {
        return contenido is null || contenido.ContenidoID == 0;
    }

}