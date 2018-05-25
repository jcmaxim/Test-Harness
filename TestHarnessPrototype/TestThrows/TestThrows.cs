/////////////////////////////////////////////////////////////////////
// TestThrows.cs - demonstration test that throws an Exception     //
//                                                                 //
// Platform:    Dell Dimension 8300, Windows XP Pro, SP 2.0        //
// Application: CSE784 - Software Studio, Final Project Prototype  //
// Author:      Jim Fawcett, Syracuse University, CST 2-187        //
//              jfawcett@twcny.rr.com, (315) 443-3948              //
/////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ==================
 * This module provides a demonstration test that throws an exception.
 * 
 * Public Interface:
 * =================
 * Test tst = new TestThrows();
 * bool ok = tst.test();
 */
/*
 * Build Process:
 * ==============
 * Files Required:
 *   Tester.cs, Loader.cs, ITest.cs, TestThrows.cs
 * Compiler Command:
 *   csc /t:Library Loader.cs
 *   csc /t:Library ITest.cs
 *   csc /t:Library TestThrows.cs
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
  public class TestThrows : Test
  {
    public TestThrows()
    {
      Console.Write("\n  Creating instance of TestThrows");
    }
    override public bool test()
    {
      throw new Exception("simulated exception");
      //return false;
    }
  }
}
