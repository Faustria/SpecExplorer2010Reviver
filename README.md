# SpecExplorer2010Reviver
Use Spec Explorer 2010

PROLOG
======
Spec Explorer 2010 is a symbolic code exploration tool based on sXRT. It shows an unseen combination and number of very practical solutions and patterns to many urgent problems in software development going far beyond it's intended use case. In it's intended use as Model-Based Testing tool it is finally giving testing a solid foundation and a maximum of automation based e.g. on the definition of alternating simulation and on symbolic execution.

The last small update to Microsoft Spec Explorer seen was in 2013 and today it is way more than 5 years old. The then supported IDE: Microsoft Visual Studio Professional is not supported or even sold and downloadable any more: Only a small number of sub-releases were compatible with the plug-in due to its deep integration into the IDE. So today nearly anybody has access to a running Spec Explorer. We need to change this. We target at first students, teachers and experienced users.

We need all users to talk about Spec Explorer, write publications and use it in as many projects as possible.

See also: <br>
Spec Explorer Forum: https://social.msdn.microsoft.com/Forums/en-US/home?forum=specexplorer <br>
Spec Explorer Blog: https://blogs.msdn.microsoft.com/specexplorer/

![Screenshot1](Images/Viewer.jpg)

REQUIREMENTS
======
This tool here is the absolute bare minimal demo possible reviving the original Spec Explorer Plug-In binaries.
Some binaries used are open source on Github like Z3, MSAGL and SpecSharp some are under closed source license like sXRT.  This reviver of course is only needed if you can't use one of the original IDE's (or the minor sub-version predecessors): <br>
Microsoft Visual Studio Professional 2010 Version 10.0.30319.1 RTMREL<br>
Microsoft Visual Studio Professional 2012 Version 11.0.50727.1 RTMREL<br>

The reviver should compile and work in theory with any compiler targeting 
Microsoft .NET Framework Version 4.0 on x86 out of the box.

All binaries are used from Spec Explorer for Visual Studio 2010 (version 3.5.3146.0) - included here for convenience.
Please also download them yourself and unpack them from Microsoft MSI file: <br>
https://marketplace.visualstudio.com/items?itemName=SpecExplorerTeam.SpecExplorer2010VisualStudioPowerTool-5089

USAGE
======
Try to compile a SpecExplorer demo-model: See the StaticModel-folder. <br>
(Repair missing links to libraries using the binaries in the SpecExplorer2010Binaries-folder)

Try to compile the Reviver and explore the demo-model-binaries using Spec Explorer 2010: See the Reviver-folder. <br>
(Repair missing links to libraries using the binaries in the SpecExplorer2010Binaries-folder,
Adapt all the absolute paths in main!)


EPILOG
======
Other very closely related fields are:
Writing good requirements for software systems
Programming of dynamic (embedded) systems
Reverse engineering of (embedded) software
Simulating dynamic systems for system engineering, prediction
Quantitative model based safety analysis for (embedded) systems

These areas have in common that in industry in all of them some very tedious, painful steps are still done manually and it is very difficult to share or reuse work within a team and between them.
Handling these problems is a minefield. Spec Explorer shows practical solutions to all these fields.

Writing good requirements:
It is well known that a general way of defining requirements today is the syntax defined in the ISO 29148:2011. 
This syntax is very similar to the guarded state update rules of Spec Explorer.
It is shown how composition allows perfect team work using cord and components.

Programming of (embedded) dynamic systems:
Embedded systems urgently needed e.g. WCET (worst case execution time) estimation already during code implementation, we need to make quantitative analysis also for maximum allocation bringing automatic memory management to embedded software or safety and security to any software application. Symbolic exploration is a key enabler for this.

Reverse engineering:
The exploration and composition/slicing are a very interesting solution for reverse engineering of legacy code and to add additional documentation/information using slicing.

Simulating dynamic systems:
Spec Explorer can simulate DEV's (discrete event systems) and in comparison to Modelica uses a dynamic memory management etc.

Quantitative model based safety analysis:
Very interesting is the comparison with tools like Keymera.

Planned "improvements":
* Add again state/step viewer.
* Add again state comparator.
* Add again exploration manager.
* Add again as plug-in into Visual Studio.
* Add newer version of Z3, MSAGL and make use of new features not seen before (graph color, ...).
* etc.

Please help improve this tool and add your improvements at Github for reviving Spec Explorer gaining it's original strength. Don't hesitate to ask, change, add etc.

The hope that anytime soon any comparable product from Microsoft will appear again is slim to zero. It seems that in industry a general climax in many fields mentioned here was reached and we are on a descent right now again.

It would be nice not to get sued - everything here is licensed under MICROSOFT PRE-RELEASE SOFTWARE LICENSE TERMS. 
All the credits go to Microsoft, the original Microsoft Spec Explorer Team and all the other supporting Microsoft employees and users.
