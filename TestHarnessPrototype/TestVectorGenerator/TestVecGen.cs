/////////////////////////////////////////////////////////////////////
// TestVecGen.cs - Test Vector Generator starter implementation    //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2010 //
/////////////////////////////////////////////////////////////////////
/*
 * This starter implementation of a Test Vector Generator (TVG)
 * provides a file generator.
 *
 * Often a TVG will need to provide both test inputs and predicted 
 * test outputs for comparison.
 *
 * Possibly useful TVGs:
 * - file generator: attaches to path and supplies files
 * - Line generator: attaches to file and supplies lines
 * - Message generator: supplies messages that are clones of 
 *     attached messages
 */
using System;
using System.Collections;
using System.IO;

namespace Tests
{
  public interface ITVG
  {
    // for now, this is just an inheritance hook
  }
  /////////////////////////////////////////////////////////////
  // TVG_files requires Navigate package

  public class TVG_files : ITVG, IEnumerator, IEnumerable
  {
    Navig.Navigate nav;
    ArrayList files;
    IEnumerator ie;

    public TVG_files(string path, string pattern, bool recurse)
    {
      files = new ArrayList();
      if(recurse)
      {
        nav = new Navig.Navigate();
        nav.newFile += new Navig.Navigate.newFileHandler(this.save);
        nav.go(path, pattern);
      }
      else
      {
        string[] tempFiles = Directory.GetFiles(path,pattern);
        foreach(string file in tempFiles)
          files.Add(file);
      }
      ie = files.GetEnumerator();
    }
     
    public void save(string file)
    {
      files.Add(file);
    }
    public bool MoveNext()
    {
      return ie.MoveNext();
    }
    public object current()
    {
      return ie.Current;
    }

    public object Current
    {
      get { return ie.Current; }
    }

    public void Reset()
    {
      ie.Reset();
    }

    public IEnumerator GetEnumerator()
    {
      return ie;
    }

    static void Main(string[] args)
    {
      Console.Write("\n  Demonstrating Starter Test Vector Generator");
      Console.Write("\n =============================================\n");

      Console.Write("\n  Generating files with foreach selector");
      Console.Write("\n ----------------------------------------");

      string path = "../..";
      string pattern = "*.*";
      TVG_files ftvg = new TVG_files(path,pattern,true);
      foreach(string file in ftvg)
        Console.Write("\n  {0}",Path.GetFileName(file));
      Console.Write("\n\n");

      Console.Write("\n  Generating files using Enumerator");
      Console.Write("\n -----------------------------------");

      ftvg.Reset();
      while(ftvg.MoveNext())
      {
        string file = ftvg.current() as string;
        Console.Write("\n  {0}",Path.GetFileName(file));
      }
      Console.Write("\n\n");
    }
  }
}
