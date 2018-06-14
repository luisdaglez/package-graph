# Package-Graph

This is a little tool I wrote to play around with .Net Core 2.0. It generates a dgml file that lays out your Visual Studio projects and dependencies in a directed graph. It takes a root path on your hard drive and scans all folders and subfolders for Visual Studio projects. All configuration options are in `Program.cs`.

## Requirements

You will need to install Visual Studio's DGML viewer (it can be found within the VS setup options) to be able to open the GUI for dgml files.
