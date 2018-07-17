// ******************************************************************************
// * Spec Explorer Reviver
// *
// * Description:  
// *    Used for this tool compiliation are:
// *    Microsoft.SpecExplorer.Core (Contains Explorartion Manager, Modeling Guidance, Viewers etc.)
// *    Microsoft.Msagl
// *    Microsoft.Msagl.Drawing
// *    Microsoft.Msagl.GraphViewerGDI
// *    
// *    Used for model compilation are:
// *    Microsoft.Xrt.Runtime
// *    Microsoft.SpecExplorer.Runtime
// *    Microsoft.SpecExplorer.DynamicTraversal
// *    Microsoft.SpecExplorer.ObjectModel
// *    
// *    Used for execution/exploration of models are:
// *    All Spec Explorer 2010 Plug-In binaries!
// *
// *    ATTENTION: Adapt all absolute paths in main-method.
// *
// ******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SpecExplorer.Viewer;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using System.Windows;
using System.Diagnostics;
using Microsoft.SpecExplorer.CommandLine;

namespace Reviver
{
	// Retrieving message-class for SpecExplorer application.
    class SpecExplorerHost : Microsoft.SpecExplorer.IHost
    {
        public Microsoft.SpecExplorer.MessageResult DecisionDialog(string title, string message, Microsoft.SpecExplorer.MessageButton messageButton)
        {
            throw new NotImplementedException();
        }

        public void DiagMessage(Microsoft.SpecExplorer.DiagnosisKind kind, string message, object location)
        {
            Debug.Write(message); ;
        }

        public IWin32Window DialogOwner
        {
            get { throw new NotImplementedException(); }
        }

        public Exception FatalError(string message, params Exception[] exceptions)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type type)
        {
            throw new NotImplementedException();
        }

        public void Log(string line)
        {
        	Debug.Write(line);
        }

        public bool Logging
        {
            get { throw new NotImplementedException(); }
        }

        public DialogResult ModalDialog(Form form)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string fileName, int line, int column)
        {
            throw new NotImplementedException();
        }

        public void NotificationDialog(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ProgressMessage(Microsoft.SpecExplorer.VerbosityLevel verbosity, string message)
        {
        	Debug.WriteLine(message);
        }

        public EventHandler Protect(EventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void RecoverFromFatalError(Exception exception)
        {
            // Even correct execution can end here.
            throw new NotImplementedException();
        }

        public void RunProtected(Microsoft.SpecExplorer.ProtectedAction action)
        {
            throw new NotImplementedException();
        }

        public bool TryFindLocation(Microsoft.SpecExplorer.MemberInfo member, out Microsoft.SpecExplorer.TextLocation location)
        {
            throw new NotImplementedException();
        }

        public bool TryGetExtensionData(string key, object inputValue, out object outputValue)
        {
            throw new NotImplementedException();
        }

        public Microsoft.SpecExplorer.VerbosityLevel Verbosity
        {
            get { throw new NotImplementedException(); }
        }
    }

    class Program
    {
    	// Spec Explorer binaries path.
    	public static string SpecExplorer = "";
    	
    	// Command list for Spec Explorer executable.
    	public enum Commands { persistexploration, generatetests};

  	
    	// Start Spec Explorer.
    	// Example:	Reviver (@"D:\SymbolicTest\bin\Debug", "SymbolicTest.dll,Service.dll", "SymbolicTest.cord,Standard.cord","SymbolicFunctionTest");
    	static void Reviver (string WorkingDirectory, string ModelLibs, string CordScripts, string Machine)
    	{
    		// Command line for shell-executable.
    		string Command =  
    			"/task:" + Enum.GetName(typeof (Commands), Commands.persistexploration) + " " + 
    			"/assemblies:" + ModelLibs + " "  +
    			"/scripts:" + CordScripts + " " +
    			"/machines:" + Machine;
            
    		// Command line for lib-interface.
    		string[] CommandArray = new []{
                "/task:" + Enum.GetName(typeof(Commands), Commands.persistexploration) + " " ,
                "/assemblies:" + ModelLibs + " " ,
                "/scripts:" + CordScripts + " " ,
                "/machines:" + Machine};

    	
    		// Start SpecExplorer using shell.
    		Process p = new Process();
			p.StartInfo.FileName = SpecExplorer ;
			p.StartInfo.Arguments = Command;
			p.StartInfo.WorkingDirectory = WorkingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
			p.Start();
 			string output = p.StandardOutput.ReadToEnd(); 			
 			Debug.Write(output);

			/* 			
    	    // Start SpecExplorer using lib-interface.
            // We have to add this to the *.exe.config 
            // <startup useLegacyV2RuntimeActivationPolicy="true">
            //       <supportedRuntime version="v4.0"/>
            // </startup> to run!
            // All binaries, model etc. must be in build directory!
    	    SpecExplorerHost Host = new SpecExplorerHost();
            CommandLineParser Parser = new CommandLineParser(Host);
            Parser.ParseCommand(CommandArray);
            ConsoleHostDriver Driver = new ConsoleHostDriver(Host,Parser);
            int i, j;
            Driver.Run(out i, out j);
            */

    	}
    	
    	
    	// Open window with exploration graph result.
    	static void Viewer (string FileName)
    	{
            // Create a Spec Explorer view definition.
            ViewDefinition ViewDef = new ViewDefinition();
            // Load the exploration result in seexpl-format.
            Graph ExplorationGraph = ViewDocumentControl.PersistenceFileToGraphForTest(FileName, ViewDef);
            // Create a form.
            Form form = new Form();
            // Create an MSAGL viewer object.
            GViewer viewer = new GViewer();
            // Bind the graph to the viewer.
            viewer.Graph = ExplorationGraph;
            // Associate the viewer with the form.
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //Show the form 
            form.ShowDialog();
    	}
    	
        static void Main(string[] args)
        {   
        	// Set path to Spec Explorer Plug-In binaries.
    		Program.SpecExplorer = @"D:\Projekte\Git_test\SpecExplorer2010Binaries\Specexplorer.exe";
			
    		// Run Spec Explorer on a demo model.
        	Reviver (@"D:\Projekte\Git_test\StaticModel\SpecExplorer1\bin\Debug",
        	         "SpecExplorer1.dll",
        	         "Config.cord",
        	         "DoubleAddScenario");
        	
    		// Open viewer on gnerated graph.
    		Viewer(@"D:\Projekte\Git_test\StaticModel\SpecExplorer1\bin\Debug\ExplorationResults\DoubleAddScenario.seexpl");
        }
    }
}
