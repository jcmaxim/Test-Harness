/////////////////////////////////////////////////////////////////////
// TestConfigure.cs - Tests Configure.cs                           //
//                                                                 //
// Platform:    Dell Dimension 8300, Windows XP Pro, SP 2.0        //
// Application: CSE784 - Software Studio, Final Project Prototype  //
// Author:      Jim Fawcett, Syracuse University, CST 2-187        //
//              jfawcett@twcny.rr.com, (315) 443-3948              //
/////////////////////////////////////////////////////////////////////
/*
 * Configure Requirements:
 * =======================
 * - Define a Root Directory on which all configured directories must
 *   reside
 * - Create Test Directories ONLY on a subtree rooted at the Root
 * - Copy files from a source directory to the configured directory
 * - Delete all files in configured directory and delete directory
 * 
 * Test Procedure:
 * ===============
 * - Manually create C:/test/empty with no files.
 * - Manually create C:/temp/test and add some files.
 * - Manually create C:/test/source with some files.
 * - Set Root to C:/temp/test.
 * - Attempt to configure on C:/temp/foobar should throw.
 * - Attempt to configure C:/temp/test/configure from empty should
 *   succeed in that configure is created, but has no files.
 * - Attempt to Remove should succeed in that configure is deleted.
 * - Attempt to configure C:/temp/test/configure, using files from 
 *   source should succeed, with the same files as in source.
 * - Repeat without calling Remove, so that configure has some files.
 *   Should succeed with same files as source.
 * - Remove should succeed in that configure and all its files are 
 *   deleted.
 * - Attempt to Remove again should succeed, e.g., nothing happens.
 */

//
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using TestHarness;

namespace Tests
{
  /////////////////////////////////////////////////////////////
  // class ITVGExtensions implements an extension method
  // for copying files.  Extension methods allow us to
  // extend an existing interface without a lot of casting

  static class ITVGExtensions
  {
    public static void CopyFiles(this ITVG tvg, string src, string des, ILogger Logger)
    {
      string[] files = Directory.GetFiles(src, "*.*");
      foreach (string file in files)
      {
        string d = des + "/" + Path.GetFileName(file);
        FileAttributes attribs = File.GetAttributes(file);
        try
        {
          bool skip = false;
          if ((attribs & FileAttributes.Hidden) == FileAttributes.Hidden)
            skip = true;
          if ((attribs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            skip = true;
          if ((attribs & FileAttributes.System) == FileAttributes.System)
            skip = true;
          if (!skip)
            File.Copy(file, d, true);
        }
        catch
        {
          Logger.add("\n  locked: " + file + "\n");
        }
      }
    }
  }
  /////////////////////////////////////////////////////////////
  // class TestConfigure is a test driver for the Configure
  // facility.  Note how its test procedure comments are
  // woven into the code so it's easy to see if it implements
  // the specified tests.

  public class TestConfigure : Test
  {
    string src;
    string dest;

    //----< construct configurer and start directories >-------

    public TestConfigure()
    {
      TestVectorGenerator = new TestTVG();
      Logger = new ConsoleLogger();
      Logger.add("  Creating instance of TestConfigure").showCurrentItem();

// * - Manually create C:/test/empty with no files.
// * - Manually create C:/temp/test and add some files.
// * - Manually create C:/test/source with some files.

      Directory.CreateDirectory("C:/test/empty");
      Directory.CreateDirectory("C:/temp/test");
      TestVectorGenerator.CopyFiles("C:/","C:/temp/test", Logger);
      Directory.CreateDirectory("C:/test/source");
      TestVectorGenerator.CopyFiles("C:/","C:/test/source", Logger);
    }
    //----< define tests >-------------------------------------

    // * - Set Root to C:/temp/test.
    // * - Attempt to configure on C:/temp/foobar should throw.

    bool test1(Configure config)
    {
      Logger.add("  test1: attempt to configure on illegal path").showCurrentItem();
      config.SetRoot("c:/temp/test");
      try
      {
        config.Destination("c:/temp/foobar");
        return false;
      }
      catch (Exception ex)
      {
        Logger.add("  " + ex.Message.Trim()).showCurrentItem();
      }
      return true;
    }

    // * - Attempt to configure C:/temp/test/configure from empty should
    // *   succeed in that configure is created, but has no files.

    bool test2(Configure config)
    {
      Logger.add("  test2: attempt to configure and copy path with no files").showCurrentItem();
      dest = "c:/temp/test/configure";
      src = "c:/test/empty";
      config.Destination(dest);
      if (!Directory.Exists(dest))
        return false;
      config.Copy(src);
      if (Directory.GetFiles(dest).Length != 0)
        return false;
      return true;
    }

    // * - Attempt to Remove should succeed in that configure is deleted.

    bool test3(Configure config)
    {
      Logger.add("  test3: attempt to remove configured directory").showCurrentItem();
      config.Remove();
      Assert(dest == "c:/temp/test/configure");
      if (Directory.Exists(dest))
        return false;
      return true;
    }

    // * - Attempt to configure C:/temp/test/configure, using files from 
    // *   source should succeed, with the same files as in source.

    bool test4(Configure config)
    {
      Logger.add("  test4: attempt to configure with non-empty directory").showCurrentItem();
      Assert(dest == "c:/temp/test/configure");
      config.Destination(dest);
      src = "c:/test/source";
      config.Copy(src);
      if (config.NumberOfCopyableFiles(src) != Directory.GetFiles(dest).Length)
        return false;
      return true;
    }

    // * - Repeat without calling Remove, so that configure has some files.
    // *   Should succeed with same files as source.

    bool test5(Configure config)
    {
      Logger.add("  test5: Repeat test4 without Removing configured directory").showCurrentItem();
      Assert(dest == "c:/temp/test/configure");
      Assert(src == "c:/test/source");
      config.Destination(dest);
      config.Copy(src);
      if (config.NumberOfCopyableFiles(src) != Directory.GetFiles(dest).Length)
        return false;
      return true;
    }

    // * - Remove should succeed in that configure and all its files are 
    // *   deleted.

    bool test6(Configure config)
    {
      Logger.add("  test6: Remove configured directory").showCurrentItem();
      Assert(dest == "c:/temp/test/configure");
      config.Remove();
      if (Directory.Exists(dest))
        return false;
      return true;
    }

    // * - Attempt to Remove again should succeed, e.g., nothing happens.

    bool test7(Configure config)
    {
      Logger.add("  test7: attempt to remove already removed directory").showCurrentItem();
      Assert(dest == "c:/temp/test/configure");
      config.Remove();
      if (Directory.Exists(dest))
        return false;
      return true;
    }
    override public bool test()
    {
      try
      {
        Configure config = new Configure();
        if (!test1(config)) return false;
        if (!test2(config)) return false;
        if (!test3(config)) return false;
        if (!test4(config)) return false;
        if (!test5(config)) return false;
        if (!test6(config)) return false;
        if (!test7(config)) return false;
      }
      catch(Exception ex)
      {
        Console.Write("\n  {0}", ex.Message);
        return false;
      }
      Logger.showAll();
      return true;
    }
    //----< test stub >----------------------------------------

    static void Main(string[] args)
    {
      Console.Write("\n  Demonstrate TestConfigure - test example");
      Console.Write("\n ==========================================\n");

      TestConfigure tc = new TestConfigure();
      if(tc.test())
        Console.Write("\n  configure tests passed");
      else
        Console.Write("\n  configure tests failed");
      Console.Write("\n\n");
    }
  }
}
