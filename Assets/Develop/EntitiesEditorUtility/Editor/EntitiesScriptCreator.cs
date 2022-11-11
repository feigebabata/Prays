using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace EntitiesEditorUtility
{
    public static class EntitiesScriptCreator
    {
        const string NAMESPACE_KEY = "EntitiesScriptCreator.Namespace";

        [MenuItem("Assets/Create/Entities/Comp",false,80)]
        static void createComp()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateScriptAction>(),
            "New Comp",null,"Comp");
        }

        [MenuItem("Assets/Create/Entities/CompAndAuthing",false,80)]
        static void createCompAndAuthing()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateScriptAction>(),
            "New CompAndAuthing",null,"CompAndAuthing");
        }

        [MenuItem("Assets/Create/Entities/ISys",false,80)]
        static void createISys()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateScriptAction>(),
            "New ISys",null,"ISys");
        }

        [MenuItem("Assets/Create/Entities/IJobSys",false,80)]
        static void createIJobSys()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateScriptAction>(),
            "New ISys",null,"IJobSys");
        }

        [MenuItem("Assets/Create/Entities/SetNamespace",false,80)]
        static void SetNamespace()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateScriptAction>(),
            "New Namespace",null,"SetNamespace");
        }

        static string getNamespace()
        {
            return EditorPrefs.GetString(NAMESPACE_KEY,"Temp");
        }

        class CreateScriptAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string name = Path.GetFileName(pathName);
                string outPath = pathName+".cs";
                UnityEngine.Object obj = null;
                switch (resourceFile)
                {
                    case "SetNamespace":
                    {
                        EditorPrefs.SetString(NAMESPACE_KEY,name);
                    }
                    break;
                    case "Comp":
                        obj = createCompScript(name,outPath);
                    break;
                    case "CompAndAuthing":
                        obj = createCompAndAuthingScript(name,outPath);
                    break;
                    case "ISys":
                        obj = createISysScript(name,outPath);
                    break;
                    case "IJobSys":
                        obj = createIJobSysScript(name,outPath);
                    break;
                }
                
                if(obj!=null)ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
            }

            static UnityEngine.Object createIJobSysScript(string name, string outPath)
            {
                string scriptText = 
@"using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

namespace |NAMESPACE|
{
    [BurstCompile]
    public partial struct |NAME| : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            
            var job = new |NAME|Job
            {
                ECB = ecb
            };
            job.Schedule();
        }

    }

    [BurstCompile]
    public partial struct |NAME|Job : IJobEntity
    {
        public EntityCommandBuffer ECB;

        void Execute()
        {

        }
    }
}
";
                scriptText = scriptText.Replace("|NAMESPACE|",getNamespace());
                scriptText = scriptText.Replace("|NAME|",name);
                File.WriteAllText(outPath,scriptText);
                //刷新资源管理器
                AssetDatabase.ImportAsset(outPath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(outPath, typeof(UnityEngine.Object));
            }

            static UnityEngine.Object createISysScript(string name, string outPath)
            {
                string scriptText = 
@"using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace |NAMESPACE|
{
    [BurstCompile]
    public partial struct |NAME| : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
        }

    }
}
";
                scriptText = scriptText.Replace("|NAMESPACE|",getNamespace());
                scriptText = scriptText.Replace("|NAME|",name);
                File.WriteAllText(outPath,scriptText);
                //刷新资源管理器
                AssetDatabase.ImportAsset(outPath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(outPath, typeof(UnityEngine.Object));
            }

            static UnityEngine.Object createCompAndAuthingScript(string name, string outPath)
            {
                outPath = outPath.Replace(".cs","Authoring.cs");
        
                string scriptText = 
@"using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace |NAMESPACE|
{

    public struct |NAME| : IComponentData
    {

    }
    
    class |NAME|Authoring : MonoBehaviour
    {

    }

    class |NAME|Baker : Baker<|NAME|Authoring>
    {
        public override void Bake(|NAME|Authoring authoring)
        {
            AddComponent(new |NAME|
            {
                
            });
        }
    }
}
";
                scriptText = scriptText.Replace("|NAMESPACE|",getNamespace());
                scriptText = scriptText.Replace("|NAME|",name);
                File.WriteAllText(outPath,scriptText);
                //刷新资源管理器
                AssetDatabase.ImportAsset(outPath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(outPath, typeof(UnityEngine.Object));
            }

            static UnityEngine.Object createCompScript(string name, string outPath)
            {
                string scriptText = 
@"using Unity.Entities;
using Unity.Mathematics;

namespace |NAMESPACE|
{
    public struct |NAME| : IComponentData
    {

    }
}
";
                scriptText = scriptText.Replace("|NAMESPACE|",getNamespace());
                scriptText = scriptText.Replace("|NAME|",name);
                File.WriteAllText(outPath,scriptText);
                //刷新资源管理器
                AssetDatabase.ImportAsset(outPath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(outPath, typeof(UnityEngine.Object));
            }
    

        }

    }
}