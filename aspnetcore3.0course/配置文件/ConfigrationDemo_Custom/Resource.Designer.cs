﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConfigrationDemo_Custom {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConfigrationDemo_Custom.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 File path must be a non-empty string. 的本地化字符串。
        /// </summary>
        public static string Error_InvalidFilePath {
            get {
                return ResourceManager.GetString("Error_InvalidFilePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not parse the JSON file. Error on line number &apos;{0}&apos;: &apos;{1}&apos;. 的本地化字符串。
        /// </summary>
        public static string Error_JSONParseError {
            get {
                return ResourceManager.GetString("Error_JSONParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 A duplicate key &apos;{0}&apos; was found. 的本地化字符串。
        /// </summary>
        public static string Error_KeyIsDuplicated {
            get {
                return ResourceManager.GetString("Error_KeyIsDuplicated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unsupported JSON token &apos;{0}&apos; was found. Path &apos;{1}&apos;, line {2} position {3}. 的本地化字符串。
        /// </summary>
        public static string Error_UnsupportedJSONToken {
            get {
                return ResourceManager.GetString("Error_UnsupportedJSONToken", resourceCulture);
            }
        }
    }
}
