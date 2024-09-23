using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class AutoAddRemoveSceneFromBuildSettings : AssetPostprocessor
{
    // 씬이 저장되거나 추가될 때 호출되는 메서드
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.EndsWith(".unity"))
            {
                AddSceneToBuildSettings(asset);
            }
        }

        foreach (string asset in deletedAssets)
        {
            if (asset.EndsWith(".unity"))
            {
                RemoveSceneFromBuildSettings(asset);
            }
        }
    }

    // 씬을 빌드 설정에 추가하는 메서드
    private static void AddSceneToBuildSettings(string scenePath)
    {
        var buildScenes = EditorBuildSettings.scenes;

        // 이미 빌드 설정에 있는지 확인
        foreach (var scene in buildScenes)
        {
            if (scene.path == scenePath)
                return;
        }

        // 새로운 씬 배열 생성
        var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
        System.Array.Copy(buildScenes, newBuildScenes, buildScenes.Length);

        // 새로운 씬 추가
        var newScene = new EditorBuildSettingsScene(scenePath, true);
        newBuildScenes[newBuildScenes.Length - 1] = newScene;

        // 빌드 설정 업데이트
        EditorBuildSettings.scenes = newBuildScenes;
        Debug.Log($"Scene '{scenePath}' has been added to the Build Settings.");
    }

    // 씬을 빌드 설정에서 제거하는 메서드
    private static void RemoveSceneFromBuildSettings(string scenePath)
    {
        var buildScenes = EditorBuildSettings.scenes;
        int index = -1;

        // 삭제된 씬의 인덱스를 찾기
        for (int i = 0; i < buildScenes.Length; i++)
        {
            if (buildScenes[i].path == scenePath)
            {
                index = i;
                break;
            }
        }

        // 씬이 빌드 설정에 있는 경우에만 제거
        if (index != -1)
        {
            var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length - 1];
            if (index > 0)
                System.Array.Copy(buildScenes, 0, newBuildScenes, 0, index);
            if (index < buildScenes.Length - 1)
                System.Array.Copy(buildScenes, index + 1, newBuildScenes, index, buildScenes.Length - index - 1);

            // 빌드 설정 업데이트
            EditorBuildSettings.scenes = newBuildScenes;
            Debug.Log($"Scene '{scenePath}' has been removed from the Build Settings.");
        }
    }
}
