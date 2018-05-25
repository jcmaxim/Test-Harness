/////////////////////////////////////////////////////////////////////
// Configure.cs - Copies and Deletes Directories with verification //
//                                                                 //
// Platform:    Dell Dimension 8300, Windows XP Pro, SP 2.0        //
// Application: CSE784 - Software Studio, Final Project Prototype  //
// Author:      Jim Fawcett, Syracuse University, CST 2-187        //
//              jfawcett@twcny.rr.com, (315) 443-3948              //
/////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ==================
 * This module provides operations to create a configure directory
 * on some path rooted at a required path, copy files into it from
 * a specified source, and delete it.  
 * 
 * WARNING!  These operations succeed even if a directory exists on 
 * the specified configure path. 
 * 
 * Public Interface:
 * =================
 * Configure config = new Configure("C:/temp/foobar");
 * config.Copy("C:/temp");
 * config.Remove()
 */
/*
 * Build Process:
 * ==============
 * Files Required:
 *   Configure.cs
 * Compiler Command:
 *   csc /t:exe Configure.cs
 *  
 * Maintence History:
 * ==================
 * ver 1.0 : 10 Oct 05
 *   - first release
 * 
 */
//
using System;
using System.IO;

namespace TestHarness
{
  public class Configure
  {
    string RequiredRoot = "c:/temp";
    DirectoryInfo di = null;
    string path_ = "c:/temp";

    //----< verifies path has Required Root >------------------

    void PathVerification()
    {
      if(path_ == null)
      {
        throw new Exception(string.Format(
          "\n  destination path not set.  Please call Destination(path)"
        ));
      }
      RequiredRoot = RequiredRoot.ToLower();
      if(path_.IndexOf(RequiredRoot,0,RequiredRoot.Length) != 0)
      {
        throw new Exception(string.Format(
          "\n  path does not start with Required Root {0}",
          RequiredRoot
        ));
      }
    }
    //----< constructor uses default root and dest path >------

    public Configure()
    {
    }
    //----< constructor sets destination path >----------------

    public Configure(string path)
    {
      path_ = path.ToLower();
      PathVerification();
      if(Directory.Exists(path_))
      {
        di = new DirectoryInfo(path_);
      }
      else
        di = Directory.CreateDirectory(path_);
    }
    //
    //----< resets root directory >----------------------------

    public void SetRoot(string path)
    {
      path = path.ToLower();
      if(path.IndexOf("c:/temp") != 0)
      {
        Console.Write("\n  Caution: Configure can delete directories!");
        Console.Write("\n  Are you sure you want to use {0}?",path);
      }
      RequiredRoot = path.ToLower();
      if(Directory.Exists(RequiredRoot))
      {
        di = new DirectoryInfo(RequiredRoot);
      }
      else
        di = Directory.CreateDirectory(RequiredRoot);
    }
    //----< set configuration path >---------------------------

    public void Destination(string path)
    {
      path_ = path.ToLower();
      PathVerification();
      if(Directory.Exists(path_))
      {
        di = new DirectoryInfo(path_);
        RemoveFiles();
      }
      else
        di = Directory.CreateDirectory(path_);
    }
    //----< don't copy hidden, system, or readonly files >-----

    public bool OKtoCopy(string file)
    {
      FileAttributes attribs = File.GetAttributes(file);
      bool skip = false;
      if((attribs & FileAttributes.Hidden) == FileAttributes.Hidden)
        skip = true;
      if((attribs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        skip = true;
      if((attribs & FileAttributes.System) == FileAttributes.System)
        skip = true;
      return !skip;
    }
    //
    //----< count of copy-able files >-------------------------

    public int NumberOfCopyableFiles(string path)
    {
      string[] files = Directory.GetFiles(path);
      int count = 0;
      foreach(string file in files)
      {
        if(OKtoCopy(file))
          ++count;
      }
      return count;
    }
    //----< copy files from sourcePath to configure path >-----

    public bool Copy(string sourcePath)
    {
      PathVerification();
      string[] files = Directory.GetFiles(sourcePath,"*.*");
      foreach(string source in files)
      {
        try
        {
          string destination = path_ + "/" + Path.GetFileName(source);
          if(OKtoCopy(source))
            File.Copy(source,destination,true);
        }
        catch
        {
          Console.Write("\n  locked: {0}",source);
        }
        Console.Write("\n  copied file: {0}", source);
      }
      return true;
    }
    //----< remove all files from configure path >-------------

    void RemoveFiles()
    {
      PathVerification();
      string[] files = Directory.GetFiles(di.FullName,"*.*");
      foreach(string file in files)
      {
        File.Delete(file);
      }
    }
    //
    //----< deletes configure path >---------------------------

    public void Remove()
    {
      if(Directory.Exists(di.FullName))
      {
        RemoveFiles();
        di.Delete(false);
      }
    }
    //----< show path and file count >-------------------------

    void ShowDirectoryState(string path)
    {
      if(Directory.Exists(path))
      {
        Console.Write(
          "\n  {0} exists and contains {1} files",
          path,
          Directory.GetFiles(path,"*.*").Length
          );
      }
      else
      {
        Console.Write("\n  {0} does not exist",path);
      }
    }
    //----< test configure class >-----------------------------

    static void Main(string[] args)
    {
      string destination = "c:/temp/foobar";
      string source = "c:/temp";

      Console.Write(
        "\n  Testing Configure on destination {0}\n",
        destination
      );
      Console.Write(new string('=',35+destination.Length) + '\n');
      
      Configure config = new Configure(destination);
      config.ShowDirectoryState(destination);
      config.Copy(source);
      config.ShowDirectoryState(destination);
      config.Remove();
      config.ShowDirectoryState(destination);
      Console.Write("\n\n");
    }
  }
}
