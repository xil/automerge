﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MergeLib.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MergeLib.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ================================    FileA    ================================.
        /// </summary>
        internal static string divLineA {
            get {
                return ResourceManager.GetString("divLineA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ================================    FileB    ================================.
        /// </summary>
        internal static string divLineB {
            get {
                return ResourceManager.GetString("divLineB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ================================ Overlapping ================================.
        /// </summary>
        internal static string divLineBegin {
            get {
                return ResourceManager.GetString("divLineBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ================================     End     ================================.
        /// </summary>
        internal static string divLineEnd {
            get {
                return ResourceManager.GetString("divLineEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ================================    FileO    ================================.
        /// </summary>
        internal static string divLineO {
            get {
                return ResourceManager.GetString("divLineO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong file path in args[].
        /// </summary>
        internal static string FileNotFound_Exception {
            get {
                return ResourceManager.GetString("FileNotFound_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Слияние, {0} из {1} строк.
        /// </summary>
        internal static string Merging {
            get {
                return ResourceManager.GetString("Merging", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Слияние файлов завершено.
        /// </summary>
        internal static string MergingDone {
            get {
                return ResourceManager.GetString("MergingDone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В файлах-потомках изменения пересекаются..
        /// </summary>
        internal static string message_Overlapping {
            get {
                return ResourceManager.GetString("message_Overlapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Анализ, {0} из {1} строк.
        /// </summary>
        internal static string Parsing {
            get {
                return ResourceManager.GetString("Parsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Анализ файлов завершен.
        /// </summary>
        internal static string ParsingDone {
            get {
                return ResourceManager.GetString("ParsingDone", resourceCulture);
            }
        }
    }
}
