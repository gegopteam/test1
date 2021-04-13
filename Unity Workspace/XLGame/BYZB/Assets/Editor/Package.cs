using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#if UNITY_EDITOR_OSX

using UnityEditor.iOS.Xcode;
using UnityEditor.XcodeEditor;

#endif

public class Package
{
	#if UNITY_EDITOR_OSX
	[PostProcessBuildAttribute (100)]
	public static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
	{
		if (target != BuildTarget.iOS) {
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}
		// Create a new project object from build target
		PBXProject project = new PBXProject ();
		string configFilePath = PBXProject.GetPBXProjectPath (pathToBuiltProject);
		project.ReadFromFile (configFilePath);
		string targetGuid = project.TargetGuidByName ("Unity-iPhone");
		string debug = project.BuildConfigByName (targetGuid, "Debug");
		string release = project.BuildConfigByName (targetGuid, "Release");
		project.AddBuildPropertyForConfig (debug, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");
		project.AddBuildPropertyForConfig (release, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");

		project.AddFrameworkToProject (targetGuid, "SystemConfiguration.framework", true);
		project.AddFrameworkToProject (targetGuid, "Security.framework", true);
		project.AddFrameworkToProject (targetGuid, "libz.tbd", true);
		project.AddFrameworkToProject (targetGuid, "libc++.tbd", true);

		project.SetBuildProperty (targetGuid, "ENABLE_BITCODE", "NO");

		project.WriteToFile (configFilePath);

		EditSuitIpXCode (pathToBuiltProject);
	}

	public static void EditSuitIpXCode (string path)
	{
		//插入代码
		//读取UnityAppController.mm文件
		string src = @"_window         = [[UIWindow alloc] initWithFrame: [UIScreen mainScreen].bounds];";
		string dst = @"//    _window         = [[UIWindow alloc] initWithFrame: [UIScreen mainScreen].bounds];

   CGRect winSize = [UIScreen mainScreen].bounds;
   if (winSize.size.width / winSize.size.height> 2) {
       winSize.size.width -= 64;
       winSize.origin.x = 32;
       ::printf(""-> is iphonex "");
   } else {
       ::printf(""-> is not iphonex"");
   }
   _window = [[UIWindow alloc] initWithFrame: winSize];

   ";

		string unityAppControllerPath = path + "/Classes/UnityAppController.mm";
		XClassExt UnityAppController = new XClassExt (unityAppControllerPath);
		UnityAppController.Replace (src, dst);
	}
	#endif

}
