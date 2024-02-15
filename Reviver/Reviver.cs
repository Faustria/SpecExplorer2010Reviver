// ******************************************************************************
// * Spec Explorer Reviver
// *
// * Description:  
// *    Used for this tool compiliation are:
// *    Microsoft.SpecExplorer.Core (explorartion manager, modeling guidance etc.)
// *    Microsoft.Msagl (transitien viewer)
// *    Microsoft.Msagl.Drawing (transitien viewer)
// *    Microsoft.Msagl.GraphViewerGDI (transitien viewer)
// *    Microsoft.Xrt (state viewer)
// *    Microsoft.SpecExplorer.ObjectModel (exploration results)
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
// *               Adapt also position of windows and sizes.
// *
// *    xrt.config: switches which can only be set in this config file using "D".
// *    Z3LogFilePath
// *    EnableDebugInfo
// *    PauseAtEnd
// *	SuspendsOnEachStep
// *	SuspendsOnEnterExit
// *	PlatformAssembliesLocation
// *	RegressionMode
// *	EntryPoint
// *	MachineName
// *	Script
// *	CheckOnly
// *	ExplorationMode
// *	DepthFirstSearch
// *	LogFile
// *	AppendLog
// *	IgnoreThreadIdOfRuleControlState
// *	AllowLoadingRuntimeAssemblies
// *	DoNotLockFiles
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
using Microsoft.SpecExplorer.ObjectModel;
using System.IO;

namespace Reviver
{
    class Program
    {
    	// Spec Explorer binaries path.
    	public const string SpecExplorer = @"D:\Projekte\SpecExplorerBinaries\Specexplorer.exe";
    	
    	// Spec Explorer exploration result path.
    	public const string ExplorationResults ="ExplorationResults";
    	
    	// Binaries sub folder.
    	public const string BinFolder = @"\bin\Debug";
    		
    	// Command list for Spec Explorer executable.
    	public enum Commands { persistexploration, generatetests};

    	// StateViewer Form.
    	private static Form frmStateViewer;
    	
    	// TransitionViewer Form.
    	private static Form frmTransitionViewer;

    	// LogViewer Form.
    	private static Form frmLogViewer;
    	
    	// Exploration state result control.
    	private static Microsoft.Xrt.UI.StateBrowserControl sbcStateControl;
    	
    	// Transition result control.
    	private static GViewer gvTransitionControl;
    	
    	// The full data of the result.
    	private static ExplorationResultLoader erlResult;
  	
    	// Start Spec Explorer.
    	// Example:	Reviver (@"D:\SymbolicTest\bin\Debug", "SymbolicTest.dll,Service.dll", "SymbolicTest.cord,Standard.cord","SymbolicFunctionTest");
    	static void Reviver (string WorkingDirectory, List<string>  ModelLibs, List<string> CordScripts, string Machine)
    	{
    		// Command line for shell-executable.
    		string Command =  
    			"/task:" + Enum.GetName(typeof (Commands), Commands.persistexploration) + " " + 
    			"/assemblies:" + String.Join(",", ModelLibs) + " "  +
    			"/scripts:" + WorkingDirectory + "\\" + CordScripts.Aggregate((ii, jj) => ii + "," + WorkingDirectory + "\\" + jj) + " " +
    			"/machines:" + Machine + " " +
    			"/ExplorationResultPath:" + WorkingDirectory + "\\" + ExplorationResults  + " " +
    			"/Reexplore" + " " + 
    			"/verbose";
            
    		// Command line for lib-interface.
    		string[] CommandArray = new []{
                "/task:" + Enum.GetName(typeof(Commands), Commands.persistexploration) + " " ,
                "/assemblies:" + WorkingDirectory + BinFolder + "\\" + ModelLibs.Aggregate((ii, jj) =>   ii + "," + WorkingDirectory + BinFolder + "\\"  + jj),
                "/scripts:" +  WorkingDirectory + "\\" + CordScripts.Aggregate((ii, jj) => ii + "," + WorkingDirectory + "\\" + jj),
                "/machines:" + Machine,
    		    "/ExplorationResultPath:" + WorkingDirectory + "\\" + ExplorationResults,
                //"/ReplayTestResultFile:" 
    		    "/Reexplore",
    		    "/verbose"};

    	
    		// Start SpecExplorer using shell.
    		Process p = new Process();
			p.StartInfo.FileName = SpecExplorer ;
			p.StartInfo.Arguments = Command;
			p.StartInfo.WorkingDirectory = WorkingDirectory + BinFolder;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
			p.Start();
 			string output = p.StandardOutput.ReadToEnd(); 			
 			Debug.Write(output);
			
			
    	    // Start SpecExplorer using lib-interface.
            // We have to add this to the *.exe.config 
            // <startup useLegacyV2RuntimeActivationPolicy="true">
            //       <supportedRuntime version="v4.0"/>
            // </startup> to run!
            // All binaries, model etc. must be in build directory of Reviver!
            // or (as we do here, give full paths.
            /*
    	    SpecExplorerHost Host = new SpecExplorerHost();
            CommandLineParser Parser = new CommandLineParser(Host);
            
            Parser.ParseCommand(CommandArray);
            ConsoleHostDriver Driver = new ConsoleHostDriver(Host,Parser);
            int i, j;
            Driver.Run(out i, out j);
            */
    	}   
    	
    	// Update state data in state view.
    	static void UpdateStateView (string StateName)
    	{
            var erResult = erlResult.LoadExplorationResult();
            // State data - in a tree node structure.
            // Shared values is the content.
            var seData = erResult.SharedEntities;
            // Just a descriptor.
            Microsoft.Xrt.UI.StateNodeInfomation sniNode = LoadStateInfo(StateName, erlResult);
            // Tree data content is assembled from tree structure in the grid view.
            sbcStateControl.LoadStates(seData,new Microsoft.Xrt.UI.StateNodeInfomation[1]{sniNode});
    		frmStateViewer.Refresh();
    	}
    	
    	// Prepare a description for the state information consumed by state viewer.
    	static Microsoft.Xrt.UI.StateNodeInfomation LoadStateInfo(string state, ExplorationResultLoader loader)
		{
    		Microsoft.SpecExplorer.ObjectModel.Node state2 = null;
  			StateEntity stateEntity = loader.LoadState(state);
  			state2 = stateEntity.Content;
    		Dictionary<string, object> dictionary = new Dictionary<string, object>();
    		dictionary.Add("Description", state);
    		return new Microsoft.Xrt.UI.StateNodeInfomation(state2, state, dictionary);
    	}
    	
    	// Open the state view.
    	static void StateView ()
    	{
            // Create a form.
            frmStateViewer = new Form();
            frmStateViewer.SuspendLayout();
           
            sbcStateControl = new Microsoft.Xrt.UI.StateBrowserControl();
			// Set the column size of first column.
            //sbcStateControl.Columns[0].Width = 200;
            UpdateStateView("S0");
           
            frmStateViewer.Controls.Add(sbcStateControl);
            
            // Show the form. 
            // Here you can adjust the window position.
            var Location = new System.Drawing.Point(600,0);
            frmStateViewer.Location = Location;
            frmStateViewer.StartPosition = FormStartPosition.Manual;
            // Here you can adjust the window size.            
            frmStateViewer.Size = new System.Drawing.Size(424,300);
            frmStateViewer.ResumeLayout();
            frmStateViewer.Show();    		
    		frmStateViewer.Refresh();    	
    	}
    	
		// Open the transition window.    	
    	static void TransitionView (string Directory, string Machine)
    	{
			// Create Filename.
    		string FileName = Directory + @"\" + ExplorationResults + @"\" + Machine + ".seexpl";    		
            // Create a Spec Explorer view definition.
            ViewDefinition ViewDef = new ViewDefinition();
            // Load the exploration result in seexpl-format.
            Graph ExplorationGraph = ViewDocumentControl.PersistenceFileToGraphForTest(FileName, ViewDef);
            // Create a form.
            frmTransitionViewer = new Form();
            // Create an MSAGL viewer object.
            gvTransitionControl = new GViewer();
            // Bind the graph to the viewer.
            gvTransitionControl.Graph = ExplorationGraph;
            // Associate the viewer with the form.
            frmTransitionViewer.SuspendLayout();
            gvTransitionControl.Dock = System.Windows.Forms.DockStyle.Fill;
 
            frmTransitionViewer.Controls.Add(gvTransitionControl);
            
            // Show the form. 
            // Here you can adjust the window position.
            var Location = new System.Drawing.Point(0,0);
            frmTransitionViewer.Location = Location;
            frmTransitionViewer.StartPosition = FormStartPosition.Manual; 
            // Here you can adjust the window size.
            frmTransitionViewer.Size = new System.Drawing.Size(600,600);
            frmTransitionViewer.ResumeLayout();
            frmTransitionViewer.Show();  
            frmTransitionViewer.Refresh();            
    	}
    	
    	// Open the exploration result data.
    	static void LoadResult (string Directory, string Machine)
    	{
     		// Create Filename.
    		string FileName = Directory + @"\" + ExplorationResults + @"\" + Machine + ".seexpl";    		

    		erlResult = new ExplorationResultLoader(FileName);
    	}
    	
    	// Show log file results.
    	static void ShowLogView (string Directory)
    	{
    		// The XRT log file for the z3 commands can be enabeld via xrt.config switch.
			// <D>
            //    "Z3LogFilePath=z3.log"
            // </D>
    		string sZ3LogFile = Directory + BinFolder + "\\z3.log";
    		string sContents;
    		
    		// Try to load the log file.
    		try
    		{
    			sContents = File.ReadAllText(sZ3LogFile);
    		}
    		catch
    		{
    			sContents = "";
    		}
    		
    		// Create a form.
            frmLogViewer = new Form();
            frmLogViewer.SuspendLayout();
           
            TextBox tbLogText = new TextBox();
            
            
            tbLogText.ScrollBars = ScrollBars.Both;
			tbLogText.WordWrap = false;
			tbLogText.Size = new System.Drawing.Size(400, 50);
            //tbLogText.Text = sContents;
            tbLogText.Multiline = true;
            tbLogText.AppendText (sContents);
			frmLogViewer.Controls.Add(tbLogText);
            
            
            // Show the form. 
            // Here you can adjust the window position.
            var Location = new System.Drawing.Point(600,300);
            frmLogViewer.Location = Location;
            frmLogViewer.StartPosition = FormStartPosition.Manual;
            // Here you can adjust the window size.            
            frmLogViewer.Size = new System.Drawing.Size(424,100);
            frmLogViewer.ResumeLayout();
            frmLogViewer.Show();    		
    		frmLogViewer.Refresh();    	
    	}

    	
    	// Open all windows with exploration graph, state data, log results.
    	static void Viewer (string Directory, string Machine)
    	{    		
    		ShowLogView(Directory);
    		LoadResult(Directory, Machine);
    		StateView();
    		TransitionView(Directory, Machine);
    		//var s = frmTransitionViewer.ActiveControl;
    		var o = gvTransitionControl.SelectedObject;
    		//gvTransitionControl.DrawingLayoutEditor.SelectedEdge
    		string Name;
    		while (true)
    		{
    			// Process all user events of the just opened forms.    			
    			Application.DoEvents();
    			// Get object in transitient view mouse hovered over. 
    		    o = gvTransitionControl.SelectedObject;
    		    try
    		    {
    		        Name = o.ToString();
    		    }
    		    catch
    		    {
    		    	Name ="";
    		    }
    		    if (o!= null )
    		    {
    		    	if (Name.Length >=  2 & 
						Name.Substring(0,2) == "\"S")
    		    	{
    		    		Name = Name.Split('"')[1];
    		    		// Try to update according to object under mouse.
    		    		UpdateStateView(Name);
    		    	}
    		    }
    		}
    	}    	
		
    	// Explore and open window with result.
		static void Explore (string WorkingDirectory, List<string> ModelLibs, List<string> CordScripts, string Machine)
    	{
    		PositionConsoleWindow.PlaceConsoleView();
    		Reviver (WorkingDirectory, ModelLibs, CordScripts, Machine);
    		Viewer (WorkingDirectory, Machine);    	
    	}    	
    	
    	// Open window with result.
    	static void View (string WorkingDirectory, string Machine)
    	{
    		PositionConsoleWindow.PlaceConsoleView();
    		Viewer (WorkingDirectory, Machine);    	
    	}    	
    	
        static void Main(string[] args)
        {   


        	// Run Spec Explorer on a demo model.
        	Explore (@"D:\Projekte\Git_test\StaticModel\SpecExplorer1",
        	         new List<string> {"SpecExplorer1.dll"},
        	         new List<string> {"Config.cord"},
        	         "SlicedAccumulatorModelProgram");

        	
        	
           // View(@"D:\Projekte\Git_test\StaticModel\SpecExplorer1","SlicedAccumulatorModelProgram");
            
            
        }
    }

}
