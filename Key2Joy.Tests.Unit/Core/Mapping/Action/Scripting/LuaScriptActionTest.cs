﻿using Key2Joy.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Tests.Unit.Core.Mapping.Action.Scripting
{
    [TestClass]
    public class LuaScriptActionTest
    {
        [TestMethod]
        public void Script_ExecutesLuaFunction_ChecksResult()
        {
            var action = new LuaScriptAction(string.Empty, string.Empty);
            var lua = action.SetupEnvironment();

            action.Script = "function execute() return 1337 end\n" +
                "numberIsOne = execute()";
            action.Execute(null).Wait();

            var actual = lua["numberIsOne"];
            Assert.AreEqual((double)1337, actual);
        }
    }
}