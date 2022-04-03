using System;
using System.Reflection;

public class Util
{
    protected static Assembly AssemblyCSharp;
    public static Type GetType(string typeName)
    {
        if(AssemblyCSharp==null) 
            AssemblyCSharp=Assembly.Load("Assembly-CSharp");
        return AssemblyCSharp.GetType(typeName);
    }
}