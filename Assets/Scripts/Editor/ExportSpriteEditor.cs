using UnityEngine;
using UnityEditor;

/// <summary>
/// �������鹤��
/// </summary>
public class ExportSpriteEditor
{
    [MenuItem("Tools/��������")]
    static void ExportSprite()
    {
        string resourcesPath = "Assets/Resources/";
        foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath(obj);
            if (selectionPath.StartsWith(resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension(selectionPath);
                if (selectionExt.Length == 0)
                {
                    Debug.LogError($"��Դ{selectionPath}����չ�����ԣ���ѡ��ͼƬ��Դ");
                    continue;
                }
                // ���selectionPath = "Assets/Resources/UI/Common.png"
                // ��ôloadPath = "UI/Common"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);
                // ���ش��ļ��µ�������Դ
                Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
                if (sprites.Length > 0)
                {
                    // ��������Ŀ¼
                    string exportPath = Application.dataPath + "/ExportSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(exportPath);

                    foreach (Sprite sprite in sprites)
                    {
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height,
                            sprite.texture.format, false);
                        tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                            (int)sprite.rect.width, (int)sprite.rect.height));
                        tex.Apply();

                        // ��ͼƬ����д���ļ�
                        System.IO.File.WriteAllBytes(exportPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                    }
                    Debug.Log("�������鵽" + exportPath);
                }
                Debug.Log("�����������");
                // ˢ����Դ
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"�뽫��Դ����{resourcesPath}Ŀ¼��");
            }
        }
    }
}