﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stg.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Stg.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Обязательный эелемент {0}, типа Array хранящий тип данных &quot;{1}&quot; не найден в коллекции.
        /// </summary>
        internal static string eArrayNotFound {
            get {
                return ResourceManager.GetString("eArrayNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Запрашиваемый массив хранит другой тип данных.
        ///Запрашивается: {0}
        ///Хранит: {1}.
        /// </summary>
        internal static string eArrayNotFoundInArray {
            get {
                return ResourceManager.GetString("eArrayNotFoundInArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обязательный эелемент {0}, типа Array хранящий тип данных &quot;{1}&quot; не найден в коллекции,
        ///Заданный элемент хранит другой тип данных {2}.
        /// </summary>
        internal static string eArrayNotFoundOtherType {
            get {
                return ResourceManager.GetString("eArrayNotFoundOtherType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обязательный эелемент {0}, типа {1} не найден в колекции..
        /// </summary>
        internal static string eElementNotFound {
            get {
                return ResourceManager.GetString("eElementNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не выполнить преобразование типов в Stg коллекции.
        ///Запрашиваемый тип: {0}
        ///Найденный тип: {1}
        ///Имя: {2}.
        /// </summary>
        internal static string eInvalidTypeCast {
            get {
                return ResourceManager.GetString("eInvalidTypeCast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обязательный элемент {0}, типа StgElementNode имеет другой тип {1}.
        /// </summary>
        internal static string eNodeNotFound {
            get {
                return ResourceManager.GetString("eNodeNotFound", resourceCulture);
            }
        }
        
        internal static byte[] XmlCharType {
            get {
                object obj = ResourceManager.GetObject("XmlCharType", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}