// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace OLEDB.Test.ModuleCore
{
    public class MyDict<Type1, Type2> : Dictionary<Type1, Type2>
    {
        public new Type2 this[Type1 key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return base[key];
                }
                return default(Type2);
            }
            set
            {
                base[key] = value;
            }
        }
    }
    ////////////////////////////////////////////////////////////////
    // CModInfo
    //
    ////////////////////////////////////////////////////////////////
    public class CModInfo
    {
        //Data
        static private String s_strCommandLine;
        static private MyDict<string, string> s_hashOptions;
        static private object _includenotimplemented;

        //Constructor
        public CModInfo()
        {
        }

        //Helpers
        static internal void Dispose()
        {
            //Reset the info.  
            //Since this is a static class, (to make it simplier to access from anywhere in your code)
            //we need to reset this info everytime a test is run - so if you don't select an alias
            //the next time it doesn't use the previous alias setting - ie: ProviderInfo doesn't 
            //get called when no alias is selected...
            s_strCommandLine = null;
            s_hashOptions = null;
        }
        static public string CommandLine
        {
            // This Assert allows callers without the EnvironementPermission to use this property
            get
            {
                if (s_strCommandLine == null)
                    s_strCommandLine = "";
                return s_strCommandLine;
            }
            set
            {
                s_strCommandLine = value;
            }
        }
        static public MyDict<string, string> Options
        {
            get
            {
                //Deffered Parsing
                if (s_hashOptions == null)
                {
                    CKeywordParser.Tokens tokens = new CKeywordParser.Tokens();
                    tokens.Equal = " ";
                    tokens.Seperator = "/";
                    s_hashOptions = CKeywordParser.ParseKeywords(CommandLine, tokens);
                }
                return s_hashOptions;
            }
        }

        static public string Filter
        {
            //Typed options
            get { return (string)CModInfo.Options["Filter"]; }
        }

        static public string MaxPriority
        {
            //Typed options
            get { return (string)CModInfo.Options["MaxPriority"]; }
        }

        static public bool IncludeNotImplemented
        {
            //Typed options
            get
            {
                if (_includenotimplemented == null)
                {
                    _includenotimplemented = false;
                    if (CModInfo.Options["IncludeNotImplemented"] != null)
                        _includenotimplemented = true;
                }

                return (bool)_includenotimplemented;
            }
            set { _includenotimplemented = value; }
        }

        public static bool IsTestCaseSelected(string testcasename)
        {
            bool ret = true;
            string testcasefilter = CModInfo.Options["testcase"];
            if (testcasefilter != null
                && testcasefilter != "*"
                && testcasefilter != testcasename)
            {
                ret = false;
            }

            return ret;
        }

        public static bool IsVariationSelected(string variationname)
        {
            bool ret = true;
            string variationfilter = CModInfo.Options["variation"];
            if (variationfilter != null
                && variationfilter != "*"
                && variationfilter != variationname)
            {
                ret = false;
            }

            return ret;
        }
    }
}


