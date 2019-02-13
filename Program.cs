using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
            Evidence adevidence = AppDomain.CurrentDomain.Evidence;
            AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);
            Type type = typeof(Proxy);
            var value = (Proxy)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName);
            //String myDir = "C:\\Windows\\Microsoft.NET\\assembly\\GAC_64\\";
            String myDir = "C:\\Windows\\Microsoft.NET\\assembly\\";
            List<Assembly> myAssemblyList = new List<Assembly>();
            foreach (String f in Directory.GetFiles(myDir, "*.dll", SearchOption.AllDirectories))
            {
                //Console.WriteLine($"Here is f: {f}");
                Assembly currentAssembly;
                try
                {
                    currentAssembly = value.GetAssembly(f);
                    if (currentAssembly != null)
                    {
                        myAssemblyList.Add(currentAssembly);
                        Console.WriteLine(currentAssembly.FullName);
                        //Console.ReadLine();
                    }
                }
                catch (FileNotFoundException e)
                {
                    
                    Console.WriteLine($"{e.Data} is not an assembly");
                }
                    

                //Console.WriteLine($"Total Assemblies found: {myAssemblyList.Count}");
            }
            Console.WriteLine($"Total Assemblies found: {myAssemblyList.Count}");
            Console.ReadLine();
        }
    }
    public class Proxy : MarshalByRefObject
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFile(assemblyPath);
            }
            catch (Exception)
            {
                return null;
                // throw new InvalidOperationException(ex);
            }
        }
    }
}
