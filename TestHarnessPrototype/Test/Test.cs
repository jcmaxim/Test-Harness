/////////////////////////////////////////////////////////////////////
// Test.cs - plain vanilla demonstration test that passes          //
//                                                                 //
// Platform:    Dell Dimension 8300, Windows XP Pro, SP 2.0        //
// Application: CSE784 - Software Studio, Final Project Prototype  //
// Author:      Jim Fawcett, Syracuse University, CST 2-187        //
//              jfawcett@twcny.rr.com, (315) 443-3948              //
/////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ==================
 * This module provides a simple test demonstration that passes.
 * 
 * Public Interface:
 * =================
 * Test tst = new Test();
 * bool ok = tst.test();
 */
/*
 * Build Process:
 * ==============
 * Files Required:
 *   Tester.cs, Loader.cs, ITest.cs, Test.cs
 * Compiler Command:
 *   csc /t:Library Loader.cs
 *   csc /t:Library ITest.cs
 *   csc /t:Library Test.cs
 *   csc /t:exe     Tester.cs
 * Deployment:
 *   Loader.dll --> TesterDir/Loader
 *   ITest.dll  --> TesterDir/TestLibraries
 *   Test dlls  --> TesterDir/TestLibraries
 *   where TesterDir is the folder containing Tester.exe
 *  
 * Maintence History:
 * ==================
 * ver 1.0 : 09 Oct 05
 *   - first release
 * 
 */
//
using System;

namespace Tests
{
  public class TestTVG : ITVG
  {
    // This is a placeholder class that will
    // be decorated with extension methods in
    // TestDrivers.
  }

  public class Test : ITest
  {
    protected ITVG TestVectorGenerator = null;
    public ILogger Logger = null;

    public Test()
    {
      Console.Write("\n  Creating instance of Test");
    }
    virtual public bool test()
    {
      Console.Write("\n  testing now");
      return true;
    }
    public string id()
    {
      Type t = this.GetType();
      return t.FullName;
    }
    public void Assert(bool predicate)
    {
      if (!predicate)
      {
        if (Logger != null)
          Logger.add("assertion failure");
        throw (new Exception("assertion failure"));
      }
    }
  }
}
