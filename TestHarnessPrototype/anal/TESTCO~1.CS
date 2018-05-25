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
using TestHarness;

namespace Tests
{
  public class TestConfigure : Test
  {
    void CopyFiles(string src, string des)
    {
      string[] files = Directory.GetFiles(src,"*.*");
      foreach(string file in files)
      {
        string d = des + "/" + Path.GetFileName(file);
        FileAttributes attribs = File.GetAttributes(file);
        try
        {
          bool skip = false;
          if((attribs & FileAttributes.Hidden) == FileAttributes.Hidden)
            skip = true;
          if((attribs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            skip = true;
          if((attribs & FileAttributes.System) == FileAttributes.System)
            skip = true;
          if(!skip)
            File.Copy(file,d,true);
        }
        catch
        {
          Console.Write("\n  locked: {0}",file);
        }
      }
    }
//
    //----< construct configurer and start directories >-------

    public TestConfigure()
    {
// * - Manually create C:/test/empty with no files.
// * - Manually create C:/temp/test and add some files.
// * - Manually create C:/test/source with some files.
      Directory.CreateDirectory("C:/test/empty");
      Directory.CreateDirectory("C:/temp/test");
      CopyFiles("C:/","C:/temp/test");
      Directory.CreateDirectory("C:/test/source");
      CopyFiles("C:/","C:/test/source");
    }
    //----< define tests >-------------------------------------

    override public bool test()
    {
// * - Set Root to C:/temp/test.
      Configure config = new Configure();
// * - Attempt to configure on C:/temp/foobar should throw.
      config.SetRoot("c:/temp/test");
      try
      {
        config.Destination("c:/test/foobar");
        return false;
      }
      catch(Exception ex)
      {
        Console.Write("\n  {0}",ex.Message);
      }
// * - Attempt to configure C:/temp/test/configure from empty should
// *   succeed in that configure is created, but has no files.
      string dest = "c:/temp/test/configure";
      string src  = "c:/test/empty";
      config.Destination(dest);
      if(!Directory.Exists(dest))
        return false;
      config.Copy(src);
      if(Directory.GetFiles(dest).Length != 0)
        return false;
// * - Attempt to Remove should succeed in that configure is deleted.
      config.Remove();
      if(Directory.Exists(dest))
        return false;
// * - Attempt to configure C:/temp/test/configure, using files from 
// *   source should succeed, with the same files as in source.
      config.Destination(dest);
      src = "c:/test/source";
      config.Copy(src);
      if(config.NumberOfCopyableFiles(src) != Directory.GetFiles(dest).Length)
        return false;
// * - Repeat without calling Remove, so that configure has some files.
// *   Should succeed with same files as source.
      config.Destination(dest);
      config.Copy(src);
      if(config.NumberOfCopyableFiles(src) != Directory.GetFiles(dest).Length)
        return false;
// * - Remove should succeed in that configure and all its files are 
// *   deleted.
      config.Remove();
      if(Directory.Exists(dest))
        return false;
// * - Attempt to Remove again should succeed, e.g., nothing happens.
      config.Remove();
      if(Directory.Exists(dest))
        return false;
      return true;
    }
    //----< test stub >----------------------------------------

    static void Main(string[] args)
    {
      TestConfigure tc = new TestConfigure();
      if(tc.test())
        Console.Write("\n  configure tests passed");
      else
        Console.Write("\n  configure tests failed");
      Console.Write("\n\n");
    }
  }
}
