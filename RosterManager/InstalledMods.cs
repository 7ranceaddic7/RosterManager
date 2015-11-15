﻿using System;
using System.Linq;
using System.Reflection;

namespace RosterManager
{
    internal static class InstalledMods
    {
        private static Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        internal static bool IsSMInstalled
        {
            get
            {
                return IsModInstalled("ShipManifest");
            }
        }

        internal static bool IsCLSInstalled
        {
            get
            {
                return IsModInstalled("ConnectedLivingSpace");
            }
        }

        internal static bool IsDFInstalled
        {
            get
            {
                return IsModInstalled("DeepFreeze");
            }
        }

        internal static bool IsModInstalled(string assemblyName)
        {
            Assembly assembly = (from a in assemblies
                                 where a.FullName.StartsWith(assemblyName)
                                 select a).SingleOrDefault();
            return assembly != null;
        }
    }
}