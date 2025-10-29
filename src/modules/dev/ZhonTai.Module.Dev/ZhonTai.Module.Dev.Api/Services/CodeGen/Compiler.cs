using FreeSql;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;

namespace ZhonTai.Module.Dev;

internal class Compiler
{
    /// <summary>
    /// 内部反射静态类
    /// </summary>
    static class Reflect
    {
        /// <summary>
        /// 获取入口程序集
        /// </summary>
        /// <returns></returns>
        internal static Assembly GetEntryAssembly() => Assembly.GetEntryAssembly();

        /// <summary>
        /// 根据程序集名称获取运行时程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        internal static Assembly GetAssembly(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 根据路径加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Assembly LoadAssembly(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return null;
            }
            return Assembly.LoadFrom(path);
        }

        /// <summary>
        /// 通过流加载程序集
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static Assembly LoadAssembly(MemoryStream assembly)
        {
            return Assembly.Load(assembly.ToArray());
        }

        /// <summary>
        /// 根据程序集名称、类型完整限定名获取运行时类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type GetType(string assemblyName, string typeFullName)
        {
            return Reflect.GetAssembly(assemblyName).GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type GetType(Assembly assembly, string typeFullName)
        {
            return assembly.GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type GetType(MemoryStream assembly, string typeFullName)
        {
            return Reflect.LoadAssembly(assembly).GetType(typeFullName);
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static string GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetAssemblyName(Type type)
        {
            return Reflect.GetAssemblyName(type.GetTypeInfo());
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        internal static string GetAssemblyName(System.Reflection.TypeInfo typeInfo)
        {
            return Reflect.GetAssemblyName(typeInfo.Assembly);
        }

        /// <summary>
        /// 加载程序集类型，支持格式：程序集;网站类型命名空间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static Type GetStringType(string str)
        {
            string[] array = str.Split(";", StringSplitOptions.None);
            return Reflect.GetType(array[0], array[1]);
        }
    }

    IEnumerable<Assembly> _referencedAssemblies { get; set; }
    IEnumerable<string> _defaultUsings { get; set; } = new HashSet<string>
        {
            "System", "System.IO",
            "System.Linq", "System.Collections", "System.Collections.Generic",
            "System.Threading.Tasks"
        };

    public Compiler(IEnumerable<Assembly> referencedAssemblies, String[]? defaultUsings = null)
    {
        _referencedAssemblies = new HashSet<Assembly>
            {
                typeof(object).Assembly,
                typeof(CodeGenEntity).Assembly,
                typeof(IList).Assembly,
                typeof(IEnumerable<>).Assembly,
                Reflect.GetAssembly("Microsoft.CSharp"),
                Reflect.GetAssembly("System.Runtime"),
                Reflect.GetAssembly("System.Linq"),
                Reflect.GetAssembly("System.Linq.Expressions"),
                Reflect.GetAssembly("System.IO")
            };
        if (referencedAssemblies != null)
            _referencedAssemblies = _referencedAssemblies.Concat(referencedAssemblies);


        if (defaultUsings != null)
            _defaultUsings = _defaultUsings.Concat(defaultUsings).Distinct();
    }

    public Byte[] Compile(IEnumerable<String> files)
    {
        var references = _referencedAssemblies.Select(it => MetadataReference.CreateFromFile(it.Location));
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
            usings: _defaultUsings/*.Select(s => "using " + s + ";")*/);

        var outFileName = Path.GetRandomFileName();

        var syntaxTrees = files.Where(System.IO.File.Exists)
            .Select(file =>
            CSharpSyntaxTree.ParseText(
                string.Concat(string.Concat(_defaultUsings.Select(s => "using " + s + ";")), System.IO.File.ReadAllText(file, System.Text.Encoding.UTF8))
            ));

        var compilation = CSharpCompilation.Create(outFileName, syntaxTrees, references, options);

        using (var stream = new MemoryStream())
        {
            var compilationResult = compilation.Emit(stream);

            if (compilationResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
            throw new InvalidOperationException("Compilation error:" + Environment.NewLine + String.Join(Environment.NewLine, compilationResult.Diagnostics));
        }
    }
}

