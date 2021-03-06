﻿using System;
using System.Linq;
using System.Reflection;

namespace RosterManager.Api
{
  internal static class InstalledMods
  {
    private static readonly Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

    internal static bool IsSmInstalled
    {
      get
      {
        return IsModInstalled("ShipManifest");
      }
    }

    internal static bool IsClsInstalled
    {
      get
      {
        return IsModInstalled("ConnectedLivingSpace");
      }
    }

    internal static bool IsDfInstalled
    {
      get
      {
        return IsModInstalled("DeepFreeze");
      }
    }

    internal static bool IsModInstalled(string assemblyName)
    {
      Assembly assembly = (from a in Assemblies
                           where a.FullName.StartsWith(assemblyName)
                           select a).FirstOrDefault();
      return assembly != null;
    }
  }
}