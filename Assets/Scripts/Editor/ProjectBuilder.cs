using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

//namespace Assets.Scripts.Editor
//{
class ProjectBuilder
{
	static string[] SCENES = FindEnabledEditorScenes();
	static string TARGET_DIR = "build";

	[MenuItem("Custom/CI/Build Android")]
	static void PerformAndroidBuild()
	{
		BuildOptions opt = BuildOptions.None;

		//char sep = Path.DirectorySeparatorChar;
		////string BUILD_TARGET_PATH = Path.GetFullPath(".") + sep + TARGET_DIR + string.Format("/AndroidBuild_{0}.apk", PlayerSettings.buildVersion);
		//string BUILD_TARGET_PATH = Path.GetFullPath(",") + sep + TARGET_DIR + "Poker.apk";

		char sep = Path.DirectorySeparatorChar;
		string buildDirectory = Path.GetFullPath(".") + sep + TARGET_DIR;
		Directory.CreateDirectory(buildDirectory);

		string BUILD_TARGET_PATH = buildDirectory + "/android";
		Directory.CreateDirectory(BUILD_TARGET_PATH);

		BUILD_TARGET_PATH += "/Poker.apk";

		GenericBuild(SCENES, BUILD_TARGET_PATH, BuildTarget.Android, opt);
	}

	[MenuItem("Custom/CI/Build iOS Debug")]
	static void PerformiOSDebugBuild()
	{
		BuildOptions opt = BuildOptions.SymlinkLibraries |
						   BuildOptions.Development |
						   BuildOptions.ConnectWithProfiler |
						   BuildOptions.AllowDebugging |
						   BuildOptions.Development |
						   BuildOptions.AcceptExternalModificationsToPlayer;

		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
		PlayerSettings.iOS.targetOSVersion = iOSTargetOSVersion.iOS_4_3;
		PlayerSettings.statusBarHidden = true;

		char sep = Path.DirectorySeparatorChar;
		string buildDirectory = Path.GetFullPath(".") + sep + TARGET_DIR;
		Directory.CreateDirectory(buildDirectory);

		string BUILD_TARGET_PATH = buildDirectory + "/ios";
		Directory.CreateDirectory(BUILD_TARGET_PATH);

		GenericBuild(SCENES, BUILD_TARGET_PATH, BuildTarget.iOS, opt);
	}

	private static string[] FindEnabledEditorScenes()
	{
		List<string> EditorScenes = new List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}

		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_path, BuildTarget build_target, BuildOptions build_options)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options);
		if (res.Length > 0)
		{
			throw new Exception("BuildPlayer failure: " + res);
		}
	}
}
//}
