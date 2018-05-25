Test Automation Prototype
Version 2.0
Credit to Jim Fawcett

Purpose:
Illustrate how one might build a test harness that does not have to be rebuilt every time you add some new code to test.  It accomplishes this by:

1.	Creating a child Application Domain in which to run tests, running on its own thread
2.	Loading assemblies to test into the child domain
3.	Creating an instance of any class that implements the ITest interface, e.g., a test driver
4.	Running the test by calling ITest.test()
5.	Reporting success or failure
6.	Repeating for all test drivers in all the loaded assemblies
7.	Unloading the child Application Domain, which unloads all the libraries
8.	Repeating until there are no more assemblies to process.

