using UnityEditor;
using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class AnimationPostprocessor : AssetPostprocessor
    {
        private const string AnimationAssetPath = "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/Animations";
        private const string AnimationTargetPath = "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/AnimationTargets";

        private const string MaleAnimationTargetName = "AnimationTargets/MaleAnimationTargetV2";
        private const string FemaleAnimationTargetName = "AnimationTargets/FemaleAnimationTargetV2";

        private static readonly string[] AnimationFiles = new string[]
        {
        "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/Animations/Female/FemaleAnimationTargetV2@Breathing Idle.fbx",
        "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/Animations/Female/FemaleAnimationTargetV2@Walking.fbx",
        "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/Animations/Male/MaleAnimationTargetV2@Breathing Idle.fbx",
        "Assets/Plugins/Wolf3D Ready Player Me SDK/Resources/Animations/Male/MaleAnimationTargetV2@Walking.fbx",
        };

        private void OnPreprocessModel()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;

            void SetModelImportData()
            {
                modelImporter.useFileScale = false;
                modelImporter.animationType = ModelImporterAnimationType.Human;
            }

            if (assetPath.Contains(AnimationAssetPath))
            {
                SetModelImportData();

                bool isFemaleFolder = assetPath.Contains("Female");
                GameObject animationTarget = Resources.Load<GameObject>(isFemaleFolder ? FemaleAnimationTargetName : MaleAnimationTargetName);

                if (animationTarget != null)
                {
                    modelImporter.sourceAvatar = animationTarget.GetComponent<Animator>().avatar;
                }
            }
            else if (assetPath.Contains(AnimationTargetPath))
            {
                SetModelImportData();
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string item in importedAssets)
            {
                if (item.Contains(MaleAnimationTargetName))
                {
                    for (int i = 0; i < AnimationFiles.Length; i++)
                    {
                        AssetDatabase.ImportAsset(AnimationFiles[i]);
                    }
                }
            }
        }
    }
}