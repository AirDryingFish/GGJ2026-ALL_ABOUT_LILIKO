using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class VideoCompatibleOnBuilding : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    static readonly string TEMP_ROOT = "Temp/WebGLCompatibleTEMP";


    public void OnPreprocessBuild(BuildReport report)
    {
        bool isWebGL = report.summary.platform == BuildTarget.WebGL;

        if (!isWebGL)
        {
            return;
        }


        // var compas = Resources.FindObjectsOfTypeAll<VideoPlayerWebCompatible>();
        var compas = SearchAllScene<VideoPlayerWebCompatible>();
        foreach (var compa in compas)
        {
            if (!compa.useClip || compa.clip == null)
            {
                continue;
            }
            string vdPath = AssetDatabase.GetAssetPath(compa.clip);
            string dst = CopyVideoToTemp(vdPath, compa.webglCompatiblePath);
            Debug.Log($"Copy from {vdPath} to temp {dst}");
        }
    }

    public List<T> SearchAllScene<T>()
    {
         if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            throw new BuildFailedException("Cancelled due to unsaved scenes.");

        var all = new List<T>();

        foreach (var s in EditorBuildSettings.scenes)
        {
            if (!s.enabled) continue;

            var scene = EditorSceneManager.OpenScene(s.path, OpenSceneMode.Additive);

            foreach (var root in scene.GetRootGameObjects())
                all.AddRange(root.GetComponentsInChildren<T>(true)); // true: include inactive

            EditorSceneManager.CloseScene(scene, removeScene: true);
        }

        Debug.Log($"[Prebuild] Found {all.Count} {nameof(T)} in all Scenes.");
        return all;
    }

    public string CopyVideoToTemp(string path, string to)
    {
        string tmpRoot = Path.Combine(TEMP_ROOT, VideoPlayerWebCompatible.VIDEO_PATH);
        string dst = Path.Combine(tmpRoot, to.TrimStart('/','\\'));
        Directory.CreateDirectory(Path.GetDirectoryName(dst));
        // FileUtil.CopyFileOrDirectory(path, dst);
        FileUtil.ReplaceFile(path, dst);
        return dst;
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        string tmpRoot = Path.Combine(TEMP_ROOT, VideoPlayerWebCompatible.VIDEO_PATH);
        string saRoot = Path.Combine(report.summary.outputPath, "StreamingAssets", VideoPlayerWebCompatible.VIDEO_PATH);
        // FileUtil.CopyFileOrDirectory(tmpRoot, saRoot);
        Debug.Log($"Copy from {tmpRoot} to temp {saRoot}");
        FileUtil.ReplaceDirectory(tmpRoot, saRoot);
    }
}
